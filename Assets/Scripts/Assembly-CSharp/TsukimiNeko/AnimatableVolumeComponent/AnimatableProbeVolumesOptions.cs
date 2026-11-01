using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(ProbeVolumesOptions))]
	[DisallowMultipleComponent]
	public class AnimatableProbeVolumesOptions : AnimatableVolumeComponentBase
	{
		public bool override_normalBias;

		public float normalBias;

		public bool override_viewBias;

		public float viewBias;

		public bool override_scaleBiasWithMinProbeDistance;

		public bool scaleBiasWithMinProbeDistance;

		public bool override_samplingNoise;

		public float samplingNoise;

		public bool override_animateSamplingNoise;

		public bool animateSamplingNoise;

		public bool override_leakReductionMode;

		public APVLeakReductionMode leakReductionMode;

		public bool override_occlusionOnlyReflectionNormalization;

		public bool occlusionOnlyReflectionNormalization;

		public bool override_intensityMultiplier;

		public float intensityMultiplier;

		public bool override_skyOcclusionIntensityMultiplier;

		public float skyOcclusionIntensityMultiplier;

		public bool override_worldOffset;

		public Vector3 worldOffset;

		public override Type TargetType { get; } = typeof(ProbeVolumesOptions);


		private void WriteToVolumeComponent(ProbeVolumesOptions volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.normalBias.overrideState = override_normalBias;
				volumeComponent.normalBias.value = normalBias;
				volumeComponent.viewBias.overrideState = override_viewBias;
				volumeComponent.viewBias.value = viewBias;
				volumeComponent.scaleBiasWithMinProbeDistance.overrideState = override_scaleBiasWithMinProbeDistance;
				volumeComponent.scaleBiasWithMinProbeDistance.value = scaleBiasWithMinProbeDistance;
				volumeComponent.samplingNoise.overrideState = override_samplingNoise;
				volumeComponent.samplingNoise.value = samplingNoise;
				volumeComponent.animateSamplingNoise.overrideState = override_animateSamplingNoise;
				volumeComponent.animateSamplingNoise.value = animateSamplingNoise;
				volumeComponent.leakReductionMode.overrideState = override_leakReductionMode;
				volumeComponent.leakReductionMode.value = leakReductionMode;
				volumeComponent.occlusionOnlyReflectionNormalization.overrideState = override_occlusionOnlyReflectionNormalization;
				volumeComponent.occlusionOnlyReflectionNormalization.value = occlusionOnlyReflectionNormalization;
				volumeComponent.intensityMultiplier.overrideState = override_intensityMultiplier;
				volumeComponent.intensityMultiplier.value = intensityMultiplier;
				volumeComponent.skyOcclusionIntensityMultiplier.overrideState = override_skyOcclusionIntensityMultiplier;
				volumeComponent.skyOcclusionIntensityMultiplier.value = skyOcclusionIntensityMultiplier;
				volumeComponent.worldOffset.overrideState = override_worldOffset;
				volumeComponent.worldOffset.value = worldOffset;
			}
		}

		private void ReadFromVolumeComponent(ProbeVolumesOptions volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_normalBias = volumeComponent.normalBias.overrideState;
				normalBias = volumeComponent.normalBias.value;
				override_viewBias = volumeComponent.viewBias.overrideState;
				viewBias = volumeComponent.viewBias.value;
				override_scaleBiasWithMinProbeDistance = volumeComponent.scaleBiasWithMinProbeDistance.overrideState;
				scaleBiasWithMinProbeDistance = volumeComponent.scaleBiasWithMinProbeDistance.value;
				override_samplingNoise = volumeComponent.samplingNoise.overrideState;
				samplingNoise = volumeComponent.samplingNoise.value;
				override_animateSamplingNoise = volumeComponent.animateSamplingNoise.overrideState;
				animateSamplingNoise = volumeComponent.animateSamplingNoise.value;
				override_leakReductionMode = volumeComponent.leakReductionMode.overrideState;
				leakReductionMode = volumeComponent.leakReductionMode.value;
				override_occlusionOnlyReflectionNormalization = volumeComponent.occlusionOnlyReflectionNormalization.overrideState;
				occlusionOnlyReflectionNormalization = volumeComponent.occlusionOnlyReflectionNormalization.value;
				override_intensityMultiplier = volumeComponent.intensityMultiplier.overrideState;
				intensityMultiplier = volumeComponent.intensityMultiplier.value;
				override_skyOcclusionIntensityMultiplier = volumeComponent.skyOcclusionIntensityMultiplier.overrideState;
				skyOcclusionIntensityMultiplier = volumeComponent.skyOcclusionIntensityMultiplier.value;
				override_worldOffset = volumeComponent.worldOffset.overrideState;
				worldOffset = volumeComponent.worldOffset.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<ProbeVolumesOptions>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ProbeVolumesOptions>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ProbeVolumesOptions>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
