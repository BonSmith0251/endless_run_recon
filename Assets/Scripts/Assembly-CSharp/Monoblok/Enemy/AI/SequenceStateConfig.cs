using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Sequence")]
	public class SequenceStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public EnemyStateConfig[] Sequence { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new SequenceState(brain, this);
		}
	}
}
