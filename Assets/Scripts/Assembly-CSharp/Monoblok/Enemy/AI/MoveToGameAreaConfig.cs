using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Move To GameArea")]
	public class MoveToGameAreaConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public GameArea GameArea { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new MoveToGameArea(brain, this);
		}
	}
}
