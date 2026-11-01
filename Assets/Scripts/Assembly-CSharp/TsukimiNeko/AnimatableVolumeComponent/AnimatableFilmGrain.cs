using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(FilmGrain))]
	[DisallowMultipleComponent]
	public class AnimatableFilmGrain : AnimatableVolumeComponentBase
	{
		public bool override_type;

		public FilmGrainLookup type;

		public bool override_intensity;

		public float intensity;

		public bool override_response;

		public float response;

		public bool override_texture;

		public Texture texture;

		public override Type TargetType { get; } = typeof(FilmGrain);


		private void WriteToVolumeComponent(FilmGrain volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.type.overrideState = override_type;
				volumeComponent.type.value = type;
				volumeComponent.intensity.overrideState = override_intensity;
				volumeComponent.intensity.value = intensity;
				volumeComponent.response.overrideState = override_response;
				volumeComponent.response.value = response;
				volumeComponent.texture.overrideState = override_texture;
				volumeComponent.texture.value = texture;
			}
		}

		private void ReadFromVolumeComponent(FilmGrain volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_type = volumeComponent.type.overrideState;
				type = volumeComponent.type.value;
				override_intensity = volumeComponent.intensity.overrideState;
				intensity = volumeComponent.intensity.value;
				override_response = volumeComponent.response.overrideState;
				response = volumeComponent.response.value;
				override_texture = volumeComponent.texture.overrideState;
				texture = volumeComponent.texture.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<FilmGrain>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<FilmGrain>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<FilmGrain>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
