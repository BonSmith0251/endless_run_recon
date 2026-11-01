using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Enter Random")]
	public class EnterRandomStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public EnemyStateConfig[] Pool { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new EnterRandomState(brain, this);
		}
	}
}
