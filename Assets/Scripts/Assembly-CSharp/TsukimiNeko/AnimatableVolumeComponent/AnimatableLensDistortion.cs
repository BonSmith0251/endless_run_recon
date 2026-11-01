using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(LensDistortion))]
	[DisallowMultipleComponent]
	public class AnimatableLensDistortion : AnimatableVolumeComponentBase
	{
		public bool override_intensity;

		public float intensity;

		public bool override_xMultiplier;

		public float xMultiplier;

		public bool override_yMultiplier;

		public float yMultiplier;

		public bool override_center;

		public Vector2 center;

		public bool override_scale;

		public float scale;

		public override Type TargetType { get; } = typeof(LensDistortion);


		private void WriteToVolumeComponent(LensDistortion volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.intensity.overrideState = override_intensity;
				volumeComponent.intensity.value = intensity;
				volumeComponent.xMultiplier.overrideState = override_xMultiplier;
				volumeComponent.xMultiplier.value = xMultiplier;
				volumeComponent.yMultiplier.overrideState = override_yMultiplier;
				volumeComponent.yMultiplier.value = yMultiplier;
				volumeComponent.center.overrideState = override_center;
				volumeComponent.center.value = center;
				volumeComponent.scale.overrideState = override_scale;
				volumeComponent.scale.value = scale;
			}
		}

		private void ReadFromVolumeComponent(LensDistortion volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_intensity = volumeComponent.intensity.overrideState;
				intensity = volumeComponent.intensity.value;
				override_xMultiplier = volumeComponent.xMultiplier.overrideState;
				xMultiplier = volumeComponent.xMultiplier.value;
				override_yMultiplier = volumeComponent.yMultiplier.overrideState;
				yMultiplier = volumeComponent.yMultiplier.value;
				override_center = volumeComponent.center.overrideState;
				center = volumeComponent.center.value;
				override_scale = volumeComponent.scale.overrideState;
				scale = volumeComponent.scale.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<LensDistortion>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<LensDistortion>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<LensDistortion>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
