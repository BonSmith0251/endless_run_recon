using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	public class ShopActionTooltip : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private Image icon;

		[SerializeField]
		private TMP_Text titleText;

		[SerializeField]
		private TMP_Text descriptionText;

		private void Awake()
		{
			Hide();
		}

		public void Show(ShopActionMetaData data)
		{
			if (!(data == null))
			{
				SetIcon(data.Icon);
				SetTitle(data.ActionName);
				SetDescription(data.ActionDescription);
				base.gameObject.SetActive(value: true);
			}
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		private void SetIcon(Sprite sprite)
		{
			if (!(icon == null))
			{
				icon.sprite = sprite;
			}
		}

		private void SetTitle(string title)
		{
			if (!(titleText == null))
			{
				titleText.text = title;
			}
		}

		private void SetDescription(string description)
		{
			if (!(descriptionText == null))
			{
				descriptionText.text = description;
			}
		}
	}
}
