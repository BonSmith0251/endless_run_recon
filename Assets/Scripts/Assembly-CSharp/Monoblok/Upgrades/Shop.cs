using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Monoblok.Inventory;
using Monoblok.Train;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monoblok.Upgrades
{
	public class Shop : MonoBehaviour
	{
		public enum ShopState
		{
			Idle,
			Pending
		}

		[Header("References")]
		[SerializeField]
		private ShopActionFactory actionFactory;

		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private ShopView shopView;

		[SerializeField]
		private CartSelector cartSelector;

		[SerializeField]
		private CartMergeManager mergeManager;

		[SerializeField]
		private Monoblok.Train.Train train;

		[SerializeField]
		private AbilityManager abilityManager;

		[Header("Config")]
		[SerializeField]
		private int moduleCount;

		[Header("SFX")]
		[SerializeField]
		private EventReference openShopRef;

		private ShopActionContext actionContext;

		private ModuleRollState moduleRollState;

		private List<ShopAction> staticActions;

		private List<AddModuleAction> moduleActions;

		private ShopState currentState;

		private readonly Dictionary<InputActionReference, Action<InputAction.CallbackContext>> _callbacks = new Dictionary<InputActionReference, Action<InputAction.CallbackContext>>();

		public event Action<ShopState> OnStateChanged;

		private void Awake()
		{
			actionContext = new ShopActionContext(this, cartSelector, mergeManager, wallet, train, abilityManager, actionFactory.InsufficientFundsMessage);
			staticActions = actionFactory.GetStaticActions(actionContext);
			moduleRollState = new ModuleRollState();
		}

		private void OnEnable()
		{
			shopView.OnActionRequested += OnActionRequested;
		}

		private void OnDisable()
		{
			shopView.OnActionRequested -= OnActionRequested;
		}

		public void Initialize()
		{
			if (moduleActions == null)
			{
				return;
			}
			foreach (AddModuleAction moduleAction in moduleActions)
			{
				moduleAction.Dispose();
			}
			moduleActions.Clear();
		}

		[Button(null, EButtonEnableMode.Always)]
		public void OpenShop()
		{
			InitializeModuleActions();
			foreach (ShopAction staticAction in staticActions)
			{
				InputActionReference inputActionReference = staticAction.inputActionReference;
				if (inputActionReference != null)
				{
					Action<InputAction.CallbackContext> value = delegate
					{
						OnActionRequested(staticAction);
					};
					_callbacks[inputActionReference] = value;
					inputActionReference.action.performed += value;
				}
				staticAction.Initialize();
			}
			shopView.Initialize(moduleActions, staticActions);
			shopView.Show();
			RuntimeManager.PlayOneShot(openShopRef);
		}

		public Tween CloseShop()
		{
			if (moduleActions != null)
			{
				foreach (AddModuleAction moduleAction in moduleActions)
				{
					if (!moduleAction.IsLocked)
					{
						moduleAction.Dispose();
					}
				}
			}
			if (staticActions != null)
			{
				foreach (ShopAction staticAction in staticActions)
				{
					staticAction.Dispose();
				}
			}
			foreach (var (inputActionReference2, value) in _callbacks)
			{
				inputActionReference2.action.performed -= value;
			}
			_callbacks.Clear();
			shopView.Clear();
			return shopView.Hide();
		}

		public void RerollShop()
		{
			InitializeModuleActions();
			shopView.Initialize(moduleActions, staticActions);
			shopView.Refocus();
		}

		private void InitializeModuleActions()
		{
			List<AddModuleAction> list = moduleActions;
			int num = 0;
			if (list != null)
			{
				foreach (AddModuleAction item in list)
				{
					if (item.IsLocked)
					{
						num++;
					}
					else
					{
						item.Dispose();
					}
				}
			}
			int count = moduleCount - num;
			List<AddModuleAction> list2 = actionFactory.GetModuleActions(actionContext, count, moduleRollState);
			List<AddModuleAction> list3 = new List<AddModuleAction>(moduleCount);
			int num2 = 0;
			if (list != null)
			{
				foreach (AddModuleAction item2 in list)
				{
					list3.Add(item2.IsLocked ? item2 : list2[num2++]);
				}
			}
			while (num2 < list2.Count)
			{
				list3.Add(list2[num2++]);
			}
			moduleActions = list3;
			foreach (AddModuleAction moduleAction in moduleActions)
			{
				if (!moduleAction.IsLocked)
				{
					moduleAction.Initialize();
				}
			}
		}

		private void OnActionRequested(ShopAction action)
		{
			if (currentState != ShopState.Pending)
			{
				ResolveAsync(action).Forget();
			}
		}

		private async UniTaskVoid ResolveAsync(ShopAction action)
		{
			CurrencyDefinition currency = action.Price.Currency;
			int amount = action.Price.Amount.Value;
			UniTask<IShopAction.ShopActionState> uniTask = action.Resolve();
			bool wentAsync = uniTask.Status == UniTaskStatus.Pending;
			if (wentAsync)
			{
				ChangeState(ShopState.Pending);
			}
			IShopAction.ShopActionState shopActionState = await uniTask;
			if (wentAsync)
			{
				ChangeState(ShopState.Idle);
			}
			switch (shopActionState)
			{
			case IShopAction.ShopActionState.Forbidden:
				shopView.ShowError(action.Eligible.Value.Message);
				break;
			case IShopAction.ShopActionState.Completed:
				if (action.metaData != null)
				{
					RuntimeManager.PlayOneShot(action.metaData.ResolveEventRef);
				}
				wallet.RemoveCurrency(currency, amount);
				if (!staticActions.Contains(action))
				{
					action.Dispose();
				}
				break;
			}
		}

		private void ChangeState(ShopState newState)
		{
			if (newState != currentState)
			{
				currentState = newState;
				this.OnStateChanged?.Invoke(currentState);
			}
		}
	}
}
