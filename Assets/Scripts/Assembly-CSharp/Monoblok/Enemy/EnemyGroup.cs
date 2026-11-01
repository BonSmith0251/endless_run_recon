using System;

namespace Monoblok.Enemy
{
	[Serializable]
	public class EnemyGroup
	{
		public EnemyConfig[] enemyConfigs;

		public SpawnPatternConfig spawnPattern;
	}
}
