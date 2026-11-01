using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Enemy
{
	public class EnemyDeathCounter : MonoBehaviour
	{
		[SerializeField]
		private EnemyGameEvent enemyDiedEvent;

		private int counter;

		public int Counter => counter;

		public void OnEnable()
		{
			enemyDiedEvent.OnTriggered += HandleEnemyDied;
		}

		private void OnDisable()
		{
			enemyDiedEvent.OnTriggered -= HandleEnemyDied;
		}

		private void HandleEnemyDied(Enemy enemy)
		{
			counter++;
		}

		public void ResetCounter()
		{
			counter = 0;
		}

		public int GetSaveData()
		{
			return counter;
		}

		public void LoadSaveData(int data)
		{
			counter = data;
		}
	}
}
