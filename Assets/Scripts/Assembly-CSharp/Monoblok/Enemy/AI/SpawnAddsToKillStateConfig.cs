using System;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Spawn Adds to Kill")]
	public class SpawnAddsToKillStateConfig : EnemyStateConfig
	{
		[Serializable]
		public class AddGroup
		{
			public EnemyConfig config;

			public SpawnPatternConfig pattern;
		}

		[field: SerializeField]
		public AddGroup[] Groups { get; private set; }

		[field: SerializeField]
		public EnemySpawnerRuntimeValue SpawnerValue { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new SpawnAddsToKillState(brain, this);
		}
	}
}
