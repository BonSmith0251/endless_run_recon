namespace Monoblok.StateMachine
{
	public class Transition : ITransition
	{
		public IState From { get; }

		public IState To { get; }

		public ICondition Condition { get; }

		public ICompletable Completable { get; }

		public Transition(IState from, IState to, ICondition condition, ICompletable completable)
		{
			From = from;
			To = to;
			Condition = condition;
			Completable = completable;
		}
	}
}
