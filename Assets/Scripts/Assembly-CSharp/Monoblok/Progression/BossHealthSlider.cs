using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Progression
{
	public class BossHealthSlider : MonoBehaviour
	{
		[SerializeField]
		private BossWaveData data;

		[SerializeField]
		private Transform container;

		[SerializeField]
		private Image healthFill;

		[SerializeField]
		private Image delayFill;

		[SerializeField]
		private float delayBeforeCatchUp = 0.4f;

		[SerializeField]
		private float catchUpDuration = 0.3f;

		[SerializeField]
		private Image shieldFill;

		private Tween delayTween;

		private float maxShield;

		private void Awake()
		{
			container.localScale = new Vector3(0f, 1f, 1f);
		}

		private void OnEnable()
		{
			data.OnEnemySet += Open;
			data.OnEnded += Close;
		}

		private void OnDisable()
		{
			data.OnEnemySet -= Open;
			data.OnEnded -= Close;
		}

		public void Open()
		{
			Tween.ScaleX(container, 1f, 0.3f, Ease.OutBack);
			data.Enemy.Health.OnHealthChanged += UpdateProgress;
			data.Enemy.Health.OnShieldChanged += UpdateShield;
			float normalizedHealth = data.NormalizedHealth;
			healthFill.fillAmount = normalizedHealth;
			if (delayFill != null)
			{
				delayFill.fillAmount = normalizedHealth;
			}
			if (shieldFill != null)
			{
				shieldFill.gameObject.SetActive(data.HasShield);
				shieldFill.fillAmount = data.Shield;
				maxShield = data.Shield;
			}
		}

		public void Close()
		{
			Tween.ScaleX(container, 0f, 0.3f, Ease.InBack);
			if (!(data.Enemy == null))
			{
				data.Enemy.Health.OnHealthChanged -= UpdateProgress;
				data.Enemy.Health.OnShieldChanged -= UpdateShield;
			}
		}

		public void UpdateProgress(int _)
		{
			if (healthFill == null)
			{
				return;
			}
			float normalizedHealth = data.NormalizedHealth;
			Tween.UIFillAmount(healthFill, normalizedHealth, 0.1f, Ease.Linear);
			if (!(delayFill == null))
			{
				delayTween.Stop();
				if (normalizedHealth < delayFill.fillAmount)
				{
					delayTween = Tween.UIFillAmount(delayFill, normalizedHealth, catchUpDuration, Ease.OutCubic, 1, CycleMode.Restart, delayBeforeCatchUp);
				}
				else
				{
					delayFill.fillAmount = normalizedHealth;
				}
			}
		}

		public void UpdateShield(int _)
		{
			if (!(shieldFill == null))
			{
				bool hasShield = data.HasShield;
				shieldFill.gameObject.SetActive(hasShield);
				if (hasShield)
				{
					Tween.UIFillAmount(shieldFill, data.Shield / maxShield, 0.15f, Ease.Linear);
				}
			}
		}
	}
}
