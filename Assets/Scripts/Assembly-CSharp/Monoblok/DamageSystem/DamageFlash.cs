using PrimeTween;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class DamageFlash : MonoBehaviour
	{
		[SerializeField]
		private Damageable damageable;

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		[Min(0.01f)]
		private float flashTime = 1f;

		[Header("Health Hit")]
		[SerializeField]
		private Color healthColor = Color.white;

		[SerializeField]
		[Range(0f, 1f)]
		private float healthBlend = 1f;

		[Header("Shield Hit")]
		[SerializeField]
		private Color shieldColor = Color.cyan;

		[SerializeField]
		[Range(0f, 1f)]
		private float shieldBlend = 0.4f;

		private MaterialPropertyBlock mpb;

		private static readonly int ColorID = Shader.PropertyToID("_HitEffectColor");

		private static readonly int BlendID = Shader.PropertyToID("_HitEffectBlend");

		private Tween flashTween;

		private void OnValidate()
		{
			if (damageable == null)
			{
				damageable = GetComponentInChildren<Damageable>();
			}
		}

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		private void OnEnable()
		{
			damageable.OnHealthDamaged += OnHealthDamaged;
			damageable.OnShieldDamaged += OnShieldDamaged;
		}

		private void OnDisable()
		{
			damageable.OnHealthDamaged -= OnHealthDamaged;
			damageable.OnShieldDamaged -= OnShieldDamaged;
		}

		private void OnHealthDamaged(Damage dmg)
		{
			Flash(healthColor, healthBlend);
		}

		private void OnShieldDamaged(Damage dmg)
		{
			Flash(shieldColor, shieldBlend);
		}

		private void Flash(Color color, float blend)
		{
			flashTween.Complete();
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetColor(ColorID, color);
			mpb.SetFloat(BlendID, blend);
			spriteRenderer.SetPropertyBlock(mpb);
			flashTween = Tween.Delay(flashTime, delegate
			{
				if (!(spriteRenderer == null))
				{
					spriteRenderer.GetPropertyBlock(mpb);
					mpb.SetFloat(BlendID, 0f);
					spriteRenderer.SetPropertyBlock(mpb);
				}
			});
		}
	}
}
