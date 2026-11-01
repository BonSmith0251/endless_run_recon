using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(ChromaticAberration))]
	[DisallowMultipleComponent]
	public class AnimatableChromaticAberration : AnimatableVolumeComponentBase
	{
		public bool override_intensity;

		public float intensity;

		public override Type TargetType { get; } = typeof(ChromaticAberration);


		private void WriteToVolumeComponent(ChromaticAberration volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.intensity.overrideState = override_intensity;
				volumeComponent.intensity.value = intensity;
			}
		}

		private void ReadFromVolumeComponent(ChromaticAberration volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_intensity = volumeComponent.intensity.overrideState;
				intensity = volumeComponent.intensity.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<ChromaticAberration>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ChromaticAberration>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ChromaticAberration>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
