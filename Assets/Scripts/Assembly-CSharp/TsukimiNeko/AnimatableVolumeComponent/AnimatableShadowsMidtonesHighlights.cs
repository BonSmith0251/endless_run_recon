using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(ShadowsMidtonesHighlights))]
	[DisallowMultipleComponent]
	public class AnimatableShadowsMidtonesHighlights : AnimatableVolumeComponentBase
	{
		public bool override_shadows;

		public Vector4 shadows;

		public bool override_midtones;

		public Vector4 midtones;

		public bool override_highlights;

		public Vector4 highlights;

		public bool override_shadowsStart;

		public float shadowsStart;

		public bool override_shadowsEnd;

		public float shadowsEnd;

		public bool override_highlightsStart;

		public float highlightsStart;

		public bool override_highlightsEnd;

		public float highlightsEnd;

		public override Type TargetType { get; } = typeof(ShadowsMidtonesHighlights);


		private void WriteToVolumeComponent(ShadowsMidtonesHighlights volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.shadows.overrideState = override_shadows;
				volumeComponent.shadows.value = shadows;
				volumeComponent.midtones.overrideState = override_midtones;
				volumeComponent.midtones.value = midtones;
				volumeComponent.highlights.overrideState = override_highlights;
				volumeComponent.highlights.value = highlights;
				volumeComponent.shadowsStart.overrideState = override_shadowsStart;
				volumeComponent.shadowsStart.value = shadowsStart;
				volumeComponent.shadowsEnd.overrideState = override_shadowsEnd;
				volumeComponent.shadowsEnd.value = shadowsEnd;
				volumeComponent.highlightsStart.overrideState = override_highlightsStart;
				volumeComponent.highlightsStart.value = highlightsStart;
				volumeComponent.highlightsEnd.overrideState = override_highlightsEnd;
				volumeComponent.highlightsEnd.value = highlightsEnd;
			}
		}

		private void ReadFromVolumeComponent(ShadowsMidtonesHighlights volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_shadows = volumeComponent.shadows.overrideState;
				shadows = volumeComponent.shadows.value;
				override_midtones = volumeComponent.midtones.overrideState;
				midtones = volumeComponent.midtones.value;
				override_highlights = volumeComponent.highlights.overrideState;
				highlights = volumeComponent.highlights.value;
				override_shadowsStart = volumeComponent.shadowsStart.overrideState;
				shadowsStart = volumeComponent.shadowsStart.value;
				override_shadowsEnd = volumeComponent.shadowsEnd.overrideState;
				shadowsEnd = volumeComponent.shadowsEnd.value;
				override_highlightsStart = volumeComponent.highlightsStart.overrideState;
				highlightsStart = volumeComponent.highlightsStart.value;
				override_highlightsEnd = volumeComponent.highlightsEnd.overrideState;
				highlightsEnd = volumeComponent.highlightsEnd.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<ShadowsMidtonesHighlights>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ShadowsMidtonesHighlights>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ShadowsMidtonesHighlights>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
