using Monoblok.SA;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Enemy
{
	public class EnemyDieParticleSpawner : MonoBehaviour
	{
		[SerializeField]
		private EnemyGameEvent enemyDiedEvent;

		[SerializeField]
		private AutoReleaseParticle psPrefab;

		private ObjectPool<AutoReleaseParticle> pool;

		private void Awake()
		{
			pool = new ObjectPool<AutoReleaseParticle>(delegate
			{
				AutoReleaseParticle autoReleaseParticle = Object.Instantiate(psPrefab);
				autoReleaseParticle.Init(pool);
				return autoReleaseParticle;
			}, delegate(AutoReleaseParticle ps)
			{
				ps.gameObject.SetActive(value: true);
			}, delegate(AutoReleaseParticle ps)
			{
				ps.gameObject.SetActive(value: false);
			}, delegate(AutoReleaseParticle ps)
			{
				if (ps != null) Object.Destroy(ps.gameObject);
			});
		}

		private void OnEnable()
		{
			enemyDiedEvent.OnTriggered += OnEnemyDied;
		}

		private void OnDisable()
		{
			enemyDiedEvent.OnTriggered -= OnEnemyDied;
		}

		private void OnEnemyDied(Enemy enemy)
		{
			AutoReleaseParticle autoReleaseParticle = pool.Get();
			autoReleaseParticle.transform.position = enemy.transform.position;
			autoReleaseParticle.PS.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			autoReleaseParticle.PS.Play();
		}
	}
}
