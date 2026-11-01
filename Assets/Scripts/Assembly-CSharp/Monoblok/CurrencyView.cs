using Monoblok.Inventory;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class CurrencyView : MonoBehaviour
	{
		[SerializeField]
		private PlayerWallet inventory;

		[SerializeField]
		private CurrencyDefinition currencyDefinition;

		[Space]
		[Header("View")]
		[SerializeField]
		private TMP_Text countText;

		[SerializeField]
		private Image icon;

		[Space]
		[Header("Events")]
		[SerializeField]
		private UltEvent onCountTextUpdated;

		private void Awake()
		{
			if (!(icon == null))
			{
				icon.sprite = currencyDefinition.Sprite;
			}
		}

		private void OnEnable()
		{
			inventory.Subscribe(currencyDefinition, HandleCountChanged);
			HandleCountChanged(inventory.GetCount(currencyDefinition));
		}

		private void OnDisable()
		{
			inventory.Unsubscribe(currencyDefinition, HandleCountChanged);
		}

		private void HandleCountChanged(int count)
		{
			countText.SetText("{0}", count);
			onCountTextUpdated?.Invoke();
		}
	}
}
