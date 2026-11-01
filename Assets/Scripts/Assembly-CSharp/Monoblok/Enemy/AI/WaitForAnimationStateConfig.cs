using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Wait for animation")]
	public class WaitForAnimationStateConfig : EnemyStateConfig
	{
		public override EnemyState GetState(EnemyBrain brain)
		{
			return new WaitForAnimationState(brain, this);
		}
	}
}
