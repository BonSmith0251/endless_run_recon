using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Monoblok.Input
{
	public class InputRebinder : MonoBehaviour
	{
		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private Button resetButton;

		private InputBindingView[] bindingViews;

		private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

		private int rebindingBindingIndex = -1;

		private void Awake()
		{
			bindingViews = GetComponentsInChildren<InputBindingView>(includeInactive: true);
		}

		private void OnEnable()
		{
			InputBindingView[] array = bindingViews;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnClicked += HandleBindingClicked;
			}
			resetButton.onClick.AddListener(ResetToDefaults);
		}

		private void OnDisable()
		{
			InputBindingView[] array = bindingViews;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnClicked -= HandleBindingClicked;
			}
			resetButton.onClick.RemoveAllListeners();
		}

		private void HandleBindingClicked(InputBindingView view)
		{
			InputAction inputAction = inputReader.Asset.FindAction(view.Action.action.id);
			if (inputAction == null)
			{
				return;
			}
			inputAction.Disable();
			InputBindingView[] array = bindingViews;
			foreach (InputBindingView inputBindingView in array)
			{
				if (!(inputBindingView == null))
				{
					if (inputBindingView == view)
					{
						inputBindingView.SetState(InputBindingView.InputBindingViewState.WaitingForInput);
					}
					else
					{
						inputBindingView.SetState(InputBindingView.InputBindingViewState.Disabled);
					}
				}
			}
			bool flag = inputAction.bindings[view.Index].effectivePath.Contains("<Gamepad>");
			rebindingBindingIndex = view.Index;
			rebindingOperation = inputAction.PerformInteractiveRebinding(view.Index).WithControlsExcluding(flag ? "<Keyboard>" : "<Gamepad>").WithCancelingThrough(flag ? "<Keyboard>/escape" : "<Gamepad>/start")
				.OnPotentialMatch(delegate(InputActionRebindingExtensions.RebindingOperation operation)
				{
					string path = operation.selectedControl.path;
					if (path == "/Keyboard/escape" || path.Contains("/start"))
					{
						operation.Cancel();
					}
				});
			rebindingOperation.Start();
			rebindingOperation.OnComplete(HandleRebindingOperationComplete);
			rebindingOperation.OnCancel(HandleRebingindOperationCancelled);
		}

		private void HandleRebingindOperationCancelled(InputActionRebindingExtensions.RebindingOperation operation)
		{
			operation.action.Enable();
			rebindingOperation.Dispose();
			InputBindingView[] array = bindingViews;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetState(InputBindingView.InputBindingViewState.Default);
			}
			FocusLast(operation);
		}

		private void HandleRebindingOperationComplete(InputActionRebindingExtensions.RebindingOperation operation)
		{
			operation.action.Enable();
			rebindingOperation.Dispose();
			inputReader.SaveRebinds();
			InputBindingView[] array = bindingViews;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetState(InputBindingView.InputBindingViewState.Default);
			}
			FocusLast(operation);
		}

		private void FocusLast(InputActionRebindingExtensions.RebindingOperation operation)
		{
			InputBindingView inputBindingView = bindingViews.FirstOrDefault((InputBindingView v) => v.Action.action.id == operation.action.id && v.Index == rebindingBindingIndex);
			if (!(inputBindingView == null))
			{
				inputBindingView.Focus();
			}
		}

		private void ResetToDefaults()
		{
			inputReader.ResetBindings();
			InputBindingView[] array = bindingViews;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetState(InputBindingView.InputBindingViewState.Default);
			}
		}
	}
}
