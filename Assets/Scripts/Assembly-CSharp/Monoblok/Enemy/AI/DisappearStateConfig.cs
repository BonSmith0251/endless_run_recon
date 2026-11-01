using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Disappear")]
	public class DisappearStateConfig : EnemyStateConfig
	{
		public override EnemyState GetState(EnemyBrain brain)
		{
			return new DisappearState(brain, this);
		}
	}
}
