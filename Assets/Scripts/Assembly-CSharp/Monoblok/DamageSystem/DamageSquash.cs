using PrimeTween;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class DamageSquash : MonoBehaviour
	{
		[SerializeField]
		private Damageable damageable;

		[SerializeField]
		private ShakeSettings settings;

		private Tween punchTween;

		private void OnValidate()
		{
			if (damageable == null)
			{
				damageable = GetComponentInChildren<Damageable>();
			}
		}

		private void OnEnable()
		{
			damageable.OnHealthDamaged += OnDamaged;
		}

		private void OnDisable()
		{
			damageable.OnHealthDamaged -= OnDamaged;
		}

		private void OnDamaged(Damage damage)
		{
			punchTween.Complete();
			punchTween = Tween.PunchScale(base.transform, settings);
		}
	}
}
