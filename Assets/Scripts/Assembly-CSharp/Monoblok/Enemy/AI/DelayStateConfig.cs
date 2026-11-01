using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Delay")]
	public class DelayStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public float Duration { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new DelayState(brain, this);
		}
	}
}
