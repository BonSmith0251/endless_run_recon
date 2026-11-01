using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Idle")]
	public class IdleStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		[field: Min(0f)]
		public float IdleTime { get; private set; } = 2f;


		public override EnemyState GetState(EnemyBrain brain)
		{
			return new IdleState(brain, this);
		}
	}
}
