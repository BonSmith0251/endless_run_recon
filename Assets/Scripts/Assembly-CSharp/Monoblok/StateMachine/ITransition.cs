namespace Monoblok.StateMachine
{
	public interface ITransition
	{
		IState From { get; }

		IState To { get; }

		ICondition Condition { get; }

		ICompletable Completable { get; }
	}
}
