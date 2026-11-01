using System;
using System.Collections.Generic;
using NaughtyAttributes;
using PrimeTween;
using UltEvents;
using UnityEngine;

namespace Monoblok
{
	[RequireComponent(typeof(CircleCollider2D))]
	public abstract class RadialKnockback : MonoBehaviour
	{
		[SerializeField]
		private CircleCollider2D circleCollider;

		[SerializeField]
		[Layer]
		private int layer;

		[SerializeField]
		private float duration = 2f;

		[SerializeField]
		private float knockbackDistance = 5f;

		[SerializeField]
		private UltEvent onKnockback;

		private HashSet<Collider2D> targets = new HashSet<Collider2D>();

		public event Action<Collider2D> OnTargetHit;

		private void OnValidate()
		{
			if (!(circleCollider != null))
			{
				circleCollider = GetComponent<CircleCollider2D>();
			}
		}

		private void Awake()
		{
			circleCollider.radius = 0f;
			circleCollider.enabled = false;
		}

		public void TriggerKnockback()
		{
			targets.Clear();
			circleCollider.radius = 0f;
			circleCollider.enabled = true;
			float radius = GetRadius();
			Tween.Custom(this, 0f, radius, duration, delegate(RadialKnockback t, float v)
			{
				t.circleCollider.radius = v;
			}, Ease.Linear).OnComplete(this, delegate(RadialKnockback t)
			{
				t.circleCollider.enabled = false;
				t.circleCollider.radius = 0f;
			});
			onKnockback?.Invoke();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == layer && collision.TryGetComponent<Knockback>(out var component) && targets.Add(collision))
			{
				Vector3 normalized = (collision.transform.position - base.transform.position).normalized;
				component.ApplyKnockback(normalized, knockbackDistance);
				this.OnTargetHit?.Invoke(collision);
			}
		}

		protected abstract float GetRadius();
	}
}
