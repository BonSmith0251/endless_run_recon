using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Wander Random Position")]
	public class WanderRandomPositionStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		[field: Min(0f)]
		public float MinDistance { get; private set; } = 3f;


		[field: SerializeField]
		[field: Min(0f)]
		public float MaxDistance { get; private set; } = 8f;


		[field: SerializeField]
		[field: Min(0f)]
		public float WanderRadius { get; private set; } = 2f;


		[field: SerializeField]
		[field: Min(0f)]
		public float ArrivalDistance { get; private set; } = 0.2f;


		public override EnemyState GetState(EnemyBrain brain)
		{
			return new WanderRandomPositionState(brain, this);
		}

		private void OnValidate()
		{
			if (MaxDistance < MinDistance)
			{
				MaxDistance = MinDistance;
			}
		}
	}
}
