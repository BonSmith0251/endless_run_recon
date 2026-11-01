using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(PaniniProjection))]
	[DisallowMultipleComponent]
	public class AnimatablePaniniProjection : AnimatableVolumeComponentBase
	{
		public bool override_distance;

		public float distance;

		public bool override_cropToFit;

		public float cropToFit;

		public override Type TargetType { get; } = typeof(PaniniProjection);


		private void WriteToVolumeComponent(PaniniProjection volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.distance.overrideState = override_distance;
				volumeComponent.distance.value = distance;
				volumeComponent.cropToFit.overrideState = override_cropToFit;
				volumeComponent.cropToFit.value = cropToFit;
			}
		}

		private void ReadFromVolumeComponent(PaniniProjection volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_distance = volumeComponent.distance.overrideState;
				distance = volumeComponent.distance.value;
				override_cropToFit = volumeComponent.cropToFit.overrideState;
				cropToFit = volumeComponent.cropToFit.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<PaniniProjection>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<PaniniProjection>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<PaniniProjection>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
