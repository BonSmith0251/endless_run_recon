using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(ColorCurves))]
	[DisallowMultipleComponent]
	public class AnimatableColorCurves : AnimatableVolumeComponentBase
	{
		public bool override_master;

		public bool override_red;

		public bool override_green;

		public bool override_blue;

		public bool override_hueVsHue;

		public bool override_hueVsSat;

		public bool override_satVsSat;

		public bool override_lumVsSat;

		public override Type TargetType { get; } = typeof(ColorCurves);


		private void WriteToVolumeComponent(ColorCurves volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent.master.overrideState = override_master;
				volumeComponent.red.overrideState = override_red;
				volumeComponent.green.overrideState = override_green;
				volumeComponent.blue.overrideState = override_blue;
				volumeComponent.hueVsHue.overrideState = override_hueVsHue;
				volumeComponent.hueVsSat.overrideState = override_hueVsSat;
				volumeComponent.satVsSat.overrideState = override_satVsSat;
				volumeComponent.lumVsSat.overrideState = override_lumVsSat;
			}
		}

		private void ReadFromVolumeComponent(ColorCurves volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override_master = volumeComponent.master.overrideState;
				override_red = volumeComponent.red.overrideState;
				override_green = volumeComponent.green.overrideState;
				override_blue = volumeComponent.blue.overrideState;
				override_hueVsHue = volumeComponent.hueVsHue.overrideState;
				override_hueVsSat = volumeComponent.hueVsSat.overrideState;
				override_satVsSat = volumeComponent.satVsSat.overrideState;
				override_lumVsSat = volumeComponent.lumVsSat.overrideState;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<ColorCurves>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ColorCurves>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<ColorCurves>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
