using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Charge")]
	public class ChargeStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public Vector2 Direction { get; private set; } = Vector2.right;


		[field: SerializeField]
		[field: Min(0f)]
		public float Padding { get; private set; } = 2f;


		public override EnemyState GetState(EnemyBrain brain)
		{
			return new ChargeState(brain, this);
		}
	}
}
