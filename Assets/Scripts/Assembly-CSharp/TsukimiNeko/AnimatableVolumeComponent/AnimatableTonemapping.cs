using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(Tonemapping))]
	[DisallowMultipleComponent]
	public class AnimatableTonemapping : AnimatableVolumeComponentBase
	{
		public bool override_mode;

		public TonemappingMode mode;

		public bool override_neutralHDRRangeReductionMode;

		public NeutralRangeReductionMode neutralHDRRangeReductionMode;

		public bool override_acesPreset;

		public HDRACESPreset acesPreset;

		public bool override_hueShiftAmount;

		public float hueShiftAmount;

		public bool override_detectPaperWhite;

		public bool detectPaperWhite;

		public bool override_paperWhite;

		public float paperWhite;

		public bool override_detectBrightnessLimits;

		public bool detectBrightnessLimits;

		public bool override_minNits;

		public float minNits;

		public bool override_maxNits;

		public float maxNits;

		public override Type TargetType { get; } = typeof(Tonemapping);


		private void WriteToVolumeComponent(Tonemapping volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.mode.overrideState = override_mode;
				volumeComponent.mode.value = mode;
				volumeComponent.neutralHDRRangeReductionMode.overrideState = override_neutralHDRRangeReductionMode;
				volumeComponent.neutralHDRRangeReductionMode.value = neutralHDRRangeReductionMode;
				volumeComponent.acesPreset.overrideState = override_acesPreset;
				volumeComponent.acesPreset.value = acesPreset;
				volumeComponent.hueShiftAmount.overrideState = override_hueShiftAmount;
				volumeComponent.hueShiftAmount.value = hueShiftAmount;
				volumeComponent.detectPaperWhite.overrideState = override_detectPaperWhite;
				volumeComponent.detectPaperWhite.value = detectPaperWhite;
				volumeComponent.paperWhite.overrideState = override_paperWhite;
				volumeComponent.paperWhite.value = paperWhite;
				volumeComponent.detectBrightnessLimits.overrideState = override_detectBrightnessLimits;
				volumeComponent.detectBrightnessLimits.value = detectBrightnessLimits;
				volumeComponent.minNits.overrideState = override_minNits;
				volumeComponent.minNits.value = minNits;
				volumeComponent.maxNits.overrideState = override_maxNits;
				volumeComponent.maxNits.value = maxNits;
			}
		}

		private void ReadFromVolumeComponent(Tonemapping volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_mode = volumeComponent.mode.overrideState;
				mode = volumeComponent.mode.value;
				override_neutralHDRRangeReductionMode = volumeComponent.neutralHDRRangeReductionMode.overrideState;
				neutralHDRRangeReductionMode = volumeComponent.neutralHDRRangeReductionMode.value;
				override_acesPreset = volumeComponent.acesPreset.overrideState;
				acesPreset = volumeComponent.acesPreset.value;
				override_hueShiftAmount = volumeComponent.hueShiftAmount.overrideState;
				hueShiftAmount = volumeComponent.hueShiftAmount.value;
				override_detectPaperWhite = volumeComponent.detectPaperWhite.overrideState;
				detectPaperWhite = volumeComponent.detectPaperWhite.value;
				override_paperWhite = volumeComponent.paperWhite.overrideState;
				paperWhite = volumeComponent.paperWhite.value;
				override_detectBrightnessLimits = volumeComponent.detectBrightnessLimits.overrideState;
				detectBrightnessLimits = volumeComponent.detectBrightnessLimits.value;
				override_minNits = volumeComponent.minNits.overrideState;
				minNits = volumeComponent.minNits.value;
				override_maxNits = volumeComponent.maxNits.overrideState;
				maxNits = volumeComponent.maxNits.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<Tonemapping>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<Tonemapping>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<Tonemapping>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
