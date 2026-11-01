using Monoblok.DamageSystem;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;

namespace Monoblok
{
	[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
	public class ScreenWiper : MonoBehaviour
	{
		[SerializeField]
		[Layer]
		private int layer;

		[SerializeField]
		private BoxCollider2D boxCollider;

		[SerializeField]
		private float wipeDuration = 1f;

		private void OnValidate()
		{
			if (!(boxCollider != null))
			{
				boxCollider = GetComponent<BoxCollider2D>();
			}
		}

		public void WipeScreen()
		{
			Camera main = Camera.main;
			float y = main.orthographicSize * 2f;
			boxCollider.size = new Vector2(boxCollider.size.x, y);
			float num = boxCollider.size.x / 2f;
			float num2 = main.transform.position.x - main.aspect * main.orthographicSize;
			base.transform.position = new Vector3(num2 + num, main.transform.position.y, 0f);
			float endValue = main.transform.position.x + main.aspect * main.orthographicSize - num;
			Sequence.Create().Chain(Tween.PositionX(base.transform, endValue, wipeDuration)).Chain(Tween.Scale(base.transform, 0f, 0.15f))
				.OnComplete(delegate
				{
					Object.Destroy(base.gameObject);
				});
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == layer)
			{
				Health component = collision.GetComponent<Health>();
				if (component != null)
				{
					component.Damage(new Damage(component.MaxHealth, 0, Damage.DamageType.Projectile));
				}
			}
		}
	}
}
