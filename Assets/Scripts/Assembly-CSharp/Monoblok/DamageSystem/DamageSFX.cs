using FMODUnity;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class DamageSFX : MonoBehaviour
	{
		[SerializeField]
		private Damageable damageable;

		[SerializeField]
		private EventReference eventRef;

		private void OnEnable()
		{
			damageable.OnDamaged += OnDamaged;
		}

		private void OnDisable()
		{
			damageable.OnDamaged -= OnDamaged;
		}

		private void OnDamaged(Damage damage)
		{
			RuntimeManager.PlayOneShot(eventRef);
		}
	}
}
