using System;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[Serializable]
	public class EnemyPhase
	{
		[field: SerializeField]
		public EnemyStateConfig[] States { get; private set; }
	}
}
