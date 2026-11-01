using System;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class DamageJump : MonoBehaviour
	{
		[SerializeField]
		private Damageable damageable;

		[SerializeField]
		private Transform jumpCenter;

		[Header("Config")]
		[SerializeField]
		[Min(0f)]
		private float duration = 0.25f;

		[SerializeField]
		[Min(0f)]
		private float jumpHeight = 1f;

		[SerializeField]
		[MinMaxSlider(0f, 100f)]
		private Vector2 rotateRange = new Vector2(25f, 35f);

		private Tween tween;

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
			tween.Complete();
			float randomAngle = UnityEngine.Random.Range(rotateRange.x, rotateRange.y);
			Transform center = ((jumpCenter != null) ? jumpCenter : base.transform);
			float previousAngle = 0f;
			base.transform.localRotation = Quaternion.identity;
			tween = Tween.Custom(this, 0f, 1f, duration, delegate(DamageJump t, float v)
			{
				float num = Mathf.Sin(v * MathF.PI);
				t.transform.localPosition = new Vector3(0f, num * jumpHeight, 0f);
				float num2 = randomAngle * (1f - v);
				float angle = num2 - previousAngle;
				t.transform.RotateAround(center.position, Vector3.forward, angle);
				previousAngle = num2;
			});
		}
	}
}
