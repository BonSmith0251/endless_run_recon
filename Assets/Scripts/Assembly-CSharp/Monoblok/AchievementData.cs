using UnityEngine;

namespace Monoblok
{
	// Offline build: Steam achievements are disabled; the asset keeps its achievement ID.
	[CreateAssetMenu(menuName = "Steam/Trackable/Achievement")]
	public class AchievementData : SteamTrackable
	{
		public override void Trigger(out bool achieved)
		{
			achieved = false;
		}

		public override bool ShouldTrack()
		{
			return false;
		}
	}
}
