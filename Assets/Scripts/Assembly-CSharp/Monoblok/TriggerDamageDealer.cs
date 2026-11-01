using System.Collections.Generic;
using Monoblok.DamageSystem;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class TriggerDamageDealer : MonoBehaviour
	{
		[SerializeField]
		[Layer]
		private int layer;

		[SerializeField]
		private int damage;

		[SerializeField]
		private float interval;

		private readonly List<IDamageable> targets = new List<IDamageable>();

		private float t;

		[ShowNativeProperty]
		private float T => t;

		public void Initialize(int damage)
		{
			this.damage = damage;
			t = interval;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == layer && collision.TryGetComponent<IDamageable>(out var component))
			{
				component.Damage(new Damage(damage, 0, Damage.DamageType.Contact));
				targets.Add(component);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.TryGetComponent<IDamageable>(out var component))
			{
				targets.Remove(component);
			}
		}

		private void Update()
		{
			if (targets.Count == 0)
			{
				return;
			}
			t += Time.deltaTime;
			if (t < interval)
			{
				return;
			}
			foreach (IDamageable target in targets)
			{
				target.Damage(new Damage(damage, 0, Damage.DamageType.Contact));
			}
			t = 0f;
		}
	}
}
