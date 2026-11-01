namespace Monoblok
{
	// Unity 2022.3 cannot [SerializeReference] a field whose element type is a generic interface
	// instance (Unity 6 can), so GenericGameEventSinkData stores conditions through this base.
	public interface IGenericGameEventSinkConditionData
	{
	}

	public interface IGenericGameEventSinkCondition<T> : IGenericGameEventSinkConditionData
	{
		bool Evaluate(T data);
	}
}
