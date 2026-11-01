using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	public class RuntimePriceView : MonoBehaviour
	{
		[SerializeField]
		private Image currencyImage;

		[SerializeField]
		private TMP_Text priceText;

		private RuntimePrice price;

		public void Initialize(RuntimePrice price)
		{
			if (this.price != null)
			{
				this.price.Amount.OnValueChanged -= HandleValueChanged;
			}
			this.price = price;
			if (price != null)
			{
				if (price.Currency != null)
				{
					currencyImage.sprite = price.Currency.Sprite;
					priceText.color = price.Currency.TextColor;
				}
				HandleValueChanged(price.Amount.Value);
				price.Amount.OnValueChanged += HandleValueChanged;
			}
		}

		private void HandleValueChanged(int value)
		{
			priceText.text = value.ToString();
		}
	}
}
