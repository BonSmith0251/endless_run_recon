using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Disappear For Duration")]
	public class DisappearForDurationStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public float Duration { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new DisappearForDuration(brain, this);
		}
	}
}
