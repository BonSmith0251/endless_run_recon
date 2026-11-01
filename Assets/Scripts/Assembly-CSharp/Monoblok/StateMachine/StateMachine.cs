using System;
using System.Collections.Generic;

namespace Monoblok.StateMachine
{
	public class StateMachine
	{
		private class AnyState : IState
		{
			public void Enter()
			{
			}

			public void Update(float deltaTime)
			{
			}

			public void Exit()
			{
			}
		}

		private IState current;

		private readonly List<ITransition> transitions = new List<ITransition>();

		private readonly Dictionary<ITransition, Action> handlers = new Dictionary<ITransition, Action>();

		private IState initialState;

		private ITransition currentTransition;

		private bool isStopped;

		private static readonly IState FromAny = new AnyState();

		public Type CurrentStateType => current?.GetType();

		public void AddTransition(IState from, IState to, ICondition condition, ICompletable completable = null)
		{
			transitions.Add(new Transition(from, to, condition, completable));
		}

		public void AddInstantTransition(IState from, IState to, ICompletable completable = null)
		{
			AddTransition(from, to, new InstantCondition(), completable);
		}

		public void AddTransitionFromAny(IState to, ICondition condition, ICompletable completable = null)
		{
			transitions.Add(new Transition(FromAny, to, condition, completable));
		}

		public void SetInitialState(IState state)
		{
			initialState = state;
			ChangeState(initialState);
		}

		public void Stop()
		{
			isStopped = true;
			ExitCurrentState();
		}

		public void Restart()
		{
			isStopped = false;
			ExitCurrentState();
			ChangeState(initialState);
		}

		public void ForceState(IState state)
		{
			if (!isStopped)
			{
				ExitCurrentState();
				ChangeState(state);
			}
		}

		public void Update(float deltaTime)
		{
			if (!isStopped && current != null)
			{
				current.Update(deltaTime);
			}
		}

		private void ChangeState(IState state)
		{
			current = state;
			current.Enter();
			foreach (ITransition tr in GetTransitions(state))
			{
				if (tr.Condition != null)
				{
					Action value = delegate
					{
						OnConditionMet(tr);
					};
					handlers[tr] = value;
					tr.Condition.OnMet += value;
					tr.Condition.Initialize();
				}
			}
		}

		private void ExitCurrentState()
		{
			if (current == null)
			{
				return;
			}
			foreach (ITransition transition in GetTransitions(current))
			{
				if (handlers.TryGetValue(transition, out var value))
				{
					transition.Condition.OnMet -= value;
				}
				transition.Condition.Dispose();
			}
			handlers.Clear();
			current.Exit();
			current = null;
		}

		private void StartTransition(ITransition tr)
		{
			ExitCurrentState();
			if (tr.Completable == null)
			{
				ChangeState(tr.To);
				return;
			}
			tr.Completable.OnComplete += OnTransitionComplete;
			tr.Completable.Initialize();
			currentTransition = tr;
		}

		private void OnTransitionComplete()
		{
			IState to = currentTransition.To;
			currentTransition.Completable.OnComplete -= OnTransitionComplete;
			currentTransition = null;
			ChangeState(to);
		}

		private void OnConditionMet(ITransition tr)
		{
			if (currentTransition == null)
			{
				StartTransition(tr);
			}
		}

		private IEnumerable<ITransition> GetTransitions(IState from)
		{
			foreach (ITransition transition in transitions)
			{
				if (transition.From == from)
				{
					yield return transition;
				}
				else if (transition.From == FromAny && transition.To != from)
				{
					yield return transition;
				}
			}
		}

		public void Dispose()
		{
			foreach (ITransition transition in transitions)
			{
				if (transition?.Condition != null)
				{
					if (handlers.TryGetValue(transition, out var value))
					{
						transition.Condition.OnMet -= value;
					}
					transition.Condition.Dispose();
				}
			}
		}
	}
}
