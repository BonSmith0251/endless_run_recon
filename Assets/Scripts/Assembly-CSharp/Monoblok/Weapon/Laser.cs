using System;
using Monoblok.DamageSystem;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class Laser : MonoBehaviour
	{
		private int layer;

		public event Action<Laser> OnReadyToRelease;

		public event Action<IDamageable> OnHit;

		public void Initialize(Vector3 direction, int layer, float width, float range)
		{
			this.layer = layer;
			base.transform.localScale = new Vector3(0.2f, 0f, 1f);
			base.transform.up = direction;
			Sequence.Create().Chain(Tween.ScaleY(base.transform, range, 0.3f, Ease.InOutQuart)).Chain(Tween.ScaleX(base.transform, width, 0.1f))
				.ChainDelay(0.05f)
				.ChainCallback(this, delegate(Laser t)
				{
					t.FindTargets();
				})
				.Group(Tween.PunchScale(base.transform, new Vector3(10f, 0f, 0f), 0.15f))
				.Chain(Tween.ScaleX(base.transform, 0f, 0.1f))
				.OnComplete(this, delegate(Laser t)
				{
					t.OnReadyToRelease?.Invoke(this);
				});
		}

		private void FindTargets()
		{
			Collider2D[] array = Physics2D.OverlapBoxAll(base.transform.position + base.transform.up * (base.transform.localScale.y / 2f), base.transform.localScale, base.transform.eulerAngles.z, 1 << layer);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].TryGetComponent<IDamageable>(out var component))
				{
					this.OnHit?.Invoke(component);
				}
			}
		}
	}
}
