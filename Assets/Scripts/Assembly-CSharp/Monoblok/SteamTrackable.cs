using UnityEngine;

namespace Monoblok
{
	public abstract class SteamTrackable : ScriptableObject
	{
		[SerializeField]
		private string id;

		public string ID => id;

		public abstract void Trigger(out bool achieved);

		public abstract bool ShouldTrack();
	}
}
