using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Spawn Enemies")]
	public class SpawnEnemiesStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public EnemySpawnerRuntimeValue SpawnerRuntimeValue { get; private set; }

		[field: SerializeField]
		public EnemyGroup EnemyGroup { get; private set; }

		[field: SerializeField]
		public float SpreadRadius { get; private set; }

		[field: SerializeField]
		public float Delay { get; private set; }

		[field: SerializeField]
		public Vector2Int MinMaxCount { get; private set; } = new Vector2Int(1, 3);


		public override EnemyState GetState(EnemyBrain brain)
		{
			return new SpawnEnemiesState(brain, this);
		}

		private void OnValidate()
		{
			MinMaxCount = new Vector2Int(Mathf.Max(1, MinMaxCount.x), Mathf.Max(MinMaxCount.x, MinMaxCount.y));
		}
	}
}
