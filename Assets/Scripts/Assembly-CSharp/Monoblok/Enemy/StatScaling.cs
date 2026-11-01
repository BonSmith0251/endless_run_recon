using System;
using Monoblok.Progression;
using UnityEngine;

namespace Monoblok.Enemy
{
	[Serializable]
	public class StatScaling
	{
		public ScalingMode mode = ScalingMode.Multiplicative;

		[Tooltip("Multiplicative: baseValue = 1 means no change at loop 0, waveMultiplier is the growth rate per loop (e.g. 0.15 = +15%/loop.\nAdditive: baseValue/waveMultiplier are flat amounts added per loop.")]
		public IncrementalFloat value = new IncrementalFloat
		{
			baseValue = 1f
		};

		public float Apply(float baseStat, int loopCount)
		{
			float num = value.Get(loopCount);
			if (mode != ScalingMode.Multiplicative)
			{
				return baseStat + num;
			}
			return baseStat * num;
		}
	}
}
