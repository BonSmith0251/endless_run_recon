using Monoblok.Enemy;
using Monoblok.SA;

namespace Monoblok
{
	public class EnemyGameEventSink : GenericGameEventSink<Monoblok.Enemy.Enemy>
	{
		public EnemyGameEventSink(GenericGameEvent<Monoblok.Enemy.Enemy> gameEvent, IGenericGameEventSinkCondition<Monoblok.Enemy.Enemy>[] conditions, SteamTrackable trackable)
			: base(gameEvent, conditions, trackable)
		{
		}
	}
}
