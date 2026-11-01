using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class DamageCameraShake : MonoBehaviour
	{
		[SerializeField]
		private Health health;

		private void OnEnable()
		{
			health.OnDamaged += OnDamaged;
		}

		private void OnDisable()
		{
			health.OnDamaged -= OnDamaged;
		}

		private void OnDamaged(Damage damage)
		{
			CameraShaker.Shake(ShakePreset.Heavy);
		}
	}
}
