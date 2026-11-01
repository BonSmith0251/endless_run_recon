using System;
using UnityEngine;

namespace Monoblok.Enemy
{
	[Serializable]
	public class SpawnScheduleEntry
	{
		public SpawnGroup group;

		[Tooltip("X axis = wave count, Y axis = selection weight. Values below 0 are treated as zero (never spawns).")]
		public AnimationCurve weightCurve;
	}
}
