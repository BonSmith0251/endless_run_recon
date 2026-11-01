using System;
using Reflex.Attributes;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class DamageNumberProvider : MonoBehaviour
	{
		[SerializeField]
		private Health health;

		[SerializeField]
		private bool provideDamageNumber = true;

		[SerializeField]
		private bool provideDodge = true;

		[Inject]
		private readonly DamageNumberRenderer numberRenderer;

		public event Action<Damage, Transform> OnDamaged;

		public event Action<Damage, Transform> OnShieldHit;

		public event Action<Damage, Transform> OnDodged;

		private void OnEnable()
		{
			health.OnDamageApplied += HandleDamage;
			health.OnDodged += HandleDodge;
			numberRenderer.Register(this);
		}

		private void OnDisable()
		{
			health.OnDamageApplied -= HandleDamage;
			health.OnDodged -= HandleDodge;
			numberRenderer.UnRegister(this);
		}

		private void HandleDamage(Damage damage, bool hitShield)
		{
			if (provideDamageNumber)
			{
				if (hitShield)
				{
					this.OnShieldHit?.Invoke(damage, base.transform);
				}
				else
				{
					this.OnDamaged?.Invoke(damage, base.transform);
				}
			}
		}

		private void HandleDodge(Damage damage)
		{
			if (provideDodge)
			{
				this.OnDodged?.Invoke(damage, base.transform);
			}
		}
	}
}
