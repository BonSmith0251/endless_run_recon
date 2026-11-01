using Monoblok.Enemy.AI;
using Monoblok.StatSystem;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Enemy
{
	[CreateAssetMenu(menuName = "Game/Enemy/Enemy Config")]
	public class EnemyConfig : ScriptableObject
	{
		[SerializeField]
		private string enemyName;

		[SerializeField]
		private string shortDescription;

		[SerializeField]
		private Enemy prefab;

		[SerializeField]
		[Expandable]
		[AllowNesting]
		private EnemyBehaviorConfig behaviorConfig;

		[SerializeField]
		private StatInitializer[] stats;

		[SerializeField]
		private int healthConst;

		[SerializeField]
		private int shieldConst;

		[SerializeField]
		private int damageConst;

		[SerializeField]
		private int coinConst;

		[SerializeField]
		private int xpConst;

		[SerializeField]
		[Range(0f, 1f)]
		private float metaCoinChance;

		[SerializeField]
		private int metaCoinAmount;

		public string EnemyName => enemyName;

		public string ShortDescription => shortDescription;

		public Enemy Prefab => prefab;

		public EnemyBehaviorConfig BehaviorConfig => behaviorConfig;

		public StatInitializer[] Stats => stats;

		public int Health => healthConst;

		public int Shield => shieldConst;

		public int Damage => damageConst;

		public int Coins => coinConst;

		public int XP => xpConst;

		public float MetaCoinChance => metaCoinChance;

		public int MetaCoinAmount => metaCoinAmount;
	}
}
