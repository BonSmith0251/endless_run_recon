using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(WhiteBalance))]
	[DisallowMultipleComponent]
	public class AnimatableWhiteBalance : AnimatableVolumeComponentBase
	{
		public bool override_temperature;

		public float temperature;

		public bool override_tint;

		public float tint;

		public override Type TargetType { get; } = typeof(WhiteBalance);


		private void WriteToVolumeComponent(WhiteBalance volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.temperature.overrideState = override_temperature;
				volumeComponent.temperature.value = temperature;
				volumeComponent.tint.overrideState = override_tint;
				volumeComponent.tint.value = tint;
			}
		}

		private void ReadFromVolumeComponent(WhiteBalance volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_temperature = volumeComponent.temperature.overrideState;
				temperature = volumeComponent.temperature.value;
				override_tint = volumeComponent.tint.overrideState;
				tint = volumeComponent.tint.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<WhiteBalance>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<WhiteBalance>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<WhiteBalance>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
