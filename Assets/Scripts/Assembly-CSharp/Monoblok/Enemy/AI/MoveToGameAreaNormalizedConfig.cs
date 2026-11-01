using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Move To Game Area Normalized")]
	public class MoveToGameAreaNormalizedConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public GameArea GameArea { get; private set; }

		[field: SerializeField]
		[field: MinValue(0)]
		[field: MaxValue(1)]
		public Vector2 NormalizedPosition { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new MoveToGameAreaNormalized(brain, this);
		}
	}
}
