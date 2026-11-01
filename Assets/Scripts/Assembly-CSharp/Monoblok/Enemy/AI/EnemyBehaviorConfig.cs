using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/Behavior Config")]
	public class EnemyBehaviorConfig : ScriptableObject
	{
		[field: SerializeField]
		public EnemyPhase[] Phases { get; private set; }
	}
}
