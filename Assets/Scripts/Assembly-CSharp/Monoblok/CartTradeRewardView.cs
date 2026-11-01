using System;
using Monoblok.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class CartTradeRewardView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text descriptionText;

		[SerializeField]
		private Button button;

		[SerializeField]
		private RuntimePriceView priceView;

		private CartTradeReward reward;

		public CartTradeReward Reward => reward;

		public GameObject FocusGO => button.gameObject;

		public event Action<CartTradeRewardView> OnSelected;

		private void OnEnable()
		{
			button.onClick.AddListener(HandleClicked);
		}

		private void OnDisable()
		{
			button.onClick.RemoveAllListeners();
		}

		public void Initialize(CartTradeReward reward)
		{
			this.reward = reward;
			descriptionText.text = reward.Description;
			priceView.Initialize(reward.Price);
		}

		private void HandleClicked()
		{
			this.OnSelected?.Invoke(this);
		}
	}
}
