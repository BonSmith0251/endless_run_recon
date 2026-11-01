using System;
using System.Collections.Generic;
using System.Linq;
using Monoblok.Core;
using Monoblok.Input;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Monoblok.Upgrades
{
	[RequireComponent(typeof(RectTransform))]
	public class ShopView : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private RectTransform container;

		[SerializeField]
		private AddModuleActionView addModuleActionViewPrefab;

		[SerializeField]
		private GameObject focus;

		[SerializeField]
		private ShopActionTooltip tooltip;

		[SerializeField]
		private ErrorView errorView;

		[Header("Config")]
		[SerializeField]
		private TweenSettings<Vector2> showSettings;

		[SerializeField]
		private TweenSettings<Vector2> hideSettings;

		private Dictionary<Type, IShopActionView> staticViewMap = new Dictionary<Type, IShopActionView>();

		private List<AddModuleActionView> moduleActionViews = new List<AddModuleActionView>();

		public event Action<ShopAction> OnActionRequested;

		private void OnValidate()
		{
			if (rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();
			}
		}

		private void OnEnable()
		{
			InputManager.OnInputDeviceChanged += HandleInputDeviceChanged;
			InputManager.OnSelectionChanged += HandleSelectionChanged;
			HandleInputDeviceChanged(InputManager.CurrentDevice);
		}

		private void OnDisable()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
			InputManager.OnSelectionChanged -= HandleSelectionChanged;
		}

		private void OnDestroy()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
			InputManager.OnSelectionChanged -= HandleSelectionChanged;
		}

		private void Awake()
		{
			container.Clear();
			IShopActionView[] componentsInChildren = GetComponentsInChildren<IShopActionView>(includeInactive: false);
			foreach (IShopActionView shopActionView in componentsInChildren)
			{
				staticViewMap.Add(shopActionView.ActionType, shopActionView);
			}
		}

		public void Initialize(List<AddModuleAction> moduleActions, List<ShopAction> staticActions)
		{
			Clear();
			foreach (AddModuleAction moduleAction in moduleActions)
			{
				AddModuleActionView addModuleActionView = UnityEngine.Object.Instantiate(addModuleActionViewPrefab, container);
				addModuleActionView.Bind(moduleAction);
				addModuleActionView.OnActionRequested += HandleActionRequested;
				moduleAction.IsResolved.OnValueChanged += HandleAddModuleActionResolved;
				moduleActionViews.Add(addModuleActionView);
			}
			foreach (ShopAction staticAction in staticActions)
			{
				if (!TryGetView(staticAction.GetType(), out var view))
				{
					Debug.LogError("Couldn't find view for " + staticAction.GetType().Name);
					continue;
				}
				view.OnActionRequested += HandleActionRequested;
				view.OnPointerEntered += HandleViewPointerEnter;
				view.OnPointerExited += HandleViewPointerExit;
				view.Bind(staticAction);
			}
		}

		private void HandleAddModuleActionResolved(bool isResolved)
		{
			if (isResolved && InputManager.CurrentDevice == InputManager.InputDeviceType.Gamepad)
			{
				Debug.Log("Handling module resolved...");
				AddModuleActionView addModuleActionView = moduleActionViews.FirstOrDefault((AddModuleActionView v) => !v.Action.IsResolved.Value);
				Debug.Log($"Target to focus: {addModuleActionView}", addModuleActionView);
				EventSystem.current.SetSelectedGameObject(addModuleActionView.MainContainer);
			}
		}

		private void HandleActionRequested(ShopAction action)
		{
			this.OnActionRequested?.Invoke(action);
		}

		public void ShowError(string message)
		{
			if (!(errorView == null) && !string.IsNullOrEmpty(message))
			{
				errorView.Show(message);
			}
		}

		[Button(null, EButtonEnableMode.Always)]
		public void Show()
		{
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.interactable = true;
			Tween.UIAnchoredPosition(rectTransform, showSettings);
			if (InputManager.CurrentDevice == InputManager.InputDeviceType.Gamepad)
			{
				EventSystem.current.SetSelectedGameObject(focus);
			}
		}

		private void HandleSelectionChanged(GameObject prev, GameObject curr)
		{
			if (!(curr != null) || !curr.gameObject.activeInHierarchy)
			{
				EventSystem.current.SetSelectedGameObject(focus);
			}
		}

		private void HandleInputDeviceChanged(InputManager.InputDeviceType type)
		{
			if (InputManager.CurrentDevice == InputManager.InputDeviceType.Gamepad)
			{
				EventSystem.current.SetSelectedGameObject(focus);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}

		public void Refocus()
		{
			EventSystem.current.SetSelectedGameObject(focus);
		}

		[Button(null, EButtonEnableMode.Always)]
		public Tween Hide()
		{
			return Tween.UIAnchoredPosition(rectTransform, hideSettings).OnComplete(this, delegate(ShopView t)
			{
				t.canvasGroup.alpha = 0f;
				t.canvasGroup.blocksRaycasts = false;
				t.canvasGroup.interactable = false;
			});
		}

		public void Clear()
		{
			foreach (AddModuleActionView moduleActionView in moduleActionViews)
			{
				if (moduleActionView.Action != null)
				{
					moduleActionView.Action.IsResolved.OnValueChanged -= HandleAddModuleActionResolved;
				}
				moduleActionView.OnActionRequested -= HandleActionRequested;
				moduleActionView.Unbind();
			}
			foreach (KeyValuePair<Type, IShopActionView> item in staticViewMap)
			{
				IShopActionView value = item.Value;
				value.OnActionRequested -= HandleActionRequested;
				value.OnPointerEntered -= HandleViewPointerEnter;
				value.OnPointerExited -= HandleViewPointerExit;
				value.Unbind();
			}
			moduleActionViews.Clear();
			container.Clear();
		}

		private void HandleViewPointerEnter(ShopAction action)
		{
			tooltip.Show(action.metaData);
		}

		private void HandleViewPointerExit(ShopAction action)
		{
			tooltip.Hide();
		}

		private bool TryGetView(Type actionType, out IShopActionView view)
		{
			if (!staticViewMap.TryGetValue(actionType, out view))
			{
				return false;
			}
			return view != null;
		}

		public void RemoveAction(ShopAction action)
		{
			AddModuleActionView addModuleActionView = moduleActionViews.FirstOrDefault((AddModuleActionView v) => v.Action == action);
			if (!(addModuleActionView == null))
			{
				addModuleActionView.Unbind();
				addModuleActionView.OnActionRequested -= HandleActionRequested;
				UnityEngine.Object.Destroy(addModuleActionView.gameObject);
			}
		}
	}
}
