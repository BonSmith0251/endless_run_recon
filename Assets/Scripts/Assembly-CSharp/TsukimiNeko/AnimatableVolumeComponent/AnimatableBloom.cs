using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(Bloom))]
	[DisallowMultipleComponent]
	public class AnimatableBloom : AnimatableVolumeComponentBase
	{
		public bool override_threshold;

		public float threshold;

		public bool override_intensity;

		public float intensity;

		public bool override_scatter;

		public float scatter;

		public bool override_clamp;

		public float clamp;

		public bool override_tint;

		public Color tint;

		public bool override_highQualityFiltering;

		public bool highQualityFiltering;

		public bool override_filter;

		public BloomFilterMode filter;

		public bool override_downscale;

		public BloomDownscaleMode downscale;

		public bool override_maxIterations;

		public int maxIterations;

		public bool override_dirtTexture;

		public Texture dirtTexture;

		public bool override_dirtIntensity;

		public float dirtIntensity;

		public override Type TargetType { get; } = typeof(Bloom);


		private void WriteToVolumeComponent(Bloom volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.threshold.overrideState = override_threshold;
				volumeComponent.threshold.value = threshold;
				volumeComponent.intensity.overrideState = override_intensity;
				volumeComponent.intensity.value = intensity;
				volumeComponent.scatter.overrideState = override_scatter;
				volumeComponent.scatter.value = scatter;
				volumeComponent.clamp.overrideState = override_clamp;
				volumeComponent.clamp.value = clamp;
				volumeComponent.tint.overrideState = override_tint;
				volumeComponent.tint.value = tint;
				volumeComponent.highQualityFiltering.overrideState = override_highQualityFiltering;
				volumeComponent.highQualityFiltering.value = highQualityFiltering;
				// URP 14 Bloom has no filter choice; it always uses the Gaussian filter (filter == 0).
				volumeComponent.downscale.overrideState = override_downscale;
				volumeComponent.downscale.value = downscale;
				volumeComponent.maxIterations.overrideState = override_maxIterations;
				volumeComponent.maxIterations.value = maxIterations;
				volumeComponent.dirtTexture.overrideState = override_dirtTexture;
				volumeComponent.dirtTexture.value = dirtTexture;
				volumeComponent.dirtIntensity.overrideState = override_dirtIntensity;
				volumeComponent.dirtIntensity.value = dirtIntensity;
			}
		}

		private void ReadFromVolumeComponent(Bloom volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_threshold = volumeComponent.threshold.overrideState;
				threshold = volumeComponent.threshold.value;
				override_intensity = volumeComponent.intensity.overrideState;
				intensity = volumeComponent.intensity.value;
				override_scatter = volumeComponent.scatter.overrideState;
				scatter = volumeComponent.scatter.value;
				override_clamp = volumeComponent.clamp.overrideState;
				clamp = volumeComponent.clamp.value;
				override_tint = volumeComponent.tint.overrideState;
				tint = volumeComponent.tint.value;
				override_highQualityFiltering = volumeComponent.highQualityFiltering.overrideState;
				highQualityFiltering = volumeComponent.highQualityFiltering.value;
				override_downscale = volumeComponent.downscale.overrideState;
				downscale = volumeComponent.downscale.value;
				override_maxIterations = volumeComponent.maxIterations.overrideState;
				maxIterations = volumeComponent.maxIterations.value;
				override_dirtTexture = volumeComponent.dirtTexture.overrideState;
				dirtTexture = volumeComponent.dirtTexture.value;
				override_dirtIntensity = volumeComponent.dirtIntensity.overrideState;
				dirtIntensity = volumeComponent.dirtIntensity.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<Bloom>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<Bloom>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<Bloom>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
