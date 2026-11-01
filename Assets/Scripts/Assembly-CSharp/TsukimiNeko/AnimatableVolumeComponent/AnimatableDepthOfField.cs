using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(DepthOfField))]
	[DisallowMultipleComponent]
	public class AnimatableDepthOfField : AnimatableVolumeComponentBase
	{
		public bool override_mode;

		public DepthOfFieldMode mode;

		public bool override_gaussianStart;

		public float gaussianStart;

		public bool override_gaussianEnd;

		public float gaussianEnd;

		public bool override_gaussianMaxRadius;

		public float gaussianMaxRadius;

		public bool override_highQualitySampling;

		public bool highQualitySampling;

		public bool override_focusDistance;

		public float focusDistance;

		public bool override_aperture;

		public float aperture;

		public bool override_focalLength;

		public float focalLength;

		public bool override_bladeCount;

		public int bladeCount;

		public bool override_bladeCurvature;

		public float bladeCurvature;

		public bool override_bladeRotation;

		public float bladeRotation;

		public override Type TargetType { get; } = typeof(DepthOfField);


		private void WriteToVolumeComponent(DepthOfField volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.mode.overrideState = override_mode;
				volumeComponent.mode.value = mode;
				volumeComponent.gaussianStart.overrideState = override_gaussianStart;
				volumeComponent.gaussianStart.value = gaussianStart;
				volumeComponent.gaussianEnd.overrideState = override_gaussianEnd;
				volumeComponent.gaussianEnd.value = gaussianEnd;
				volumeComponent.gaussianMaxRadius.overrideState = override_gaussianMaxRadius;
				volumeComponent.gaussianMaxRadius.value = gaussianMaxRadius;
				volumeComponent.highQualitySampling.overrideState = override_highQualitySampling;
				volumeComponent.highQualitySampling.value = highQualitySampling;
				volumeComponent.focusDistance.overrideState = override_focusDistance;
				volumeComponent.focusDistance.value = focusDistance;
				volumeComponent.aperture.overrideState = override_aperture;
				volumeComponent.aperture.value = aperture;
				volumeComponent.focalLength.overrideState = override_focalLength;
				volumeComponent.focalLength.value = focalLength;
				volumeComponent.bladeCount.overrideState = override_bladeCount;
				volumeComponent.bladeCount.value = bladeCount;
				volumeComponent.bladeCurvature.overrideState = override_bladeCurvature;
				volumeComponent.bladeCurvature.value = bladeCurvature;
				volumeComponent.bladeRotation.overrideState = override_bladeRotation;
				volumeComponent.bladeRotation.value = bladeRotation;
			}
		}

		private void ReadFromVolumeComponent(DepthOfField volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_mode = volumeComponent.mode.overrideState;
				mode = volumeComponent.mode.value;
				override_gaussianStart = volumeComponent.gaussianStart.overrideState;
				gaussianStart = volumeComponent.gaussianStart.value;
				override_gaussianEnd = volumeComponent.gaussianEnd.overrideState;
				gaussianEnd = volumeComponent.gaussianEnd.value;
				override_gaussianMaxRadius = volumeComponent.gaussianMaxRadius.overrideState;
				gaussianMaxRadius = volumeComponent.gaussianMaxRadius.value;
				override_highQualitySampling = volumeComponent.highQualitySampling.overrideState;
				highQualitySampling = volumeComponent.highQualitySampling.value;
				override_focusDistance = volumeComponent.focusDistance.overrideState;
				focusDistance = volumeComponent.focusDistance.value;
				override_aperture = volumeComponent.aperture.overrideState;
				aperture = volumeComponent.aperture.value;
				override_focalLength = volumeComponent.focalLength.overrideState;
				focalLength = volumeComponent.focalLength.value;
				override_bladeCount = volumeComponent.bladeCount.overrideState;
				bladeCount = volumeComponent.bladeCount.value;
				override_bladeCurvature = volumeComponent.bladeCurvature.overrideState;
				bladeCurvature = volumeComponent.bladeCurvature.value;
				override_bladeRotation = volumeComponent.bladeRotation.overrideState;
				bladeRotation = volumeComponent.bladeRotation.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<DepthOfField>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<DepthOfField>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<DepthOfField>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
