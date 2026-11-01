using System;
using Monoblok.DamageSystem;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Weapon
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Projectile : MonoBehaviour
	{
		[SerializeField]
		private TrailRenderer trailRenderer;

		private float travelled;

		private ProjectileData data;

		private int pierceCount;

		private bool isReleased;

		[SerializeField]
		[HideInInspector]
		private Vector2 initialScale;

		public Action<Projectile> OnReadyToRelease;

		public event Action<IDamageable> OnHit;

		private void OnValidate()
		{
			initialScale = base.transform.localScale;
			if (trailRenderer == null)
			{
				trailRenderer = GetComponentInChildren<TrailRenderer>();
			}
		}

		public void Initialize(ProjectileData data)
		{
			trailRenderer.widthMultiplier = 1f;
			base.transform.localScale = initialScale;
			this.data = data;
			base.transform.right = data.Direction;
			travelled = 0f;
			pierceCount = 0;
			isReleased = false;
		}

		private void Update()
		{
			float num = data.Speed * Time.deltaTime;
			base.transform.position += data.Direction * num;
			travelled += num;
			if (travelled > data.Range)
			{
				ReleaseWithFadeOut();
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!isReleased && pierceCount <= data.Piercing && collision.gameObject.layer == data.Layer && collision.TryGetComponent<IDamageable>(out var component) && component != null)
			{
				this.OnHit?.Invoke(component);
				pierceCount++;
				if (pierceCount > data.Piercing)
				{
					ReleaseImmediate();
				}
			}
		}

		private void ReleaseImmediate()
		{
			if (!isReleased)
			{
				isReleased = true;
				OnReadyToRelease?.Invoke(this);
			}
		}

		public void ForceRelease()
		{
			if (!isReleased)
			{
				isReleased = true;
				Tween.StopAll(base.transform);
				OnReadyToRelease?.Invoke(this);
			}
		}

		private void ReleaseWithFadeOut()
		{
			if (!isReleased)
			{
				isReleased = true;
				Tween.Custom(this, 1f, 0f, 0.1f, delegate(Projectile target, float v)
				{
					target.Scale(v);
				}).OnComplete(this, delegate(Projectile t)
				{
					t.OnReadyToRelease?.Invoke(this);
				});
			}
		}

		private void Scale(float scale)
		{
			base.transform.localScale = initialScale * scale;
			trailRenderer.widthMultiplier = scale;
		}
	}
}
