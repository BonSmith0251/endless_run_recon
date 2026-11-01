using System;
using Monoblok.DamageSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class MeleeWeapon : MonoBehaviour
	{
		[SerializeField]
		private BoxCollider2D boxCollider;

		[SerializeField]
		private Rigidbody2D rb;

		private int layer;

		private float angle;

		public event Action<Health> OnHit;

		private void OnValidate()
		{
			if (boxCollider == null)
			{
				boxCollider = GetComponent<BoxCollider2D>();
			}
			if (rb == null)
			{
				rb = GetComponent<Rigidbody2D>();
			}
		}

		private void Awake()
		{
			boxCollider.isTrigger = true;
			rb.bodyType = RigidbodyType2D.Kinematic;
		}

		public void Initialize(int layer)
		{
			this.layer = layer;
		}

		public void Spin(float speed, float radius, float angleOffset)
		{
			angle += speed * Time.deltaTime;
			float x = Mathf.Cos(angle + angleOffset) * radius;
			float y = Mathf.Sin(angle + angleOffset) * radius;
			base.transform.localPosition = new Vector3(x, y, 0f);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == layer && collision.TryGetComponent<Health>(out var component))
			{
				this.OnHit?.Invoke(component);
			}
		}
	}
}
