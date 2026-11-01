using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Repeat")]
	public class RepeatStatesConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public EnemyStateConfig[] States { get; set; }

		[field: SerializeField]
		public RepeatStates.RepeatMode Mode { get; set; }

		[field: SerializeField]
		public int RepeatCount { get; set; } = -1;


		public override EnemyState GetState(EnemyBrain brain)
		{
			return new RepeatStates(brain, this);
		}
	}
}
