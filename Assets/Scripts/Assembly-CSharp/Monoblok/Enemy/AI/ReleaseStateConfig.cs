using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Release")]
	public class ReleaseStateConfig : EnemyStateConfig
	{
		public override EnemyState GetState(EnemyBrain brain)
		{
			return new ReleaseState(brain, this);
		}
	}
}
