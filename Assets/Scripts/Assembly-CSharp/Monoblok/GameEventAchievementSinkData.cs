using Monoblok.SA;
using UnityEngine;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Steam/Achievement Sink/Game Event")]
	public class GameEventAchievementSinkData : AchievementSinkData
	{
		[SerializeField]
		private GameEvent gameEvent;

		[SerializeField]
		[Min(1f)]
		private int targetCount = 1;

		[SerializeReference]
		[SubclassSelector]
		private IGameEventSinkCondition[] conditions;

		public override AchievementSink GetSink()
		{
			return new GameEventAchievementSink(gameEvent, base.Trackable, targetCount, conditions);
		}
	}
}
