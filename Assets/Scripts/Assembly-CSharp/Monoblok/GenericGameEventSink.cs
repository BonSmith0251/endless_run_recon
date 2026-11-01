using Monoblok.SA;
using UnityEngine;

namespace Monoblok
{
	public abstract class GenericGameEventSink<T> : AchievementSink
	{
		private readonly GenericGameEvent<T> gameEvent;

		private readonly IGenericGameEventSinkCondition<T>[] conditions;

		protected GenericGameEventSink(GenericGameEvent<T> gameEvent, IGenericGameEventSinkCondition<T>[] conditions, SteamTrackable trackable)
			: base(trackable)
		{
			this.gameEvent = gameEvent;
			this.conditions = conditions;
		}

		protected override void OnInitialize()
		{
			if (gameEvent == null)
			{
				Debug.LogError("No game event set");
			}
			else
			{
				gameEvent.OnTriggered += HandleGameEventTriggered;
			}
		}

		protected override void OnDispose()
		{
			if (!(gameEvent == null))
			{
				gameEvent.OnTriggered -= HandleGameEventTriggered;
			}
		}

		private void HandleGameEventTriggered(T t)
		{
			if (conditions != null)
			{
				for (int i = 0; i < conditions.Length; i++)
				{
					IGenericGameEventSinkCondition<T> genericGameEventSinkCondition = conditions[i];
					if (genericGameEventSinkCondition != null && !genericGameEventSinkCondition.Evaluate(t))
					{
						return;
					}
				}
			}
			Notify();
		}
	}
}
