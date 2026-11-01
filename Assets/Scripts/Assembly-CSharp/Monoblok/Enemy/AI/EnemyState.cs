using System;
using System.Collections.Generic;
using Animancer;
using Monoblok.StatSystem;
using Monoblok.StateMachine;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public abstract class EnemyState : IState
	{
		protected readonly EnemyBrain brain;

		private readonly AnimationClip enterAnimation;

		private readonly EnemyStateConfig config;

		protected AnimancerState enterAnimationState;

		private List<Stat> modifiedStats = new List<Stat>();

		public event Action OnFinished;

		public EnemyState(EnemyBrain brain, EnemyStateConfig config)
		{
			this.config = config;
			this.brain = brain;
			enterAnimation = config.EnterAnimation;
		}

		public void Enter()
		{
			PlayAnimation();
			RemoveStatModifiers();
			ApplyStatModifiers();
			OnEnter();
		}

		public void Update(float deltaTime)
		{
			OnUpdate(deltaTime);
		}

		public void Exit()
		{
			RemoveStatModifiers();
			OnExit();
		}

		private void PlayAnimation()
		{
			if (!(enterAnimation == null))
			{
				enterAnimationState = brain.Animancer.Play(enterAnimation);
			}
		}

		private void ApplyStatModifiers()
		{
			if (config.ModifierDefinitions == null)
			{
				return;
			}
			StatModifierDefinition[] modifierDefinitions = config.ModifierDefinitions;
			foreach (StatModifierDefinition statModifierDefinition in modifierDefinitions)
			{
				Stat stat;
				if (statModifierDefinition.statDefinition == null)
				{
					Debug.LogError("No Stat Definition provided for state " + GetType().Name);
				}
				else if (brain.StatManager.TryGetStat(statModifierDefinition.statDefinition, out stat))
				{
					stat.AddModifier(statModifierDefinition.modifierFactory.GetModifier(this));
					modifiedStats.Add(stat);
				}
			}
		}

		private void RemoveStatModifiers()
		{
			foreach (Stat modifiedStat in modifiedStats)
			{
				modifiedStat?.RemoveModifiersFromSource(this);
			}
			modifiedStats.Clear();
		}

		protected abstract void OnEnter();

		protected abstract void OnUpdate(float deltaTime);

		protected abstract void OnExit();

		protected void Finish()
		{
			this.OnFinished?.Invoke();
		}
	}
}
