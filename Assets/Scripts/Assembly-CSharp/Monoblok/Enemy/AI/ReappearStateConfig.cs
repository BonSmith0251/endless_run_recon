using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Reappear")]
	public class ReappearStateConfig : EnemyStateConfig
	{
		[field: SerializeReference]
		[field: SubclassSelector]
		public ISpawnAnchor Anchor { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new ReappearState(brain, this);
		}
	}
}
