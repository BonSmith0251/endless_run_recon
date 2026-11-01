using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VolFx;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	public static class AnimatableVolumeComponentMapping
	{
		public static readonly Dictionary<Type, Type> Map = new Dictionary<Type, Type>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitializeMap()
		{
			Map[typeof(ProbeVolumesOptions)] = typeof(AnimatableProbeVolumesOptions);
			Map[typeof(Bloom)] = typeof(AnimatableBloom);
			Map[typeof(ChannelMixer)] = typeof(AnimatableChannelMixer);
			Map[typeof(ChromaticAberration)] = typeof(AnimatableChromaticAberration);
			Map[typeof(ColorAdjustments)] = typeof(AnimatableColorAdjustments);
			Map[typeof(ColorCurves)] = typeof(AnimatableColorCurves);
			Map[typeof(ColorLookup)] = typeof(AnimatableColorLookup);
			Map[typeof(DepthOfField)] = typeof(AnimatableDepthOfField);
			Map[typeof(FilmGrain)] = typeof(AnimatableFilmGrain);
			Map[typeof(LensDistortion)] = typeof(AnimatableLensDistortion);
			Map[typeof(LiftGammaGain)] = typeof(AnimatableLiftGammaGain);
			Map[typeof(MotionBlur)] = typeof(AnimatableMotionBlur);
			Map[typeof(PaniniProjection)] = typeof(AnimatablePaniniProjection);
			Map[typeof(ScreenSpaceLensFlare)] = typeof(AnimatableScreenSpaceLensFlare);
			Map[typeof(ShadowsMidtonesHighlights)] = typeof(AnimatableShadowsMidtonesHighlights);
			Map[typeof(SplitToning)] = typeof(AnimatableSplitToning);
			Map[typeof(Tonemapping)] = typeof(AnimatableTonemapping);
			Map[typeof(Vignette)] = typeof(AnimatableVignette);
			Map[typeof(WhiteBalance)] = typeof(AnimatableWhiteBalance);
			Map[typeof(VhsVol)] = typeof(AnimatableVhsVol);
		}
	}
}
