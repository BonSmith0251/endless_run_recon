using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Monoblok.Timeline
{
	[TrackClipType(typeof(TypewriterClip))]
	[TrackBindingType(typeof(TMP_Text))]
	public class TypewriterTrack : TrackAsset
	{
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			ScriptPlayable<TypewriterMixerBehaviour> scriptPlayable = ScriptPlayable<TypewriterMixerBehaviour>.Create(graph, inputCount);
			TypewriterMixerBehaviour behaviour = scriptPlayable.GetBehaviour();
			behaviour.clipStartTimes = new double[inputCount];
			behaviour.clipEndTimes = new double[inputCount];
			int num = 0;
			foreach (TimelineClip clip in GetClips())
			{
				behaviour.clipStartTimes[num] = clip.start;
				behaviour.clipEndTimes[num] = clip.end;
				num++;
			}
			return scriptPlayable;
		}
	}
}
