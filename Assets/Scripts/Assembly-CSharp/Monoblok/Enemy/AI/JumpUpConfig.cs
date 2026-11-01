using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Jump Up")]
	public class JumpUpConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public float Height { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new JumpUp(brain, this);
		}
	}
}
