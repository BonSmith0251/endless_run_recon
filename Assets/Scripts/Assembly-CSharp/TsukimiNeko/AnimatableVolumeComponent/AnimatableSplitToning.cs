using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(SplitToning))]
	[DisallowMultipleComponent]
	public class AnimatableSplitToning : AnimatableVolumeComponentBase
	{
		public bool override_shadows;

		public Color shadows;

		public bool override_highlights;

		public Color highlights;

		public bool override_balance;

		public float balance;

		public override Type TargetType { get; } = typeof(SplitToning);


		private void WriteToVolumeComponent(SplitToning volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.shadows.overrideState = override_shadows;
				volumeComponent.shadows.value = shadows;
				volumeComponent.highlights.overrideState = override_highlights;
				volumeComponent.highlights.value = highlights;
				volumeComponent.balance.overrideState = override_balance;
				volumeComponent.balance.value = balance;
			}
		}

		private void ReadFromVolumeComponent(SplitToning volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_shadows = volumeComponent.shadows.overrideState;
				shadows = volumeComponent.shadows.value;
				override_highlights = volumeComponent.highlights.overrideState;
				highlights = volumeComponent.highlights.value;
				override_balance = volumeComponent.balance.overrideState;
				balance = volumeComponent.balance.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<SplitToning>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<SplitToning>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<SplitToning>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
