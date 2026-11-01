using System;
using Monoblok.StatSystem;
using Monoblok.Train;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	[RequireComponent(typeof(RectTransform))]
	public class CartInspectorView : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private CartDataView cartDataView;

		[SerializeField]
		private StatCollectionView statCollectionView;

		[SerializeField]
		private SlotCollectionView slotCollectionView;

		[SerializeField]
		private Button tradeButton;

		[Header("Config")]
		[SerializeField]
		private TweenSettings<Vector2> showSettings;

		[SerializeField]
		private TweenSettings<Vector2> hideSettings;

		public event Action<SlotSellView> OnSellModuleClicked;

		public event Action OnTradeClicked;

		private void OnValidate()
		{
			if (rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();
			}
		}

		private void OnEnable()
		{
			slotCollectionView.OnSellModuleClicked += HandleSellModuleClicked;
			tradeButton.onClick.AddListener(HandleTradeButtonClicked);
		}

		private void OnDisable()
		{
			slotCollectionView.OnSellModuleClicked -= HandleSellModuleClicked;
			tradeButton.onClick.RemoveAllListeners();
		}

		public void Initialize(TrainCart cart, bool canTrade)
		{
			cartDataView.Initialize(cart.CartData);
			statCollectionView.Initialize(cart.StatManager.StatMap.Values);
			bool flag = cart.ModuleManager.Slots.Count > 0;
			_ = cart.CartData.CanMerge;
			slotCollectionView.gameObject.SetActive(flag);
			if (flag)
			{
				slotCollectionView.Initialize(cart.ModuleManager.Slots);
			}
			tradeButton.interactable = canTrade;
		}

		private void HandleSellModuleClicked(SlotSellView view)
		{
			this.OnSellModuleClicked?.Invoke(view);
		}

		private void HandleTradeButtonClicked()
		{
			this.OnTradeClicked?.Invoke();
		}
	}
}
