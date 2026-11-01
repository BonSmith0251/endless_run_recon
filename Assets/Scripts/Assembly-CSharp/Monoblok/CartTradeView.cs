using System;
using System.Collections.Generic;
using System.Linq;
using Monoblok.Core;
using Monoblok.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monoblok
{
	public class CartTradeView : MonoBehaviour
	{
		[SerializeField]
		private CartTradeRewardView rewardViewPrefab;

		[SerializeField]
		private RectTransform mainContainer;

		[SerializeField]
		private RectTransform shopContainer;

		[SerializeField]
		private RectTransform container;

		[SerializeField]
		private Button cancelButton;

		private List<CartTradeRewardView> views = new List<CartTradeRewardView>();

		public event Action<CartTradeRewardView> OnSelected;

		public event Action OnCanceled;

		private void OnEnable()
		{
			cancelButton.onClick.AddListener(HandleCancelClicked);
			InputManager.OnInputDeviceChanged += HandleInputDeviceChanged;
			HandleInputDeviceChanged(InputManager.CurrentDevice);
		}

		private void OnDisable()
		{
			cancelButton.onClick.RemoveAllListeners();
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
		}

		private void HandleInputDeviceChanged(InputManager.InputDeviceType type)
		{
			if (type == InputManager.InputDeviceType.Gamepad)
			{
				CartTradeRewardView cartTradeRewardView = views.FirstOrDefault();
				if (!(cartTradeRewardView == null))
				{
					EventSystem.current.SetSelectedGameObject(cartTradeRewardView.FocusGO);
				}
			}
		}

		public void Initialize(CartTradeReward[] rewards)
		{
			Clear();
			foreach (CartTradeReward reward in rewards)
			{
				CartTradeRewardView cartTradeRewardView = UnityEngine.Object.Instantiate(rewardViewPrefab, container);
				cartTradeRewardView.Initialize(reward);
				cartTradeRewardView.OnSelected += HandleSelected;
				views.Add(cartTradeRewardView);
			}
			mainContainer.gameObject.SetActive(value: true);
			shopContainer.gameObject.SetActive(value: false);
		}

		public void Hide()
		{
			mainContainer.gameObject.SetActive(value: false);
			shopContainer.gameObject.SetActive(value: true);
		}

		public void Clear()
		{
			foreach (CartTradeRewardView view in views)
			{
				if (!(view == null))
				{
					view.OnSelected -= HandleSelected;
				}
			}
			container.Clear();
			views.Clear();
		}

		private void HandleSelected(CartTradeRewardView view)
		{
			this.OnSelected?.Invoke(view);
		}

		private void HandleCancelClicked()
		{
			this.OnCanceled?.Invoke();
		}
	}
}
