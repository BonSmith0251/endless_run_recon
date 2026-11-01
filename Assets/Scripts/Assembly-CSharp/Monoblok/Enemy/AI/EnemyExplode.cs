using Monoblok.DamageSystem;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class EnemyExplode : MonoBehaviour
	{
		[SerializeField]
		private EnemyBrain brain;

		[SerializeField]
		private ParticleSystem explosionParticles;

		[SerializeField]
		private AnimationClip explosionAnimation;

		private Damage damage;

		private int layer;

		private float radius;

		public void Explode(Damage damage, int layer, float radius)
		{
			Debug.Log("Explode");
			this.damage = damage;
			this.layer = layer;
			this.radius = radius;
			brain.Animancer.Play(explosionAnimation).OwnedEvents.EndEvent.callback = OnExplosionAnimationComplete;
		}

		private void OnExplosionAnimationComplete()
		{
			if (Physics2D.OverlapCircle(base.transform.position, radius, layer).TryGetComponent<Damageable>(out var component))
			{
				component.Damage(damage);
			}
			explosionParticles.Play();
			brain.Enemy.ReleaseToPool();
		}
	}
}
