using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(ScreenSpaceLensFlare))]
	[DisallowMultipleComponent]
	public class AnimatableScreenSpaceLensFlare : AnimatableVolumeComponentBase
	{
		public bool override_intensity;

		public float intensity;

		public bool override_tintColor;

		public Color tintColor;

		public bool override_bloomMip;

		public int bloomMip;

		public bool override_firstFlareIntensity;

		public float firstFlareIntensity;

		public bool override_secondaryFlareIntensity;

		public float secondaryFlareIntensity;

		public bool override_warpedFlareIntensity;

		public float warpedFlareIntensity;

		public bool override_warpedFlareScale;

		public Vector2 warpedFlareScale;

		public bool override_samples;

		public int samples;

		public bool override_sampleDimmer;

		public float sampleDimmer;

		public bool override_vignetteEffect;

		public float vignetteEffect;

		public bool override_startingPosition;

		public float startingPosition;

		public bool override_scale;

		public float scale;

		public bool override_streaksIntensity;

		public float streaksIntensity;

		public bool override_streaksLength;

		public float streaksLength;

		public bool override_streaksOrientation;

		public float streaksOrientation;

		public bool override_streaksThreshold;

		public float streaksThreshold;

		public bool override_resolution;

		public ScreenSpaceLensFlareResolution resolution;

		public bool override_chromaticAbberationIntensity;

		public float chromaticAbberationIntensity;

		public override Type TargetType { get; } = typeof(ScreenSpaceLensFlare);


		private void WriteToVolumeComponent(ScreenSpaceLensFlare volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.intensity.overrideState = override_intensity;
				volumeComponent.intensity.value = intensity;
				volumeComponent.tintColor.overrideState = override_tintColor;
				volumeComponent.tintColor.value = tintColor;
				volumeComponent.bloomMip.overrideState = override_bloomMip;
				volumeComponent.bloomMip.value = bloomMip;
				volumeComponent.firstFlareIntensity.overrideState = override_firstFlareIntensity;
				volumeComponent.firstFlareIntensity.value = firstFlareIntensity;
				volumeComponent.secondaryFlareIntensity.overrideState = override_secondaryFlareIntensity;
				volumeComponent.secondaryFlareIntensity.value = secondaryFlareIntensity;
				volumeComponent.warpedFlareIntensity.overrideState = override_warpedFlareIntensity;
				volumeComponent.warpedFlareIntensity.value = warpedFlareIntensity;
				volumeComponent.warpedFlareScale.overrideState = override_warpedFlareScale;
				volumeComponent.warpedFlareScale.value = warpedFlareScale;
				volumeComponent.samples.overrideState = override_samples;
				volumeComponent.samples.value = samples;
				volumeComponent.sampleDimmer.overrideState = override_sampleDimmer;
				volumeComponent.sampleDimmer.value = sampleDimmer;
				volumeComponent.vignetteEffect.overrideState = override_vignetteEffect;
				volumeComponent.vignetteEffect.value = vignetteEffect;
				volumeComponent.startingPosition.overrideState = override_startingPosition;
				volumeComponent.startingPosition.value = startingPosition;
				volumeComponent.scale.overrideState = override_scale;
				volumeComponent.scale.value = scale;
				volumeComponent.streaksIntensity.overrideState = override_streaksIntensity;
				volumeComponent.streaksIntensity.value = streaksIntensity;
				volumeComponent.streaksLength.overrideState = override_streaksLength;
				volumeComponent.streaksLength.value = streaksLength;
				volumeComponent.streaksOrientation.overrideState = override_streaksOrientation;
				volumeComponent.streaksOrientation.value = streaksOrientation;
				volumeComponent.streaksThreshold.overrideState = override_streaksThreshold;
				volumeComponent.streaksThreshold.value = streaksThreshold;
				volumeComponent.resolution.overrideState = override_resolution;
				volumeComponent.resolution.value = resolution;
				volumeComponent.chromaticAbberationIntensity.overrideState = override_chromaticAbberationIntensity;
				volumeComponent.chromaticAbberationIntensity.value = chromaticAbberationIntensity;
			}
		}

		private void ReadFromVolumeComponent(ScreenSpaceLensFlare volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_intensity = volumeComponent.intensity.overrideState;
				intensity = volumeComponent.intensity.value;
				override_tintColor = volumeComponent.tintColor.overrideState;
				tintColor = volumeComponent.tintColor.value;
				override_bloomMip = volumeComponent.bloomMip.overrideState;
				bloomMip = volumeComponent.bloomMip.value;
				override_firstFlareIntensity = volumeComponent.firstFlareIntensity.overrideState;
				firstFlareIntensity = volumeComponent.firstFlareIntensity.value;
				override_secondaryFlareIntensity = volumeComponent.secondaryFlareIntensity.overrideState;
				secondaryFlareIntensity = volumeComponent.secondaryFlareIntensity.value;
				override_warpedFlareIntensity = volumeComponent.warpedFlareIntensity.overrideState;
				warpedFlareIntensity = volumeComponent.warpedFlareIntensity.value;
				override_warpedFlareScale = volumeComponent.warpedFlareScale.overrideState;
				warpedFlareScale = volumeComponent.warpedFlareScale.value;
				override_samples = volumeComponent.samples.overrideState;
				samples = volumeComponent.samples.value;
				override_sampleDimmer = volumeComponent.sampleDimmer.overrideState;
				sampleDimmer = volumeComponent.sampleDimmer.value;
				override_vignetteEffect = volumeComponent.vignetteEffect.overrideState;
				vignetteEffect = volumeComponent.vignetteEffect.value;
				override_startingPosition = volumeComponent.startingPosition.overrideState;
				startingPosition = volumeComponent.startingPosition.value;
				override_scale = volumeComponent.scale.overrideState;
				scale = volumeComponent.scale.value;
				override_streaksIntensity = volumeComponent.streaksIntensity.overrideState;
				streaksIntensity = volumeComponent.streaksIntensity.value;
				override_streaksLength = volumeComponent.streaksLength.overrideState;
				streaksLength = volumeComponent.streaksLength.value;
				override_streaksOrientation = volumeComponent.streaksOrientation.overrideState;
				streaksOrientation = volumeComponent.streaksOrientation.value;
				override_streaksThreshold = volumeComponent.streaksThreshold.overrideState;
				streaksThreshold = volumeComponent.streaksThreshold.value;
				override_resolution = volumeComponent.resolution.overrideState;
				resolution = volumeComponent.resolution.value;
				override_chromaticAbberationIntensity = volumeComponent.chromaticAbberationIntensity.overrideState;
				chromaticAbberationIntensity = volumeComponent.chromaticAbberationIntensity.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<ScreenSpaceLensFlare>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ScreenSpaceLensFlare>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ScreenSpaceLensFlare>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
