using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Move Along Edge")]
	public class MoveAlongEdgeConfig : EnemyStateConfig
	{
		public enum Edge
		{
			Top,
			Bottom,
			Left,
			Right
		}

		[field: SerializeField]
		public GameArea GameArea { get; private set; }

		[field: SerializeField]
		public Edge EdgeToMoveAlong { get; private set; }

		[field: SerializeField]
		[field: MinValue(0)]
		[field: MaxValue(1)]
		public int Direction { get; private set; }

		[field: SerializeField]
		[field: MinValue(0)]
		public float Padding { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new MoveAlongEdge(brain, this);
		}
	}
}
