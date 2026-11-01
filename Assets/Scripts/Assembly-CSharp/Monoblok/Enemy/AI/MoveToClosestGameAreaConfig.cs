using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Move To Closest GameArea")]
	public class MoveToClosestGameAreaConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public GameArea[] GameAreas { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new MoveToClosestGameArea(brain, this);
		}
	}
}
