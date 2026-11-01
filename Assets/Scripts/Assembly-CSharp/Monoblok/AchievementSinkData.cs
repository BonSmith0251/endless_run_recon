using UnityEngine;

namespace Monoblok
{
	public abstract class AchievementSinkData : ScriptableObject
	{
		[SerializeField]
		private SteamTrackable trackable;

		public SteamTrackable Trackable => trackable;

		public abstract AchievementSink GetSink();
	}
}
