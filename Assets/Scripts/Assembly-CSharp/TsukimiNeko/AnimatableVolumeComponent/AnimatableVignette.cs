using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(Vignette))]
	[DisallowMultipleComponent]
	public class AnimatableVignette : AnimatableVolumeComponentBase
	{
		public bool override_color;

		public Color color;

		public bool override_center;

		public Vector2 center;

		public bool override_intensity;

		public float intensity;

		public bool override_smoothness;

		public float smoothness;

		public bool override_rounded;

		public bool rounded;

		public override Type TargetType { get; } = typeof(Vignette);


		private void WriteToVolumeComponent(Vignette volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.color.overrideState = override_color;
				volumeComponent.color.value = color;
				volumeComponent.center.overrideState = override_center;
				volumeComponent.center.value = center;
				volumeComponent.intensity.overrideState = override_intensity;
				volumeComponent.intensity.value = intensity;
				volumeComponent.smoothness.overrideState = override_smoothness;
				volumeComponent.smoothness.value = smoothness;
				volumeComponent.rounded.overrideState = override_rounded;
				volumeComponent.rounded.value = rounded;
			}
		}

		private void ReadFromVolumeComponent(Vignette volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_color = volumeComponent.color.overrideState;
				color = volumeComponent.color.value;
				override_center = volumeComponent.center.overrideState;
				center = volumeComponent.center.value;
				override_intensity = volumeComponent.intensity.overrideState;
				intensity = volumeComponent.intensity.value;
				override_smoothness = volumeComponent.smoothness.overrideState;
				smoothness = volumeComponent.smoothness.value;
				override_rounded = volumeComponent.rounded.overrideState;
				rounded = volumeComponent.rounded.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<Vignette>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<Vignette>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<Vignette>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
