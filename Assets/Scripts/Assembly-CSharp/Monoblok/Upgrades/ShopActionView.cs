using System;
using InputGlyphs.Display;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Monoblok.Upgrades
{
	public abstract class ShopActionView<T> : MonoBehaviour, IShopActionView, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler where T : ShopAction
	{
		[SerializeField]
		private TMP_Text priceText;

		[SerializeField]
		private InputGlyphImage glyphImage;

		private Color originalPriceTextColor;

		private T action;

		private bool isBound;

		public bool IsBound => isBound;

		public T Action => action;

		public Type ActionType => typeof(T);

		public event Action<ShopAction> OnActionRequested;

		public event Action<ShopAction> OnPointerEntered;

		public event Action<ShopAction> OnPointerExited;

		private void Awake()
		{
			originalPriceTextColor = priceText.color;
		}

		private void OnEnable()
		{
			if (IsBound && action != null && action.Eligible != null)
			{
				OnEligibilityChanged(action.Eligible.Value);
			}
		}

		public void Bind(ShopAction action)
		{
			if (isBound)
			{
				return;
			}
			if (action == null)
			{
				Debug.LogError(typeof(T).Name + " action provided is null");
				return;
			}
			if (!(action is T val))
			{
				Debug.LogError($"Invalid bind: expected {typeof(T)}, got {action.GetType()}");
				return;
			}
			this.action = val;
			if (action.inputActionReference != null && glyphImage != null)
			{
				glyphImage.InputActionReference = action.inputActionReference;
				glyphImage.UpdateGlyphs();
			}
			action.Price.Amount.OnValueChanged += HandlePriceChanged;
			HandlePriceChanged(action.Price.Amount.Value);
			action.Eligible.OnValueChanged += OnEligibilityChanged;
			OnBind(val);
			OnEligibilityChanged(action.Eligible.Value);
			isBound = true;
		}

		private void HandlePriceChanged(int count)
		{
			priceText.text = count.ToString();
			if (isBound)
			{
				OnEligibilityChanged(action.Eligible.Value);
			}
		}

		public void Unbind()
		{
			if (isBound)
			{
				action.Eligible.OnValueChanged -= OnEligibilityChanged;
				action.Price.Amount.OnValueChanged -= HandlePriceChanged;
				OnUnbind();
				action = null;
				isBound = false;
			}
		}

		protected void RequestResolve()
		{
			this.OnActionRequested?.Invoke(action);
		}

		protected virtual void OnEligibilityChanged(EligibilityState state)
		{
			if (state != null && !(priceText == null))
			{
				TMP_Text tMP_Text = priceText;
				Color color = ((state.Reason != IneligibilityReason.InsufficientFunds) ? originalPriceTextColor : Color.red);
				tMP_Text.color = color;
			}
		}

		protected abstract void OnBind(T action);

		protected abstract void OnUnbind();

		public void OnPointerEnter(PointerEventData eventData)
		{
			this.OnPointerEntered?.Invoke(action);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			this.OnPointerExited?.Invoke(action);
		}

		public void OnSelect(BaseEventData eventData)
		{
			this.OnPointerEntered?.Invoke(action);
		}

		public void OnDeselect(BaseEventData eventData)
		{
			this.OnPointerExited?.Invoke(action);
		}
	}
}
