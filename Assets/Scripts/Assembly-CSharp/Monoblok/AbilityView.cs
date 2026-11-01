using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class AbilityView : MonoBehaviour
	{
		[SerializeField]
		private Image iconImage;

		[SerializeField]
		private Image cooldownOverlay;

		[SerializeField]
		private TMP_Text chargesText;

		[SerializeField]
		private Color normalColor = Color.white;

		[SerializeField]
		private Color cooldownColor = Color.grey;

		private Ability ability;

		private void OnDestroy()
		{
			if (ability != null)
			{
				ability.OnChargesChanged -= RefreshCharges;
				ability.OnCooldownChanged -= RefreshCooldownVisibility;
				ability = null;
			}
		}

		public void Initialize(Ability ability)
		{
			this.ability = ability;
			iconImage.sprite = ability.Icon;
			ability.OnChargesChanged += RefreshCharges;
			ability.OnCooldownChanged += RefreshCooldownVisibility;
			RefreshCharges();
			RefreshCooldownVisibility(ability.IsOnCooldown);
		}

		private void Update()
		{
			if (ability != null && ability.IsOnCooldown)
			{
				cooldownOverlay.fillAmount = Mathf.Clamp01(ability.CooldownProgress);
			}
		}

		private void RefreshCharges()
		{
			chargesText.SetText(ability.CurrentCharges.ToString());
		}

		private void RefreshCooldownVisibility(bool onCooldown)
		{
			iconImage.color = (onCooldown ? cooldownColor : normalColor);
			if (!onCooldown)
			{
				cooldownOverlay.fillAmount = 1f;
			}
			else
			{
				cooldownOverlay.fillAmount = Mathf.Clamp01(ability.CooldownProgress);
			}
		}
	}
}
