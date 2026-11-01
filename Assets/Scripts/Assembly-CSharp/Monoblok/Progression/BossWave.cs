using Monoblok.Enemy;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Progression
{
	public class BossWave : Wave<BossWaveData>
	{
		private readonly EnemyConfig enemyConfig;

		private readonly SpawnPatternConfig pattern;

		private readonly EnemySpawner spawner;

		private readonly BossIntroduction bossIntroduction;

		private readonly BossCelebration bossCelebration;

		private Monoblok.Enemy.Enemy enemy;

		private Tween introDelayTween;

		private Tween activateDelayTween;

		private Sequence deathSequence;

		public BossWave(EnemyConfig enemyConfig, SpawnPatternConfig pattern, EnemySpawner spawner, BossIntroduction bossIntroduction, BossCelebration bossCelebration, BossWaveData data, int waveIndex)
			: base(waveIndex, data)
		{
			this.enemyConfig = enemyConfig;
			this.pattern = pattern;
			this.spawner = spawner;
			this.bossIntroduction = bossIntroduction;
			this.bossCelebration = bossCelebration;
		}

		protected override void OnStartWave()
		{
			introDelayTween = Tween.Delay(1f).OnComplete(this, delegate(BossWave t)
			{
				t.StartIntroduction();
			});
		}

		private void StartIntroduction()
		{
			bossIntroduction.OnComplete += OnIntroductionComplete;
			bossIntroduction.StartIntroduction(enemyConfig);
		}

		private void OnIntroductionComplete()
		{
			bossIntroduction.OnComplete -= OnIntroductionComplete;
			activateDelayTween = Tween.Delay(1f).OnComplete(this, delegate(BossWave t)
			{
				t.ActivateEnemy();
			});
		}

		private void ActivateEnemy()
		{
			enemy = spawner.SpawnSingle(enemyConfig, pattern);
			base.Data.SetEnemy(enemy);
			enemy.OnDied += HandleEnemyDied;
		}

		private void HandleEnemyDied(Monoblok.Enemy.Enemy enemy)
		{
			if (enemy != null)
			{
				enemy.OnDied -= HandleEnemyDied;
			}
			deathSequence = Sequence.Create().Chain(Tween.GlobalTimeScale(0.5f, 0.3f)).ChainDelay(2f)
				.Chain(Tween.GlobalTimeScale(1f, 0.3f))
				.ChainCallback(this, delegate(BossWave t)
				{
					t.bossCelebration.PlayAnimation();
				})
				.ChainDelay(5f)
				.OnComplete(this, delegate(BossWave t)
				{
					t.CompleteWave();
				});
		}

		protected override void OnUpdateWave(float deltaTime)
		{
		}

		protected override void OnWaveEnded()
		{
			introDelayTween.Stop();
			activateDelayTween.Stop();
			if (deathSequence.isAlive)
			{
				deathSequence.Stop();
				Time.timeScale = 1f;
			}
			bossIntroduction.OnComplete -= OnIntroductionComplete;
			if (enemy != null)
			{
				enemy.OnDied -= HandleEnemyDied;
			}
			bossIntroduction.Cancel();
			bossCelebration.Cancel();
		}
	}
}
