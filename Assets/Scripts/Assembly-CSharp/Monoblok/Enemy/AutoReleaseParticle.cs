using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Enemy
{
	public class AutoReleaseParticle : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem ps;

		private ObjectPool<AutoReleaseParticle> pool;

		public ParticleSystem PS => ps;

		public void Init(ObjectPool<AutoReleaseParticle> pool)
		{
			this.pool = pool;
			ParticleSystem.MainModule main = ps.main;
			main.stopAction = ParticleSystemStopAction.Callback;
		}

		private void OnParticleSystemStopped()
		{
			if (pool != null)
			{
				pool.Release(this);
			}
		}
	}
}
