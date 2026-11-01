using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(ColorLookup))]
	[DisallowMultipleComponent]
	public class AnimatableColorLookup : AnimatableVolumeComponentBase
	{
		public bool override_texture;

		public Texture texture;

		public bool override_contribution;

		public float contribution;

		public override Type TargetType { get; } = typeof(ColorLookup);


		private void WriteToVolumeComponent(ColorLookup volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.texture.overrideState = override_texture;
				volumeComponent.texture.value = texture;
				volumeComponent.contribution.overrideState = override_contribution;
				volumeComponent.contribution.value = contribution;
			}
		}

		private void ReadFromVolumeComponent(ColorLookup volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_texture = volumeComponent.texture.overrideState;
				texture = volumeComponent.texture.value;
				override_contribution = volumeComponent.contribution.overrideState;
				contribution = volumeComponent.contribution.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<ColorLookup>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ColorLookup>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ColorLookup>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
