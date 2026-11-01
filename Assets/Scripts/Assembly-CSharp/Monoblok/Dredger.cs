using Monoblok.DamageSystem;
using Monoblok.Isometric;
using Monoblok.SA;
using Monoblok.StatSystem;
using Monoblok.Train;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;
using UnityEngine.Animations;

namespace Monoblok
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Dredger : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private ParentConstraint parentConstraint;

		[SerializeField]
		private Monoblok.Train.Train train;

		[SerializeField]
		private TrainController trainController;

		[SerializeField]
		private BoolRuntimeValue dredgerEnabled;

		[SerializeField]
		private StatInitializer damage;

		[SerializeField]
		private StatManager statManager;

		[SerializeField]
		[Layer]
		private int layer;

		[SerializeField]
		private float velocityThreshold = 1f;

		[SerializeField]
		[ReadOnly]
		private Vector2 originalLocalPos;

		private bool velocitySufficient;

		private Tween timeScaleTween;

		private void OnValidate()
		{
			originalLocalPos = base.transform.localPosition;
		}

		private void OnEnable()
		{
			dredgerEnabled.OnValueChanged += HandleDredgerEnabledChanged;
			HandleDredgerEnabledChanged(dredgerEnabled.Value);
		}

		private void OnDisable()
		{
			dredgerEnabled.OnValueChanged -= HandleDredgerEnabledChanged;
		}

		private void Update()
		{
			if (CanDredge())
			{
				float num = Vector2.Dot((Vector2)trainController.Velocity, Iso.Right);
				velocitySufficient = num >= velocityThreshold;
				train.Locomotive.Damageable.SetDamageTypeMask(velocitySufficient ? (~Damage.DamageType.Contact) : ((Damage.DamageType)(-1)));
			}
		}

		public void SetParentConstraintSource(Transform source)
		{
			ResetConstraintAndLocalPosition();
			parentConstraint.AddSource(new ConstraintSource
			{
				sourceTransform = source,
				weight = 1f
			});
			parentConstraint.SetTranslationOffset(0, originalLocalPos);
			parentConstraint.constraintActive = true;
		}

		public void ResetConstraintAndLocalPosition()
		{
			for (int num = parentConstraint.sourceCount - 1; num >= 0; num--)
			{
				parentConstraint.RemoveSource(num);
			}
			parentConstraint.constraintActive = false;
			base.transform.localPosition = originalLocalPos;
			base.transform.localRotation = Quaternion.identity;
		}

		private bool CanDredge()
		{
			if (dredgerEnabled != null && !dredgerEnabled.Value)
			{
				return false;
			}
			return true;
		}

		private void HandleDredgerEnabledChanged(bool enabled)
		{
			if (enabled)
			{
				statManager.AcquireStat(damage);
			}
			else
			{
				statManager.ReleaseStat(damage.StatDefinition);
			}
			spriteRenderer.gameObject.SetActive(enabled);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (CanDredge() && velocitySufficient && collision.gameObject.layer == layer && collision.TryGetComponent<Health>(out var component))
			{
				component.Damage(new Damage((int)statManager.GetStatValue(damage.StatDefinition), 0, Damage.DamageType.Contact));
				timeScaleTween.Complete();
				Time.timeScale = 0.15f;
				timeScaleTween = Tween.Delay(0.07f, delegate
				{
					Time.timeScale = 1f;
				}, useUnscaledTime: true);
				CameraShaker.Shake(ShakePreset.Medium);
			}
		}
	}
}
