using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(ChannelMixer))]
	[DisallowMultipleComponent]
	public class AnimatableChannelMixer : AnimatableVolumeComponentBase
	{
		public bool override_redOutRedIn;

		public float redOutRedIn;

		public bool override_redOutGreenIn;

		public float redOutGreenIn;

		public bool override_redOutBlueIn;

		public float redOutBlueIn;

		public bool override_greenOutRedIn;

		public float greenOutRedIn;

		public bool override_greenOutGreenIn;

		public float greenOutGreenIn;

		public bool override_greenOutBlueIn;

		public float greenOutBlueIn;

		public bool override_blueOutRedIn;

		public float blueOutRedIn;

		public bool override_blueOutGreenIn;

		public float blueOutGreenIn;

		public bool override_blueOutBlueIn;

		public float blueOutBlueIn;

		public override Type TargetType { get; } = typeof(ChannelMixer);


		private void WriteToVolumeComponent(ChannelMixer volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.redOutRedIn.overrideState = override_redOutRedIn;
				volumeComponent.redOutRedIn.value = redOutRedIn;
				volumeComponent.redOutGreenIn.overrideState = override_redOutGreenIn;
				volumeComponent.redOutGreenIn.value = redOutGreenIn;
				volumeComponent.redOutBlueIn.overrideState = override_redOutBlueIn;
				volumeComponent.redOutBlueIn.value = redOutBlueIn;
				volumeComponent.greenOutRedIn.overrideState = override_greenOutRedIn;
				volumeComponent.greenOutRedIn.value = greenOutRedIn;
				volumeComponent.greenOutGreenIn.overrideState = override_greenOutGreenIn;
				volumeComponent.greenOutGreenIn.value = greenOutGreenIn;
				volumeComponent.greenOutBlueIn.overrideState = override_greenOutBlueIn;
				volumeComponent.greenOutBlueIn.value = greenOutBlueIn;
				volumeComponent.blueOutRedIn.overrideState = override_blueOutRedIn;
				volumeComponent.blueOutRedIn.value = blueOutRedIn;
				volumeComponent.blueOutGreenIn.overrideState = override_blueOutGreenIn;
				volumeComponent.blueOutGreenIn.value = blueOutGreenIn;
				volumeComponent.blueOutBlueIn.overrideState = override_blueOutBlueIn;
				volumeComponent.blueOutBlueIn.value = blueOutBlueIn;
			}
		}

		private void ReadFromVolumeComponent(ChannelMixer volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_redOutRedIn = volumeComponent.redOutRedIn.overrideState;
				redOutRedIn = volumeComponent.redOutRedIn.value;
				override_redOutGreenIn = volumeComponent.redOutGreenIn.overrideState;
				redOutGreenIn = volumeComponent.redOutGreenIn.value;
				override_redOutBlueIn = volumeComponent.redOutBlueIn.overrideState;
				redOutBlueIn = volumeComponent.redOutBlueIn.value;
				override_greenOutRedIn = volumeComponent.greenOutRedIn.overrideState;
				greenOutRedIn = volumeComponent.greenOutRedIn.value;
				override_greenOutGreenIn = volumeComponent.greenOutGreenIn.overrideState;
				greenOutGreenIn = volumeComponent.greenOutGreenIn.value;
				override_greenOutBlueIn = volumeComponent.greenOutBlueIn.overrideState;
				greenOutBlueIn = volumeComponent.greenOutBlueIn.value;
				override_blueOutRedIn = volumeComponent.blueOutRedIn.overrideState;
				blueOutRedIn = volumeComponent.blueOutRedIn.value;
				override_blueOutGreenIn = volumeComponent.blueOutGreenIn.overrideState;
				blueOutGreenIn = volumeComponent.blueOutGreenIn.value;
				override_blueOutBlueIn = volumeComponent.blueOutBlueIn.overrideState;
				blueOutBlueIn = volumeComponent.blueOutBlueIn.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<ChannelMixer>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ChannelMixer>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ChannelMixer>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
