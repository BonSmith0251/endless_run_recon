using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(LiftGammaGain))]
	[DisallowMultipleComponent]
	public class AnimatableLiftGammaGain : AnimatableVolumeComponentBase
	{
		public bool override_lift;

		public Vector4 lift;

		public bool override_gamma;

		public Vector4 gamma;

		public bool override_gain;

		public Vector4 gain;

		public override Type TargetType { get; } = typeof(LiftGammaGain);


		private void WriteToVolumeComponent(LiftGammaGain volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.lift.overrideState = override_lift;
				volumeComponent.lift.value = lift;
				volumeComponent.gamma.overrideState = override_gamma;
				volumeComponent.gamma.value = gamma;
				volumeComponent.gain.overrideState = override_gain;
				volumeComponent.gain.value = gain;
			}
		}

		private void ReadFromVolumeComponent(LiftGammaGain volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_lift = volumeComponent.lift.overrideState;
				lift = volumeComponent.lift.value;
				override_gamma = volumeComponent.gamma.overrideState;
				gamma = volumeComponent.gamma.value;
				override_gain = volumeComponent.gain.overrideState;
				gain = volumeComponent.gain.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<LiftGammaGain>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<LiftGammaGain>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<LiftGammaGain>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
