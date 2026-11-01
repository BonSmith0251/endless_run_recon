using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Train
{
	public class AbilityInfoView : MonoBehaviour
	{
		[SerializeField]
		private Image icon;

		[SerializeField]
		private TMP_Text abilityNameText;

		[SerializeField]
		private TMP_Text abilityDescriptionText;

		public void Initialize(AbilityConfig config)
		{
			icon.sprite = config.Icon;
			abilityNameText.text = config.AbilityName;
			abilityDescriptionText.text = config.AbilityDescription;
		}
	}
}
