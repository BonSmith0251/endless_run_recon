using Monoblok.Enemy;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Steam/Achievement Sink/Generic Game Event/Enemy")]
	public class EnemyGameEventSinkData : GenericGameEventSinkData<Monoblok.Enemy.Enemy>
	{
		protected override GenericGameEventSink<Monoblok.Enemy.Enemy> GetConcreteSink(GenericGameEvent<Monoblok.Enemy.Enemy> gameEvent, IGenericGameEventSinkCondition<Monoblok.Enemy.Enemy>[] conditions)
		{
			return new EnemyGameEventSink(gameEvent, conditions, base.Trackable);
		}
	}
}
