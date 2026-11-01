using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Monoblok.Input
{
	[CreateAssetMenu(menuName = "Game/Input/Input Reader")]
	public class InputReader : ScriptableObject, InputActions.ITrainActions, InputActions.IStationActions, InputActions.IGlobalActions, InputActions.IDroneActions, InputActions.ICutsceneActions
	{
		private InputActions inputActions;

		private Camera cam;

		private static readonly string RebindKey = "InputBindings";

		public ReadOnlyArray<InputActionMap> ActionMaps
		{
			get
			{
				if (inputActions != null)
				{
					return inputActions.asset.actionMaps;
				}
				return null;
			}
		}

		public InputActionAsset Asset => inputActions?.asset;

		public float TrainMove => inputActions.Train.MoveTrain.ReadValue<float>();

		public bool JumpDownPressed => inputActions.Train.JumpDown.WasPressedThisFrame();

		public bool JumpUpPressed => inputActions.Train.JumpUp.WasPressedThisFrame();

		public bool WaveStartPressed => inputActions.Station.StartWave.WasPressedThisFrame();

		public bool CycleLeftPressed => inputActions.Station.CycleLeft.WasPressedThisFrame();

		public bool CycleRightPressed => inputActions.Station.CycleRight.WasPressedThisFrame();

		public bool SwapLeftPressed => inputActions.Station.SwapLeft.WasPressedThisFrame();

		public bool SwapRightPressed => inputActions.Station.SwapRight.WasPressedThisFrame();

		public bool SubmitPressed => inputActions.Station.Submit.WasPressedThisFrame();

		public Vector2 PointerPosition => inputActions.Global.PointerPosition.ReadValue<Vector2>();

		public bool PointerPressedThisFrame => inputActions.Global.PointerPress.WasPressedThisFrame();

		public bool PointerReleasedThisFrame => inputActions.Global.PointerPress.WasReleasedThisFrame();

		public bool PointerPressed => inputActions.Global.PointerPress.IsPressed();

		public Vector2 Scroll => inputActions.Global.PointerScroll.ReadValue<Vector2>();

		public Vector2 DroneMove => inputActions.Drone.Move.ReadValue<Vector2>();

		public bool DashPerformed => inputActions.Drone.Dash.WasPressedThisFrame();

		public bool SkipPressed => inputActions.Cutscene.Skip.WasPressedThisFrame();

		public bool SkipReleased => inputActions.Cutscene.Skip.WasReleasedThisFrame();

		public Vector2 CameraMove => inputActions.Global.CameraMove.ReadValue<Vector2>();

		public event Action OnPausePerformed;

		public event Action OnCanceled;

		public event Action OnSkipPressed;

		public event Action OnSkipReleased;

		public void Initialize()
		{
			cam = Camera.main;
			if (inputActions == null)
			{
				inputActions = new InputActions();
				inputActions.Train.SetCallbacks(this);
				inputActions.Global.SetCallbacks(this);
				inputActions.Drone.SetCallbacks(this);
				inputActions.Station.SetCallbacks(this);
			}
			inputActions.UI.Enable();
			LoadRebinds();
		}

		public void SaveRebinds()
		{
			if (inputActions != null)
			{
				string value = inputActions.SaveBindingOverridesAsJson();
				PlayerPrefs.SetString(RebindKey, value);
				SyncOverridesToPlayerInput();
			}
		}

		private void LoadRebinds()
		{
			if (inputActions != null)
			{
				string @string = PlayerPrefs.GetString(RebindKey);
				if (!string.IsNullOrEmpty(@string))
				{
					inputActions.LoadBindingOverridesFromJson(@string);
				}
				SyncOverridesToPlayerInput();
			}
		}

		public void ResetBindings()
		{
			if (inputActions != null)
			{
				inputActions.RemoveAllBindingOverrides();
				PlayerPrefs.DeleteKey(RebindKey);
				SyncOverridesToPlayerInput();
			}
		}

		private void SyncOverridesToPlayerInput()
		{
			PlayerInput playerInput = PlayerInput.all.FirstOrDefault();
			if (!(playerInput == null) && !(playerInput.actions == null))
			{
				string json = inputActions.SaveBindingOverridesAsJson();
				playerInput.actions.LoadBindingOverridesFromJson(json);
			}
		}

		[Button(null, EButtonEnableMode.Always)]
		private void Test()
		{
			Debug.Log("Drone enabled: " + inputActions.Drone.enabled);
			Debug.Log("Dash enabled: " + inputActions.Drone.Dash.enabled);
		}

		public void EnableDroneControls(bool enable)
		{
			if (inputActions != null)
			{
				if (enable)
				{
					inputActions.Drone.Enable();
				}
				else
				{
					inputActions.Drone.Disable();
				}
			}
		}

		public void EnableGlobalControls(bool enable)
		{
			if (inputActions != null)
			{
				if (enable)
				{
					inputActions.Global.Enable();
				}
				else
				{
					inputActions.Global.Disable();
				}
			}
		}

		public void EnableStationControls(bool enable)
		{
			if (inputActions != null)
			{
				if (enable)
				{
					inputActions.Station.Enable();
				}
				else
				{
					inputActions.Station.Disable();
				}
			}
		}

		public void EnableTrainControls(bool enable)
		{
			if (inputActions != null)
			{
				if (enable)
				{
					inputActions.Train.Enable();
				}
				else
				{
					inputActions.Train.Disable();
				}
			}
		}

		public void EnableLocoSelectorControls(bool enable)
		{
			if (inputActions != null)
			{
				if (enable)
				{
					inputActions.LocoSelector.Enable();
				}
				else
				{
					inputActions.LocoSelector.Disable();
				}
			}
		}

		public void EnableCutsceneControls(bool enable)
		{
			if (inputActions != null)
			{
				if (enable)
				{
					Debug.Log("Enabled cutscene controls");
					inputActions.Cutscene.Enable();
				}
				else
				{
					inputActions.Cutscene.Disable();
				}
			}
		}

		public void OnMoveTrain(InputAction.CallbackContext context)
		{
		}

		public void OnStartWave(InputAction.CallbackContext context)
		{
		}

		public void OnJumpDown(InputAction.CallbackContext context)
		{
		}

		public void OnJumpUp(InputAction.CallbackContext context)
		{
		}

		public void OnPointerPosition(InputAction.CallbackContext context)
		{
		}

		public void OnPointerPress(InputAction.CallbackContext context)
		{
		}

		public void OnPointerScroll(InputAction.CallbackContext context)
		{
		}

		public void OnMove(InputAction.CallbackContext context)
		{
		}

		public void OnPause(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				this.OnPausePerformed?.Invoke();
			}
		}

		public void OnCancel(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				this.OnCanceled?.Invoke();
			}
		}

		public void OnDash(InputAction.CallbackContext context)
		{
		}

		public void OnCycleLeft(InputAction.CallbackContext context)
		{
		}

		public void OnCycleRight(InputAction.CallbackContext context)
		{
		}

		public void OnSubmit(InputAction.CallbackContext context)
		{
		}

		public void OnSwapLeft(InputAction.CallbackContext context)
		{
		}

		public void OnSwapRight(InputAction.CallbackContext context)
		{
		}

		public void OnAddCart(InputAction.CallbackContext context)
		{
		}

		public void OnRepairCarts(InputAction.CallbackContext context)
		{
		}

		public void OnRerollShop(InputAction.CallbackContext context)
		{
		}

		public void OnMerge(InputAction.CallbackContext context)
		{
		}

		public void OnCameraMove(InputAction.CallbackContext context)
		{
		}

		public void OnRefundUpgrades(InputAction.CallbackContext context)
		{
		}

		public void OnSkip(InputAction.CallbackContext context)
		{
			Debug.Log("Skip");
			if (context.started)
			{
				this.OnSkipPressed?.Invoke();
			}
			else if (context.canceled)
			{
				this.OnSkipReleased?.Invoke();
			}
		}
	}
}
