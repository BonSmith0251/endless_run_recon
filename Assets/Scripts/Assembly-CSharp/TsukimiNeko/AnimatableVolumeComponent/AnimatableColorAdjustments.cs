using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(ColorAdjustments))]
	[DisallowMultipleComponent]
	public class AnimatableColorAdjustments : AnimatableVolumeComponentBase
	{
		public bool override_postExposure;

		public float postExposure;

		public bool override_contrast;

		public float contrast;

		public bool override_colorFilter;

		public Color colorFilter;

		public bool override_hueShift;

		public float hueShift;

		public bool override_saturation;

		public float saturation;

		public override Type TargetType { get; } = typeof(ColorAdjustments);


		private void WriteToVolumeComponent(ColorAdjustments volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.postExposure.overrideState = override_postExposure;
				volumeComponent.postExposure.value = postExposure;
				volumeComponent.contrast.overrideState = override_contrast;
				volumeComponent.contrast.value = contrast;
				volumeComponent.colorFilter.overrideState = override_colorFilter;
				volumeComponent.colorFilter.value = colorFilter;
				volumeComponent.hueShift.overrideState = override_hueShift;
				volumeComponent.hueShift.value = hueShift;
				volumeComponent.saturation.overrideState = override_saturation;
				volumeComponent.saturation.value = saturation;
			}
		}

		private void ReadFromVolumeComponent(ColorAdjustments volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_postExposure = volumeComponent.postExposure.overrideState;
				postExposure = volumeComponent.postExposure.value;
				override_contrast = volumeComponent.contrast.overrideState;
				contrast = volumeComponent.contrast.value;
				override_colorFilter = volumeComponent.colorFilter.overrideState;
				colorFilter = volumeComponent.colorFilter.value;
				override_hueShift = volumeComponent.hueShift.overrideState;
				hueShift = volumeComponent.hueShift.value;
				override_saturation = volumeComponent.saturation.overrideState;
				saturation = volumeComponent.saturation.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<ColorAdjustments>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ColorAdjustments>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ColorAdjustments>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
