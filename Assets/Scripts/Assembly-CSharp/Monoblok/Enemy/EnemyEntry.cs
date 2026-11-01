using System;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Enemy
{
	[Serializable]
	public class EnemyEntry
	{
		public EnemyConfig config;

		[Min(0f)]
		public float weight = 1f;

		[Expandable]
		public SpawnPatternConfig pattern;
	}
}
