using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class DieParticles : MonoBehaviour
	{
		[SerializeField]
		private Health health;

		[SerializeField]
		private ParticleSystem ps;

		private void OnEnable()
		{
			health.OnHealthZero += HandleHealthZero;
		}

		private void OnDisable()
		{
			health.OnHealthZero -= HandleHealthZero;
		}

		private void HandleHealthZero()
		{
			if (!(ps == null))
			{
				ps.Play();
			}
		}
	}
}
