using Monoblok.SA;
using UnityEngine;

namespace Monoblok
{
	public class GameEventAchievementSink : AchievementSink
	{
		private readonly GameEvent gameEvent;

		private readonly int targetCount;

		private readonly IGameEventSinkCondition[] conditions;

		private int counter;

		public GameEventAchievementSink(GameEvent gameEvent, SteamTrackable trackable, int targetCount, IGameEventSinkCondition[] conditions)
			: base(trackable)
		{
			this.gameEvent = gameEvent;
			this.targetCount = targetCount;
			this.conditions = conditions;
		}

		protected override void OnInitialize()
		{
			if (gameEvent == null)
			{
				Debug.LogError("No game event set");
				return;
			}
			gameEvent.OnTriggered += HandleTrigger;
			Debug.Log("Registering game event sink");
		}

		private void HandleTrigger()
		{
			if (conditions != null)
			{
				for (int i = 0; i < conditions.Length; i++)
				{
					if (!conditions[i].IsEligible())
					{
						return;
					}
				}
			}
			counter++;
			if (counter >= targetCount)
			{
				Notify();
			}
		}

		protected override void OnDispose()
		{
			if (!(gameEvent == null))
			{
				gameEvent.OnTriggered -= HandleTrigger;
				Debug.Log("Disposing Game event sink");
			}
		}
	}
}
