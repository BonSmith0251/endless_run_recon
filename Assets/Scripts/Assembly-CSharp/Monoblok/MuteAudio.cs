using FMODUnity;
using UnityEngine;

namespace Monoblok
{
	public class MuteAudio : MonoBehaviour
	{
		public bool mute;

		private void Awake()
		{
			if (mute)
			{
				RuntimeManager.CoreSystem.getMasterChannelGroup(out var channelgroup);
				channelgroup.setVolume(0f);
			}
		}
	}
}
