using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Scale")]
	public class ScaleStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public float TargetScale { get; private set; }

		[field: SerializeField]
		public float Duration { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new ScaleState(brain, this);
		}
	}
}
