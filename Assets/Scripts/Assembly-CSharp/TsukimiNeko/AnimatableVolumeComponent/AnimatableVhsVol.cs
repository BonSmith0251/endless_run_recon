using System;
using UnityEngine;
using UnityEngine.Rendering;
using VolFx;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[AnimatableOf(typeof(VhsVol))]
	[DisallowMultipleComponent]
	public class AnimatableVhsVol : AnimatableVolumeComponentBase
	{
		public bool override__weight;

		public float _weight;

		public bool override__tape;

		public float _tape;

		public bool override__shades;

		public float _shades;

		public bool override__rocking;

		public float _rocking;

		public bool override__squeeze;

		public float _squeeze;

		public bool override__density;

		public float _density;

		public bool override__intensity;

		public float _intensity;

		public bool override__scale;

		public float _scale;

		public bool override__flickering;

		public float _flickering;

		public bool override__lines;

		public bool _lines;

		public bool override__color;

		public Color _color;

		public bool override__bleed;

		public float _bleed;

		public bool override__sl_count;

		public float _sl_count;

		public bool override__sl_intensity;

		public float _sl_intensity;

		public bool override__sl_move;

		public float _sl_move;

		public bool override__sl_flicker;

		public float _sl_flicker;

		public bool override__flow;

		public float _flow;

		public bool override__pulsation;

		public float _pulsation;

		public override Type TargetType { get; } = typeof(VhsVol);


		private void WriteToVolumeComponent(VhsVol volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				volumeComponent.active = active;
				volumeComponent._weight.overrideState = override__weight;
				volumeComponent._weight.value = _weight;
				volumeComponent._tape.overrideState = override__tape;
				volumeComponent._tape.value = _tape;
				volumeComponent._shades.overrideState = override__shades;
				volumeComponent._shades.value = _shades;
				volumeComponent._rocking.overrideState = override__rocking;
				volumeComponent._rocking.value = _rocking;
				volumeComponent._squeeze.overrideState = override__squeeze;
				volumeComponent._squeeze.value = _squeeze;
				volumeComponent._density.overrideState = override__density;
				volumeComponent._density.value = _density;
				volumeComponent._intensity.overrideState = override__intensity;
				volumeComponent._intensity.value = _intensity;
				volumeComponent._scale.overrideState = override__scale;
				volumeComponent._scale.value = _scale;
				volumeComponent._flickering.overrideState = override__flickering;
				volumeComponent._flickering.value = _flickering;
				volumeComponent._lines.overrideState = override__lines;
				volumeComponent._lines.value = _lines;
				volumeComponent._color.overrideState = override__color;
				volumeComponent._color.value = _color;
				volumeComponent._bleed.overrideState = override__bleed;
				volumeComponent._bleed.value = _bleed;
				volumeComponent._sl_count.overrideState = override__sl_count;
				volumeComponent._sl_count.value = _sl_count;
				volumeComponent._sl_intensity.overrideState = override__sl_intensity;
				volumeComponent._sl_intensity.value = _sl_intensity;
				volumeComponent._sl_move.overrideState = override__sl_move;
				volumeComponent._sl_move.value = _sl_move;
				volumeComponent._sl_flicker.overrideState = override__sl_flicker;
				volumeComponent._sl_flicker.value = _sl_flicker;
				volumeComponent._flow.overrideState = override__flow;
				volumeComponent._flow.value = _flow;
				volumeComponent._pulsation.overrideState = override__pulsation;
				volumeComponent._pulsation.value = _pulsation;
			}
		}

		private void ReadFromVolumeComponent(VhsVol volumeComponent)
		{
			if ((bool)volumeComponent)
			{
				active = volumeComponent.active;
				override__weight = volumeComponent._weight.overrideState;
				_weight = volumeComponent._weight.value;
				override__tape = volumeComponent._tape.overrideState;
				_tape = volumeComponent._tape.value;
				override__shades = volumeComponent._shades.overrideState;
				_shades = volumeComponent._shades.value;
				override__rocking = volumeComponent._rocking.overrideState;
				_rocking = volumeComponent._rocking.value;
				override__squeeze = volumeComponent._squeeze.overrideState;
				_squeeze = volumeComponent._squeeze.value;
				override__density = volumeComponent._density.overrideState;
				_density = volumeComponent._density.value;
				override__intensity = volumeComponent._intensity.overrideState;
				_intensity = volumeComponent._intensity.value;
				override__scale = volumeComponent._scale.overrideState;
				_scale = volumeComponent._scale.value;
				override__flickering = volumeComponent._flickering.overrideState;
				_flickering = volumeComponent._flickering.value;
				override__lines = volumeComponent._lines.overrideState;
				_lines = volumeComponent._lines.value;
				override__color = volumeComponent._color.overrideState;
				_color = volumeComponent._color.value;
				override__bleed = volumeComponent._bleed.overrideState;
				_bleed = volumeComponent._bleed.value;
				override__sl_count = volumeComponent._sl_count.overrideState;
				_sl_count = volumeComponent._sl_count.value;
				override__sl_intensity = volumeComponent._sl_intensity.overrideState;
				_sl_intensity = volumeComponent._sl_intensity.value;
				override__sl_move = volumeComponent._sl_move.overrideState;
				_sl_move = volumeComponent._sl_move.value;
				override__sl_flicker = volumeComponent._sl_flicker.overrideState;
				_sl_flicker = volumeComponent._sl_flicker.value;
				override__flow = volumeComponent._flow.overrideState;
				_flow = volumeComponent._flow.value;
				override__pulsation = volumeComponent._pulsation.overrideState;
				_pulsation = volumeComponent._pulsation.value;
			}
		}

		private void Reset()
		{
			Volume component = GetComponent<Volume>();
			if ((bool)component && (bool)component.sharedProfile && component.sharedProfile.TryGet<VhsVol>(out var component2))
			{
				ReadFromVolumeComponent(component2);
			}
		}

		public override void WriteToVolumeComponent()
		{
			if (base.volumeHelper.TryGet<VhsVol>(out var volumeComponent))
			{
				WriteToVolumeComponent(volumeComponent);
			}
		}

		public override void ReadFromVolumeComponent()
		{
			if (base.volumeHelper.TryGet<VhsVol>(out var volumeComponent))
			{
				ReadFromVolumeComponent(volumeComponent);
			}
		}
	}
}
