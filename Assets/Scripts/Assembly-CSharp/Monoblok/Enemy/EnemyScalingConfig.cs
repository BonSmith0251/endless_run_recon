using UnityEngine;

namespace Monoblok.Enemy
{
	[CreateAssetMenu(menuName = "Game/Enemy/Enemy Scaling Config")]
	public class EnemyScalingConfig : ScriptableObject
	{
		[SerializeField]
		private StatScaling health = new StatScaling();

		[SerializeField]
		private StatScaling shield = new StatScaling();

		[SerializeField]
		private StatScaling damage = new StatScaling();

		[SerializeField]
		private StatScaling coin = new StatScaling();

		[SerializeField]
		private StatScaling xp = new StatScaling();

		public int GetHealth(EnemyConfig config, int loopCount)
		{
			return Mathf.RoundToInt(health.Apply(config.Health, loopCount));
		}

		public int GetShield(EnemyConfig config, int loopCount)
		{
			return Mathf.RoundToInt(shield.Apply(config.Shield, loopCount));
		}

		public int GetDamage(EnemyConfig config, int loopCount)
		{
			return Mathf.RoundToInt(damage.Apply(config.Damage, loopCount));
		}

		public int GetCoins(EnemyConfig config, int loopCount)
		{
			return Mathf.RoundToInt(coin.Apply(config.Coins, loopCount));
		}

		public int GetXP(EnemyConfig config, int loopCount)
		{
			return Mathf.RoundToInt(xp.Apply(config.XP, loopCount));
		}
	}
}
