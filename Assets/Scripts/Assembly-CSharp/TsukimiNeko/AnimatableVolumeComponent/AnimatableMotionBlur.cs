using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(MotionBlur))]
	[DisallowMultipleComponent]
	public class AnimatableMotionBlur : AnimatableVolumeComponentBase
	{
		public bool override_mode;

		public MotionBlurMode mode;

		public bool override_quality;

		public MotionBlurQuality quality;

		public bool override_intensity;

		public float intensity;

		public bool override_clamp;

		public float clamp;

		public override Type TargetType { get; } = typeof(MotionBlur);


		private void WriteToVolumeComponent(MotionBlur volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.mode.overrideState = override_mode;
				volumeComponent.mode.value = mode;
				volumeComponent.quality.overrideState = override_quality;
				volumeComponent.quality.value = quality;
				volumeComponent.intensity.overrideState = override_intensity;
				volumeComponent.intensity.value = intensity;
				volumeComponent.clamp.overrideState = override_clamp;
				volumeComponent.clamp.value = clamp;
			}
		}

		private void ReadFromVolumeComponent(MotionBlur volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_mode = volumeComponent.mode.overrideState;
				mode = volumeComponent.mode.value;
				override_quality = volumeComponent.quality.overrideState;
				quality = volumeComponent.quality.value;
				override_intensity = volumeComponent.intensity.overrideState;
				intensity = volumeComponent.intensity.value;
				override_clamp = volumeComponent.clamp.overrideState;
				clamp = volumeComponent.clamp.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<MotionBlur>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<MotionBlur>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<MotionBlur>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
