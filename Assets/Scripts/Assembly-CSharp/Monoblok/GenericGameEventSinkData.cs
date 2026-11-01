using System.Linq;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok
{
	public abstract class GenericGameEventSinkData<T> : AchievementSinkData
	{
		[SerializeField]
		private GenericGameEvent<T> gameEvent;

		[SerializeReference]
		[SubclassSelector]
		private IGenericGameEventSinkConditionData[] conditions;

		public sealed override AchievementSink GetSink()
		{
			return GetConcreteSink(gameEvent, conditions?.Cast<IGenericGameEventSinkCondition<T>>().ToArray());
		}

		protected abstract GenericGameEventSink<T> GetConcreteSink(GenericGameEvent<T> gameEvent, IGenericGameEventSinkCondition<T>[] conditions);
	}
}
