using UnityEngine;

namespace Monoblok
{
	// Offline build: Steam stats are disabled; the asset keeps its stat ID and settings.
	[CreateAssetMenu(menuName = "Steam/Trackable/Stat")]
	public class StatData : SteamTrackable
	{
		[SerializeField]
		private bool autoStore = true;

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
