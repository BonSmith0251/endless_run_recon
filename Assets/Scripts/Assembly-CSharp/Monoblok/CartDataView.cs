using Monoblok.StatSystem;
using Monoblok.Train;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Monoblok
{
	public class CartDataView : MonoBehaviour
	{
		[SerializeField]
		private Image cartImage;

		[SerializeField]
		private TMP_Text descriptionText;

		[SerializeField]
		private TMP_Text slotCounterText;

		[SerializeField]
		private TMP_Text tierText;

		[SerializeField]
		private TMP_Text moduleStatModifierText;

		[SerializeField]
		private LocalizedString localizedModuleStatModifierText;

		private CartData data;

		public void Initialize(CartData data)
		{
			if (this.data != null)
			{
				this.data.LocalizedDescription.StringChanged -= SetDescriptionText;
			}
			this.data = data;
			data.LocalizedDescription.StringChanged += SetDescriptionText;
			SetCartImage(data.CartSprite);
			SetDescriptionText(data.Description);
			SetSlotCount(data.SlotLocalPositions.Count);
			SetTierText(data.Tier.DisplayName);
			SetModuleStatModifierText(data.ModuleStatModifierFactory);
		}

		private void SetCartImage(Sprite sprite)
		{
			if (!(cartImage == null))
			{
				cartImage.sprite = sprite;
			}
		}

		private void SetSlotCount(int count)
		{
			if (!(slotCounterText == null))
			{
				slotCounterText.text = count.ToString();
			}
		}

		private void SetTierText(string text)
		{
			if (!(tierText == null))
			{
				tierText.text = text;
			}
		}

		private void SetDescriptionText(string text)
		{
			if (!(descriptionText == null))
			{
				descriptionText.text = text;
			}
		}

		private void SetModuleStatModifierText(StatModifierFactory factory)
		{
			if (!(moduleStatModifierText == null))
			{
				bool flag = factory.IsValid();
				moduleStatModifierText.gameObject.SetActive(flag);
				if (flag)
				{
					moduleStatModifierText.text = localizedModuleStatModifierText.GetLocalizedString(factory.ToString());
				}
			}
		}
	}
}
