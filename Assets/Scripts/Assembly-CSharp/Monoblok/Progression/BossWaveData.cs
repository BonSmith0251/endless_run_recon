using System;
using Monoblok.Enemy;
using UnityEngine;

namespace Monoblok.Progression
{
	[CreateAssetMenu(menuName = "Game/Progression/Boss Wave Data")]
	public class BossWaveData : WaveData
	{
		private Monoblok.Enemy.Enemy enemy;

		public Monoblok.Enemy.Enemy Enemy => enemy;

		public float CurrentHealth
		{
			get
			{
				if (!(enemy != null))
				{
					return 0f;
				}
				return enemy.Health.CurrentHealth;
			}
		}

		public float NormalizedHealth
		{
			get
			{
				if (!(enemy != null))
				{
					return 0f;
				}
				return enemy.Health.NormalizedHealth;
			}
		}

		public int CurrentShield
		{
			get
			{
				if (!(enemy != null))
				{
					return 0;
				}
				return enemy.Health.CurrentShield;
			}
		}

		public bool HasShield
		{
			get
			{
				if (enemy != null)
				{
					return enemy.Health.HasShield;
				}
				return false;
			}
		}

		public float Shield
		{
			get
			{
				if (!(enemy != null))
				{
					return 0f;
				}
				return enemy.Health.CurrentShield;
			}
		}

		public event Action OnEnemySet;

		public void SetEnemy(Monoblok.Enemy.Enemy enemy)
		{
			this.enemy = enemy;
			this.OnEnemySet?.Invoke();
		}

		protected override void OnEnd()
		{
			enemy = null;
		}
	}
}
