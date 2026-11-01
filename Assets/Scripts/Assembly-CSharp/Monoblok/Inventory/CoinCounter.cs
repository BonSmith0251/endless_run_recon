using Monoblok.Enemy;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Inventory
{
	public class CoinCounter : MonoBehaviour
	{
		[SerializeField]
		private CurrencyDefinition coinDefinition;

		[SerializeField]
		private EnemyGameEvent enemyDiedEvent;

		private int totalCoinsCollected;

		public int TotalCoinsCollected => totalCoinsCollected;

		private void OnEnable()
		{
			enemyDiedEvent.OnTriggered += OnEnemyDied;
		}

		private void OnDisable()
		{
			enemyDiedEvent.OnTriggered -= OnEnemyDied;
		}

		public void ResetTotalCoins()
		{
			totalCoinsCollected = 0;
		}

		private void OnEnemyDied(Monoblok.Enemy.Enemy enemy)
		{
			totalCoinsCollected += enemy.Coin;
		}

		public int GetSaveData()
		{
			return totalCoinsCollected;
		}

		public void LoadSaveData(int savedCoins)
		{
			totalCoinsCollected = savedCoins;
		}
	}
}
