using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Charge At Train")]
	public class ChargeAtTrainStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public TrainRuntimeValue TrainRuntimeValue { get; private set; }

		[field: SerializeField]
		public ChargeSignal SignalPrefab { get; private set; }

		[field: SerializeField]
		public float WindupDuration { get; private set; }

		[field: SerializeField]
		public float OverShoot { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new ChargeAtTrainState(brain, this);
		}
	}
}
