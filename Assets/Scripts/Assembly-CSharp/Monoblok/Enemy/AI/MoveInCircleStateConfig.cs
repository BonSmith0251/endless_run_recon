using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Move In Circle")]
	public class MoveInCircleStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public TrainRuntimeValue TrainRuntimeValue { get; private set; }

		[field: SerializeReference]
		[field: SubclassSelector]
		public IRadiusProvider RadiusProvider { get; private set; }

		[field: SerializeField]
		public float MinAngleDegrees { get; private set; } = 360f;


		[field: SerializeField]
		public float MaxAngleDegrees { get; private set; } = 360f;


		public override EnemyState GetState(EnemyBrain brain)
		{
			return new MoveInCircleState(brain, this);
		}
	}
}
