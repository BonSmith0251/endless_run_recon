using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Monoblok.Timeline
{
	public class TypewriterClip : PlayableAsset, ITimelineClipAsset
	{
		public ClipCaps clipCaps => ClipCaps.None;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			return ScriptPlayable<TypewriterBehaviour>.Create(graph);
		}
	}
}
