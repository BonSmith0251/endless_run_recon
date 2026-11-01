using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Chase Random Cart")]
	public class ChaseRandomCartStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public TrainRuntimeValue TrainRuntimeValue { get; private set; }

		[field: SerializeField]
		public float StoppingDistance { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new ChaseRandomCartState(brain, this);
		}
	}
}
