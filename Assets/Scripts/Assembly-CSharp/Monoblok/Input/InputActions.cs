using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Monoblok.Input
{
	public class InputActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
	{
		public struct TrainActions
		{
			private InputActions m_Wrapper;

			public InputAction MoveTrain => m_Wrapper.m_Train_MoveTrain;

			public InputAction JumpDown => m_Wrapper.m_Train_JumpDown;

			public InputAction JumpUp => m_Wrapper.m_Train_JumpUp;

			public bool enabled => Get().enabled;

			public TrainActions(InputActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputActionMap Get()
			{
				return m_Wrapper.m_Train;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public static implicit operator InputActionMap(TrainActions set)
			{
				return set.Get();
			}

			public void AddCallbacks(ITrainActions instance)
			{
				if (instance != null && !m_Wrapper.m_TrainActionsCallbackInterfaces.Contains(instance))
				{
					m_Wrapper.m_TrainActionsCallbackInterfaces.Add(instance);
					MoveTrain.started += instance.OnMoveTrain;
					MoveTrain.performed += instance.OnMoveTrain;
					MoveTrain.canceled += instance.OnMoveTrain;
					JumpDown.started += instance.OnJumpDown;
					JumpDown.performed += instance.OnJumpDown;
					JumpDown.canceled += instance.OnJumpDown;
					JumpUp.started += instance.OnJumpUp;
					JumpUp.performed += instance.OnJumpUp;
					JumpUp.canceled += instance.OnJumpUp;
				}
			}

			private void UnregisterCallbacks(ITrainActions instance)
			{
				MoveTrain.started -= instance.OnMoveTrain;
				MoveTrain.performed -= instance.OnMoveTrain;
				MoveTrain.canceled -= instance.OnMoveTrain;
				JumpDown.started -= instance.OnJumpDown;
				JumpDown.performed -= instance.OnJumpDown;
				JumpDown.canceled -= instance.OnJumpDown;
				JumpUp.started -= instance.OnJumpUp;
				JumpUp.performed -= instance.OnJumpUp;
				JumpUp.canceled -= instance.OnJumpUp;
			}

			public void RemoveCallbacks(ITrainActions instance)
			{
				if (m_Wrapper.m_TrainActionsCallbackInterfaces.Remove(instance))
				{
					UnregisterCallbacks(instance);
				}
			}

			public void SetCallbacks(ITrainActions instance)
			{
				foreach (ITrainActions trainActionsCallbackInterface in m_Wrapper.m_TrainActionsCallbackInterfaces)
				{
					UnregisterCallbacks(trainActionsCallbackInterface);
				}
				m_Wrapper.m_TrainActionsCallbackInterfaces.Clear();
				AddCallbacks(instance);
			}
		}

		public struct UIActions
		{
			private InputActions m_Wrapper;

			public InputAction Navigate => m_Wrapper.m_UI_Navigate;

			public InputAction Submit => m_Wrapper.m_UI_Submit;

			public InputAction Cancel => m_Wrapper.m_UI_Cancel;

			public InputAction Point => m_Wrapper.m_UI_Point;

			public InputAction Click => m_Wrapper.m_UI_Click;

			public InputAction ScrollWheel => m_Wrapper.m_UI_ScrollWheel;

			public InputAction MiddleClick => m_Wrapper.m_UI_MiddleClick;

			public InputAction RightClick => m_Wrapper.m_UI_RightClick;

			public InputAction TrackedDevicePosition => m_Wrapper.m_UI_TrackedDevicePosition;

			public InputAction TrackedDeviceOrientation => m_Wrapper.m_UI_TrackedDeviceOrientation;

			public bool enabled => Get().enabled;

			public UIActions(InputActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputActionMap Get()
			{
				return m_Wrapper.m_UI;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public static implicit operator InputActionMap(UIActions set)
			{
				return set.Get();
			}

			public void AddCallbacks(IUIActions instance)
			{
				if (instance != null && !m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance))
				{
					m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
					Navigate.started += instance.OnNavigate;
					Navigate.performed += instance.OnNavigate;
					Navigate.canceled += instance.OnNavigate;
					Submit.started += instance.OnSubmit;
					Submit.performed += instance.OnSubmit;
					Submit.canceled += instance.OnSubmit;
					Cancel.started += instance.OnCancel;
					Cancel.performed += instance.OnCancel;
					Cancel.canceled += instance.OnCancel;
					Point.started += instance.OnPoint;
					Point.performed += instance.OnPoint;
					Point.canceled += instance.OnPoint;
					Click.started += instance.OnClick;
					Click.performed += instance.OnClick;
					Click.canceled += instance.OnClick;
					ScrollWheel.started += instance.OnScrollWheel;
					ScrollWheel.performed += instance.OnScrollWheel;
					ScrollWheel.canceled += instance.OnScrollWheel;
					MiddleClick.started += instance.OnMiddleClick;
					MiddleClick.performed += instance.OnMiddleClick;
					MiddleClick.canceled += instance.OnMiddleClick;
					RightClick.started += instance.OnRightClick;
					RightClick.performed += instance.OnRightClick;
					RightClick.canceled += instance.OnRightClick;
					TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
					TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
					TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
					TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
					TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
					TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
				}
			}

			private void UnregisterCallbacks(IUIActions instance)
			{
				Navigate.started -= instance.OnNavigate;
				Navigate.performed -= instance.OnNavigate;
				Navigate.canceled -= instance.OnNavigate;
				Submit.started -= instance.OnSubmit;
				Submit.performed -= instance.OnSubmit;
				Submit.canceled -= instance.OnSubmit;
				Cancel.started -= instance.OnCancel;
				Cancel.performed -= instance.OnCancel;
				Cancel.canceled -= instance.OnCancel;
				Point.started -= instance.OnPoint;
				Point.performed -= instance.OnPoint;
				Point.canceled -= instance.OnPoint;
				Click.started -= instance.OnClick;
				Click.performed -= instance.OnClick;
				Click.canceled -= instance.OnClick;
				ScrollWheel.started -= instance.OnScrollWheel;
				ScrollWheel.performed -= instance.OnScrollWheel;
				ScrollWheel.canceled -= instance.OnScrollWheel;
				MiddleClick.started -= instance.OnMiddleClick;
				MiddleClick.performed -= instance.OnMiddleClick;
				MiddleClick.canceled -= instance.OnMiddleClick;
				RightClick.started -= instance.OnRightClick;
				RightClick.performed -= instance.OnRightClick;
				RightClick.canceled -= instance.OnRightClick;
				TrackedDevicePosition.started -= instance.OnTrackedDevicePosition;
				TrackedDevicePosition.performed -= instance.OnTrackedDevicePosition;
				TrackedDevicePosition.canceled -= instance.OnTrackedDevicePosition;
				TrackedDeviceOrientation.started -= instance.OnTrackedDeviceOrientation;
				TrackedDeviceOrientation.performed -= instance.OnTrackedDeviceOrientation;
				TrackedDeviceOrientation.canceled -= instance.OnTrackedDeviceOrientation;
			}

			public void RemoveCallbacks(IUIActions instance)
			{
				if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
				{
					UnregisterCallbacks(instance);
				}
			}

			public void SetCallbacks(IUIActions instance)
			{
				foreach (IUIActions uIActionsCallbackInterface in m_Wrapper.m_UIActionsCallbackInterfaces)
				{
					UnregisterCallbacks(uIActionsCallbackInterface);
				}
				m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
				AddCallbacks(instance);
			}
		}

		public struct StationActions
		{
			private InputActions m_Wrapper;

			public InputAction StartWave => m_Wrapper.m_Station_StartWave;

			public InputAction Cancel => m_Wrapper.m_Station_Cancel;

			public InputAction CycleLeft => m_Wrapper.m_Station_CycleLeft;

			public InputAction CycleRight => m_Wrapper.m_Station_CycleRight;

			public InputAction Submit => m_Wrapper.m_Station_Submit;

			public InputAction SwapLeft => m_Wrapper.m_Station_SwapLeft;

			public InputAction SwapRight => m_Wrapper.m_Station_SwapRight;

			public InputAction AddCart => m_Wrapper.m_Station_AddCart;

			public InputAction RepairCarts => m_Wrapper.m_Station_RepairCarts;

			public InputAction RerollShop => m_Wrapper.m_Station_RerollShop;

			public InputAction Merge => m_Wrapper.m_Station_Merge;

			public bool enabled => Get().enabled;

			public StationActions(InputActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputActionMap Get()
			{
				return m_Wrapper.m_Station;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public static implicit operator InputActionMap(StationActions set)
			{
				return set.Get();
			}

			public void AddCallbacks(IStationActions instance)
			{
				if (instance != null && !m_Wrapper.m_StationActionsCallbackInterfaces.Contains(instance))
				{
					m_Wrapper.m_StationActionsCallbackInterfaces.Add(instance);
					StartWave.started += instance.OnStartWave;
					StartWave.performed += instance.OnStartWave;
					StartWave.canceled += instance.OnStartWave;
					Cancel.started += instance.OnCancel;
					Cancel.performed += instance.OnCancel;
					Cancel.canceled += instance.OnCancel;
					CycleLeft.started += instance.OnCycleLeft;
					CycleLeft.performed += instance.OnCycleLeft;
					CycleLeft.canceled += instance.OnCycleLeft;
					CycleRight.started += instance.OnCycleRight;
					CycleRight.performed += instance.OnCycleRight;
					CycleRight.canceled += instance.OnCycleRight;
					Submit.started += instance.OnSubmit;
					Submit.performed += instance.OnSubmit;
					Submit.canceled += instance.OnSubmit;
					SwapLeft.started += instance.OnSwapLeft;
					SwapLeft.performed += instance.OnSwapLeft;
					SwapLeft.canceled += instance.OnSwapLeft;
					SwapRight.started += instance.OnSwapRight;
					SwapRight.performed += instance.OnSwapRight;
					SwapRight.canceled += instance.OnSwapRight;
					AddCart.started += instance.OnAddCart;
					AddCart.performed += instance.OnAddCart;
					AddCart.canceled += instance.OnAddCart;
					RepairCarts.started += instance.OnRepairCarts;
					RepairCarts.performed += instance.OnRepairCarts;
					RepairCarts.canceled += instance.OnRepairCarts;
					RerollShop.started += instance.OnRerollShop;
					RerollShop.performed += instance.OnRerollShop;
					RerollShop.canceled += instance.OnRerollShop;
					Merge.started += instance.OnMerge;
					Merge.performed += instance.OnMerge;
					Merge.canceled += instance.OnMerge;
				}
			}

			private void UnregisterCallbacks(IStationActions instance)
			{
				StartWave.started -= instance.OnStartWave;
				StartWave.performed -= instance.OnStartWave;
				StartWave.canceled -= instance.OnStartWave;
				Cancel.started -= instance.OnCancel;
				Cancel.performed -= instance.OnCancel;
				Cancel.canceled -= instance.OnCancel;
				CycleLeft.started -= instance.OnCycleLeft;
				CycleLeft.performed -= instance.OnCycleLeft;
				CycleLeft.canceled -= instance.OnCycleLeft;
				CycleRight.started -= instance.OnCycleRight;
				CycleRight.performed -= instance.OnCycleRight;
				CycleRight.canceled -= instance.OnCycleRight;
				Submit.started -= instance.OnSubmit;
				Submit.performed -= instance.OnSubmit;
				Submit.canceled -= instance.OnSubmit;
				SwapLeft.started -= instance.OnSwapLeft;
				SwapLeft.performed -= instance.OnSwapLeft;
				SwapLeft.canceled -= instance.OnSwapLeft;
				SwapRight.started -= instance.OnSwapRight;
				SwapRight.performed -= instance.OnSwapRight;
				SwapRight.canceled -= instance.OnSwapRight;
				AddCart.started -= instance.OnAddCart;
				AddCart.performed -= instance.OnAddCart;
				AddCart.canceled -= instance.OnAddCart;
				RepairCarts.started -= instance.OnRepairCarts;
				RepairCarts.performed -= instance.OnRepairCarts;
				RepairCarts.canceled -= instance.OnRepairCarts;
				RerollShop.started -= instance.OnRerollShop;
				RerollShop.performed -= instance.OnRerollShop;
				RerollShop.canceled -= instance.OnRerollShop;
				Merge.started -= instance.OnMerge;
				Merge.performed -= instance.OnMerge;
				Merge.canceled -= instance.OnMerge;
			}

			public void RemoveCallbacks(IStationActions instance)
			{
				if (m_Wrapper.m_StationActionsCallbackInterfaces.Remove(instance))
				{
					UnregisterCallbacks(instance);
				}
			}

			public void SetCallbacks(IStationActions instance)
			{
				foreach (IStationActions stationActionsCallbackInterface in m_Wrapper.m_StationActionsCallbackInterfaces)
				{
					UnregisterCallbacks(stationActionsCallbackInterface);
				}
				m_Wrapper.m_StationActionsCallbackInterfaces.Clear();
				AddCallbacks(instance);
			}
		}

		public struct DroneActions
		{
			private InputActions m_Wrapper;

			public InputAction Move => m_Wrapper.m_Drone_Move;

			public InputAction Dash => m_Wrapper.m_Drone_Dash;

			public bool enabled => Get().enabled;

			public DroneActions(InputActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputActionMap Get()
			{
				return m_Wrapper.m_Drone;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public static implicit operator InputActionMap(DroneActions set)
			{
				return set.Get();
			}

			public void AddCallbacks(IDroneActions instance)
			{
				if (instance != null && !m_Wrapper.m_DroneActionsCallbackInterfaces.Contains(instance))
				{
					m_Wrapper.m_DroneActionsCallbackInterfaces.Add(instance);
					Move.started += instance.OnMove;
					Move.performed += instance.OnMove;
					Move.canceled += instance.OnMove;
					Dash.started += instance.OnDash;
					Dash.performed += instance.OnDash;
					Dash.canceled += instance.OnDash;
				}
			}

			private void UnregisterCallbacks(IDroneActions instance)
			{
				Move.started -= instance.OnMove;
				Move.performed -= instance.OnMove;
				Move.canceled -= instance.OnMove;
				Dash.started -= instance.OnDash;
				Dash.performed -= instance.OnDash;
				Dash.canceled -= instance.OnDash;
			}

			public void RemoveCallbacks(IDroneActions instance)
			{
				if (m_Wrapper.m_DroneActionsCallbackInterfaces.Remove(instance))
				{
					UnregisterCallbacks(instance);
				}
			}

			public void SetCallbacks(IDroneActions instance)
			{
				foreach (IDroneActions droneActionsCallbackInterface in m_Wrapper.m_DroneActionsCallbackInterfaces)
				{
					UnregisterCallbacks(droneActionsCallbackInterface);
				}
				m_Wrapper.m_DroneActionsCallbackInterfaces.Clear();
				AddCallbacks(instance);
			}
		}

		public struct GlobalActions
		{
			private InputActions m_Wrapper;

			public InputAction Pause => m_Wrapper.m_Global_Pause;

			public InputAction PointerPosition => m_Wrapper.m_Global_PointerPosition;

			public InputAction PointerPress => m_Wrapper.m_Global_PointerPress;

			public InputAction PointerScroll => m_Wrapper.m_Global_PointerScroll;

			public InputAction CameraMove => m_Wrapper.m_Global_CameraMove;

			public InputAction RefundUpgrades => m_Wrapper.m_Global_RefundUpgrades;

			public bool enabled => Get().enabled;

			public GlobalActions(InputActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputActionMap Get()
			{
				return m_Wrapper.m_Global;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public static implicit operator InputActionMap(GlobalActions set)
			{
				return set.Get();
			}

			public void AddCallbacks(IGlobalActions instance)
			{
				if (instance != null && !m_Wrapper.m_GlobalActionsCallbackInterfaces.Contains(instance))
				{
					m_Wrapper.m_GlobalActionsCallbackInterfaces.Add(instance);
					Pause.started += instance.OnPause;
					Pause.performed += instance.OnPause;
					Pause.canceled += instance.OnPause;
					PointerPosition.started += instance.OnPointerPosition;
					PointerPosition.performed += instance.OnPointerPosition;
					PointerPosition.canceled += instance.OnPointerPosition;
					PointerPress.started += instance.OnPointerPress;
					PointerPress.performed += instance.OnPointerPress;
					PointerPress.canceled += instance.OnPointerPress;
					PointerScroll.started += instance.OnPointerScroll;
					PointerScroll.performed += instance.OnPointerScroll;
					PointerScroll.canceled += instance.OnPointerScroll;
					CameraMove.started += instance.OnCameraMove;
					CameraMove.performed += instance.OnCameraMove;
					CameraMove.canceled += instance.OnCameraMove;
					RefundUpgrades.started += instance.OnRefundUpgrades;
					RefundUpgrades.performed += instance.OnRefundUpgrades;
					RefundUpgrades.canceled += instance.OnRefundUpgrades;
				}
			}

			private void UnregisterCallbacks(IGlobalActions instance)
			{
				Pause.started -= instance.OnPause;
				Pause.performed -= instance.OnPause;
				Pause.canceled -= instance.OnPause;
				PointerPosition.started -= instance.OnPointerPosition;
				PointerPosition.performed -= instance.OnPointerPosition;
				PointerPosition.canceled -= instance.OnPointerPosition;
				PointerPress.started -= instance.OnPointerPress;
				PointerPress.performed -= instance.OnPointerPress;
				PointerPress.canceled -= instance.OnPointerPress;
				PointerScroll.started -= instance.OnPointerScroll;
				PointerScroll.performed -= instance.OnPointerScroll;
				PointerScroll.canceled -= instance.OnPointerScroll;
				CameraMove.started -= instance.OnCameraMove;
				CameraMove.performed -= instance.OnCameraMove;
				CameraMove.canceled -= instance.OnCameraMove;
				RefundUpgrades.started -= instance.OnRefundUpgrades;
				RefundUpgrades.performed -= instance.OnRefundUpgrades;
				RefundUpgrades.canceled -= instance.OnRefundUpgrades;
			}

			public void RemoveCallbacks(IGlobalActions instance)
			{
				if (m_Wrapper.m_GlobalActionsCallbackInterfaces.Remove(instance))
				{
					UnregisterCallbacks(instance);
				}
			}

			public void SetCallbacks(IGlobalActions instance)
			{
				foreach (IGlobalActions globalActionsCallbackInterface in m_Wrapper.m_GlobalActionsCallbackInterfaces)
				{
					UnregisterCallbacks(globalActionsCallbackInterface);
				}
				m_Wrapper.m_GlobalActionsCallbackInterfaces.Clear();
				AddCallbacks(instance);
			}
		}

		public struct LocoSelectorActions
		{
			private InputActions m_Wrapper;

			public InputAction CycleLeft => m_Wrapper.m_LocoSelector_CycleLeft;

			public InputAction CycleRight => m_Wrapper.m_LocoSelector_CycleRight;

			public bool enabled => Get().enabled;

			public LocoSelectorActions(InputActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputActionMap Get()
			{
				return m_Wrapper.m_LocoSelector;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public static implicit operator InputActionMap(LocoSelectorActions set)
			{
				return set.Get();
			}

			public void AddCallbacks(ILocoSelectorActions instance)
			{
				if (instance != null && !m_Wrapper.m_LocoSelectorActionsCallbackInterfaces.Contains(instance))
				{
					m_Wrapper.m_LocoSelectorActionsCallbackInterfaces.Add(instance);
					CycleLeft.started += instance.OnCycleLeft;
					CycleLeft.performed += instance.OnCycleLeft;
					CycleLeft.canceled += instance.OnCycleLeft;
					CycleRight.started += instance.OnCycleRight;
					CycleRight.performed += instance.OnCycleRight;
					CycleRight.canceled += instance.OnCycleRight;
				}
			}

			private void UnregisterCallbacks(ILocoSelectorActions instance)
			{
				CycleLeft.started -= instance.OnCycleLeft;
				CycleLeft.performed -= instance.OnCycleLeft;
				CycleLeft.canceled -= instance.OnCycleLeft;
				CycleRight.started -= instance.OnCycleRight;
				CycleRight.performed -= instance.OnCycleRight;
				CycleRight.canceled -= instance.OnCycleRight;
			}

			public void RemoveCallbacks(ILocoSelectorActions instance)
			{
				if (m_Wrapper.m_LocoSelectorActionsCallbackInterfaces.Remove(instance))
				{
					UnregisterCallbacks(instance);
				}
			}

			public void SetCallbacks(ILocoSelectorActions instance)
			{
				foreach (ILocoSelectorActions locoSelectorActionsCallbackInterface in m_Wrapper.m_LocoSelectorActionsCallbackInterfaces)
				{
					UnregisterCallbacks(locoSelectorActionsCallbackInterface);
				}
				m_Wrapper.m_LocoSelectorActionsCallbackInterfaces.Clear();
				AddCallbacks(instance);
			}
		}

		public struct CutsceneActions
		{
			private InputActions m_Wrapper;

			public InputAction Skip => m_Wrapper.m_Cutscene_Skip;

			public bool enabled => Get().enabled;

			public CutsceneActions(InputActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputActionMap Get()
			{
				return m_Wrapper.m_Cutscene;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public static implicit operator InputActionMap(CutsceneActions set)
			{
				return set.Get();
			}

			public void AddCallbacks(ICutsceneActions instance)
			{
				if (instance != null && !m_Wrapper.m_CutsceneActionsCallbackInterfaces.Contains(instance))
				{
					m_Wrapper.m_CutsceneActionsCallbackInterfaces.Add(instance);
					Skip.started += instance.OnSkip;
					Skip.performed += instance.OnSkip;
					Skip.canceled += instance.OnSkip;
				}
			}

			private void UnregisterCallbacks(ICutsceneActions instance)
			{
				Skip.started -= instance.OnSkip;
				Skip.performed -= instance.OnSkip;
				Skip.canceled -= instance.OnSkip;
			}

			public void RemoveCallbacks(ICutsceneActions instance)
			{
				if (m_Wrapper.m_CutsceneActionsCallbackInterfaces.Remove(instance))
				{
					UnregisterCallbacks(instance);
				}
			}

			public void SetCallbacks(ICutsceneActions instance)
			{
				foreach (ICutsceneActions cutsceneActionsCallbackInterface in m_Wrapper.m_CutsceneActionsCallbackInterfaces)
				{
					UnregisterCallbacks(cutsceneActionsCallbackInterface);
				}
				m_Wrapper.m_CutsceneActionsCallbackInterfaces.Clear();
				AddCallbacks(instance);
			}
		}

		public interface ITrainActions
		{
			void OnMoveTrain(InputAction.CallbackContext context);

			void OnJumpDown(InputAction.CallbackContext context);

			void OnJumpUp(InputAction.CallbackContext context);
		}

		public interface IUIActions
		{
			void OnNavigate(InputAction.CallbackContext context);

			void OnSubmit(InputAction.CallbackContext context);

			void OnCancel(InputAction.CallbackContext context);

			void OnPoint(InputAction.CallbackContext context);

			void OnClick(InputAction.CallbackContext context);

			void OnScrollWheel(InputAction.CallbackContext context);

			void OnMiddleClick(InputAction.CallbackContext context);

			void OnRightClick(InputAction.CallbackContext context);

			void OnTrackedDevicePosition(InputAction.CallbackContext context);

			void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
		}

		public interface IStationActions
		{
			void OnStartWave(InputAction.CallbackContext context);

			void OnCancel(InputAction.CallbackContext context);

			void OnCycleLeft(InputAction.CallbackContext context);

			void OnCycleRight(InputAction.CallbackContext context);

			void OnSubmit(InputAction.CallbackContext context);

			void OnSwapLeft(InputAction.CallbackContext context);

			void OnSwapRight(InputAction.CallbackContext context);

			void OnAddCart(InputAction.CallbackContext context);

			void OnRepairCarts(InputAction.CallbackContext context);

			void OnRerollShop(InputAction.CallbackContext context);

			void OnMerge(InputAction.CallbackContext context);
		}

		public interface IDroneActions
		{
			void OnMove(InputAction.CallbackContext context);

			void OnDash(InputAction.CallbackContext context);
		}

		public interface IGlobalActions
		{
			void OnPause(InputAction.CallbackContext context);

			void OnPointerPosition(InputAction.CallbackContext context);

			void OnPointerPress(InputAction.CallbackContext context);

			void OnPointerScroll(InputAction.CallbackContext context);

			void OnCameraMove(InputAction.CallbackContext context);

			void OnRefundUpgrades(InputAction.CallbackContext context);
		}

		public interface ILocoSelectorActions
		{
			void OnCycleLeft(InputAction.CallbackContext context);

			void OnCycleRight(InputAction.CallbackContext context);
		}

		public interface ICutsceneActions
		{
			void OnSkip(InputAction.CallbackContext context);
		}

		private readonly InputActionMap m_Train;

		private List<ITrainActions> m_TrainActionsCallbackInterfaces = new List<ITrainActions>();

		private readonly InputAction m_Train_MoveTrain;

		private readonly InputAction m_Train_JumpDown;

		private readonly InputAction m_Train_JumpUp;

		private readonly InputActionMap m_UI;

		private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();

		private readonly InputAction m_UI_Navigate;

		private readonly InputAction m_UI_Submit;

		private readonly InputAction m_UI_Cancel;

		private readonly InputAction m_UI_Point;

		private readonly InputAction m_UI_Click;

		private readonly InputAction m_UI_ScrollWheel;

		private readonly InputAction m_UI_MiddleClick;

		private readonly InputAction m_UI_RightClick;

		private readonly InputAction m_UI_TrackedDevicePosition;

		private readonly InputAction m_UI_TrackedDeviceOrientation;

		private readonly InputActionMap m_Station;

		private List<IStationActions> m_StationActionsCallbackInterfaces = new List<IStationActions>();

		private readonly InputAction m_Station_StartWave;

		private readonly InputAction m_Station_Cancel;

		private readonly InputAction m_Station_CycleLeft;

		private readonly InputAction m_Station_CycleRight;

		private readonly InputAction m_Station_Submit;

		private readonly InputAction m_Station_SwapLeft;

		private readonly InputAction m_Station_SwapRight;

		private readonly InputAction m_Station_AddCart;

		private readonly InputAction m_Station_RepairCarts;

		private readonly InputAction m_Station_RerollShop;

		private readonly InputAction m_Station_Merge;

		private readonly InputActionMap m_Drone;

		private List<IDroneActions> m_DroneActionsCallbackInterfaces = new List<IDroneActions>();

		private readonly InputAction m_Drone_Move;

		private readonly InputAction m_Drone_Dash;

		private readonly InputActionMap m_Global;

		private List<IGlobalActions> m_GlobalActionsCallbackInterfaces = new List<IGlobalActions>();

		private readonly InputAction m_Global_Pause;

		private readonly InputAction m_Global_PointerPosition;

		private readonly InputAction m_Global_PointerPress;

		private readonly InputAction m_Global_PointerScroll;

		private readonly InputAction m_Global_CameraMove;

		private readonly InputAction m_Global_RefundUpgrades;

		private readonly InputActionMap m_LocoSelector;

		private List<ILocoSelectorActions> m_LocoSelectorActionsCallbackInterfaces = new List<ILocoSelectorActions>();

		private readonly InputAction m_LocoSelector_CycleLeft;

		private readonly InputAction m_LocoSelector_CycleRight;

		private readonly InputActionMap m_Cutscene;

		private List<ICutsceneActions> m_CutsceneActionsCallbackInterfaces = new List<ICutsceneActions>();

		private readonly InputAction m_Cutscene_Skip;

		private int m_KBMSchemeIndex = -1;

		private int m_ControllerSchemeIndex = -1;

		public InputActionAsset asset { get; }

		public InputBinding? bindingMask
		{
			get
			{
				return asset.bindingMask;
			}
			set
			{
				asset.bindingMask = value;
			}
		}

		public ReadOnlyArray<InputDevice>? devices
		{
			get
			{
				return asset.devices;
			}
			set
			{
				asset.devices = value;
			}
		}

		public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

		public IEnumerable<InputBinding> bindings => asset.bindings;

		public TrainActions Train => new TrainActions(this);

		public UIActions UI => new UIActions(this);

		public StationActions Station => new StationActions(this);

		public DroneActions Drone => new DroneActions(this);

		public GlobalActions Global => new GlobalActions(this);

		public LocoSelectorActions LocoSelector => new LocoSelectorActions(this);

		public CutsceneActions Cutscene => new CutsceneActions(this);

		public InputControlScheme KBMScheme
		{
			get
			{
				if (m_KBMSchemeIndex == -1)
				{
					m_KBMSchemeIndex = asset.FindControlSchemeIndex("KBM");
				}
				return asset.controlSchemes[m_KBMSchemeIndex];
			}
		}

		public InputControlScheme ControllerScheme
		{
			get
			{
				if (m_ControllerSchemeIndex == -1)
				{
					m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
				}
				return asset.controlSchemes[m_ControllerSchemeIndex];
			}
		}

		public InputActions()
		{
			asset = InputActionAsset.FromJson("{\n    \"version\": 1,\n    \"name\": \"Input Actions\",\n    \"maps\": [\n        {\n            \"name\": \"Train\",\n            \"id\": \"c95bf1d8-febb-47e8-9979-54d20cbf6898\",\n            \"actions\": [\n                {\n                    \"name\": \"MoveTrain\",\n                    \"type\": \"Value\",\n                    \"id\": \"4fe0ab37-7574-4341-8903-3faaf6d9af19\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"JumpDown\",\n                    \"type\": \"Button\",\n                    \"id\": \"ed36d79c-90fb-4d32-a4a6-968cf7e3ff01\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"JumpUp\",\n                    \"type\": \"Button\",\n                    \"id\": \"5a8bf007-9d23-480e-aac0-afc830ff848e\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"1D Axis\",\n                    \"id\": \"d423f67f-9b50-42de-9bfc-f52158b03688\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveTrain\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"negative\",\n                    \"id\": \"35781c3f-33c7-4715-8e3e-c3d9d3ad5aa6\",\n                    \"path\": \"<Gamepad>/leftTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Controller\",\n                    \"action\": \"MoveTrain\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"baed0fdb-0100-4921-b0c8-5fe540e498c4\",\n                    \"path\": \"<Gamepad>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Controller\",\n                    \"action\": \"MoveTrain\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"1D Axis\",\n                    \"id\": \"3f1211fb-aa7a-4c00-91c5-4b4d584f063a\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveTrain\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"negative\",\n                    \"id\": \"a6b2c0e7-c458-415f-84e3-e7e8120ea623\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"MoveTrain\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"2e84d035-f6c5-4110-9326-4af3768a988b\",\n                    \"path\": \"<Mouse>/rightButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"MoveTrain\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c93c221a-124c-4941-9593-c34a410f4733\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"JumpDown\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"cfaee89d-2faf-4c56-9423-5c0ab73975bd\",\n                    \"path\": \"<Gamepad>/dpad/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"JumpDown\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"4702a389-c419-4505-a99a-c0837b1853e5\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"JumpUp\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"a4108273-dd4a-4c55-84b9-905b1ae1c153\",\n                    \"path\": \"<Gamepad>/dpad/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"JumpUp\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"UI\",\n            \"id\": \"944dbdb6-beb8-40c5-a5a0-2b7053b57bac\",\n            \"actions\": [\n                {\n                    \"name\": \"Navigate\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"5005c114-c0d1-4fe9-a4ae-65f1a13b9454\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Submit\",\n                    \"type\": \"Button\",\n                    \"id\": \"1e594715-15ad-4d2e-81fa-6c39cb246de8\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Cancel\",\n                    \"type\": \"Button\",\n                    \"id\": \"04565325-0af8-4c77-a71b-b0aa63733479\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Point\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"d73adcef-427f-446f-a48e-195109d06284\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Click\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"7d23b900-1047-408e-88e9-03c5b19589e5\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"ScrollWheel\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"8f5c4a96-6859-4016-9ae9-19b91cad39aa\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"MiddleClick\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"feb993b1-9431-415a-b55a-d8d7e1155721\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"RightClick\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"d75ffca7-031b-44cf-88aa-e3193f297f24\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"TrackedDevicePosition\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"eacae063-9fe8-40b4-b746-fd38262efead\",\n                    \"expectedControlType\": \"Vector3\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"TrackedDeviceOrientation\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"6a057297-9241-4841-8ddd-33563942833c\",\n                    \"expectedControlType\": \"Quaternion\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"Gamepad\",\n                    \"id\": \"9b633275-f838-4cfa-93db-d47d762aaaf5\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"StickDeadzone\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"ccbb3605-4d13-4601-a922-ae843c599fa8\",\n                    \"path\": \"<Gamepad>/leftStick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"1766ead0-0549-4f4d-956c-32c31b22d81f\",\n                    \"path\": \"<Gamepad>/dpad/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad;Controller\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"199dd97a-5c13-4d1b-865c-84ca4eaa2165\",\n                    \"path\": \"<Gamepad>/leftStick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"7fa5784a-1d40-41f6-afc9-922222ea0f34\",\n                    \"path\": \"<Gamepad>/dpad/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"386f59d9-069e-4a97-97b0-a143c54771e5\",\n                    \"path\": \"<Gamepad>/leftStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"a9817391-7297-4bc1-8786-90b7f5577378\",\n                    \"path\": \"<Gamepad>/dpad/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"93446268-5ce5-4015-8522-72e24a36cdac\",\n                    \"path\": \"<Gamepad>/leftStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"232121c4-680c-422c-9f0e-62f4882496e9\",\n                    \"path\": \"<Gamepad>/dpad/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Joystick\",\n                    \"id\": \"6d81b070-026c-438e-81be-c7bf608ef1eb\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"StickDeadzone\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"bf19dd3c-d56a-4900-920d-a31d997289a7\",\n                    \"path\": \"<Joystick>/stick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"8ef223ac-4138-4334-8530-6a72bae285fe\",\n                    \"path\": \"<Joystick>/stick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"0650086a-850d-43b1-b8a1-8651b445ee9d\",\n                    \"path\": \"<Joystick>/stick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"b11f5c58-dde8-4b16-b856-f60ad6e65a89\",\n                    \"path\": \"<Joystick>/stick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Keyboard\",\n                    \"id\": \"edb90daf-c1b0-4288-8f15-78fffbee7c58\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"32de1136-e1ae-42dd-a3c0-1c9ca95644e7\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"25f0db83-5534-4f02-b158-26c7b9a3e8cc\",\n                    \"path\": \"<Keyboard>/upArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"55c4645c-dd37-4e17-9a4c-222676f42e27\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"23516e9b-4041-4a09-a8dd-ebf2e15b2dae\",\n                    \"path\": \"<Keyboard>/downArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"d679c72f-c3bf-4802-9440-b31c2ee16846\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"e5790090-54d5-4eaf-a0bd-74082c7788ae\",\n                    \"path\": \"<Keyboard>/leftArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"ca899b3c-6e55-4e49-9671-b1dcc16fcf1a\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"ffb327ad-5e94-4875-be0d-cd19121b45ac\",\n                    \"path\": \"<Keyboard>/rightArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Navigate\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"95a5981f-e742-48fd-9ef7-d81cb44e5e18\",\n                    \"path\": \"*/{Submit}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad;Touch;Joystick;XR\",\n                    \"action\": \"Submit\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"98f57c80-e86f-45a2-b564-eb579858fb66\",\n                    \"path\": \"*/{Cancel}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad;Touch;Joystick;XR\",\n                    \"action\": \"Cancel\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"66eb90c1-63cf-46e2-96ce-4cf759d7a745\",\n                    \"path\": \"<Mouse>/position\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Point\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c7759c6c-1ce2-4f59-91b4-3c21adffa8e8\",\n                    \"path\": \"<Pen>/position\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Point\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"cddf3aa3-e1c4-4428-af70-c606262441c4\",\n                    \"path\": \"<Touchscreen>/touch*/position\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Touch\",\n                    \"action\": \"Point\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"cae9ea49-0883-4552-9a83-4b09b3242e05\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Click\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"88a357f4-b3df-4a2b-a556-8521e61c593f\",\n                    \"path\": \"<Pen>/tip\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Click\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"a80d9e90-0d00-4d48-9c21-6e708c65c3f3\",\n                    \"path\": \"<Touchscreen>/touch*/press\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Touch\",\n                    \"action\": \"Click\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3624a465-840c-431e-be31-2bcc56fcb5ac\",\n                    \"path\": \"<XRController>/trigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"Click\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8a8c210c-640d-4949-94af-1f1a99c983c8\",\n                    \"path\": \"<Mouse>/scroll\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"ScrollWheel\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c5a9f6ce-2114-489b-b1d2-74377b3aad45\",\n                    \"path\": \"<Mouse>/middleButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"MiddleClick\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"681eaf08-383c-4439-8412-b688d04a27a7\",\n                    \"path\": \"<Mouse>/rightButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"RightClick\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"ae2b242c-af73-466c-8a8b-ed8a66078c19\",\n                    \"path\": \"<XRController>/devicePosition\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"TrackedDevicePosition\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"60e8747e-3d88-4100-a47c-22a1c32d1b54\",\n                    \"path\": \"<XRController>/deviceRotation\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"TrackedDeviceOrientation\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Station\",\n            \"id\": \"ad672516-5c16-40e9-a083-4040afd61254\",\n            \"actions\": [\n                {\n                    \"name\": \"StartWave\",\n                    \"type\": \"Button\",\n                    \"id\": \"4e4f2126-4550-43c8-82e0-bb47e0a7615e\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Cancel\",\n                    \"type\": \"Button\",\n                    \"id\": \"271d866b-216b-4625-8bd5-9c76235bc7bb\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"CycleLeft\",\n                    \"type\": \"Button\",\n                    \"id\": \"b33e4107-8c66-40f3-97aa-6a897d9685f4\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"CycleRight\",\n                    \"type\": \"Button\",\n                    \"id\": \"36a6eba1-78fa-40ad-970b-ad1c3cfa190e\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Submit\",\n                    \"type\": \"Button\",\n                    \"id\": \"98c3abfe-3667-4ba1-9be6-7184a41c45c4\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"SwapLeft\",\n                    \"type\": \"Button\",\n                    \"id\": \"3802c195-bc96-4980-8886-4e24d74a3895\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"SwapRight\",\n                    \"type\": \"Button\",\n                    \"id\": \"c7c7e92e-1d8b-4594-9fea-6ea945cde606\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"AddCart\",\n                    \"type\": \"Button\",\n                    \"id\": \"71b9bb0a-34ab-430e-a2ff-17e06b30f6c2\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"RepairCarts\",\n                    \"type\": \"Button\",\n                    \"id\": \"ef8fbb44-931d-4588-94b3-988318bcab1d\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"RerollShop\",\n                    \"type\": \"Button\",\n                    \"id\": \"1a12eae9-7546-4113-a5d3-04373e91bb83\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Merge\",\n                    \"type\": \"Button\",\n                    \"id\": \"9af8e118-2c91-4bd9-9ebf-b463fb3b09ee\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"c6dbcd45-54a3-4638-95b7-31df20146a8a\",\n                    \"path\": \"<Keyboard>/space\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"StartWave\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"9fbfa9dd-c573-4f12-94fe-b0b393085ac4\",\n                    \"path\": \"<Gamepad>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"StartWave\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"969441b4-6a77-4377-b9e8-0df476fb9720\",\n                    \"path\": \"<Mouse>/rightButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Cancel\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"5cfab71d-5df6-451d-9d92-b486f7443cf1\",\n                    \"path\": \"<Gamepad>/buttonEast\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"Cancel\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"12745c15-6cd9-4fd1-994e-eac8f08e357c\",\n                    \"path\": \"<Gamepad>/leftShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"CycleLeft\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3d892dc8-98d7-425f-9269-cf15aab20562\",\n                    \"path\": \"<Gamepad>/rightShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"CycleRight\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"18cb4246-2a02-4ac4-9bc4-c64f5a2e3c65\",\n                    \"path\": \"<Gamepad>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"Submit\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"7097693b-355f-46db-a46a-c182f9db02cf\",\n                    \"path\": \"<Gamepad>/leftTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"SwapLeft\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"b327a35e-0816-448c-bc5f-c765c0d030de\",\n                    \"path\": \"<Gamepad>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"SwapRight\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1208fdb3-94f7-45e9-8338-fd23026b6036\",\n                    \"path\": \"<Gamepad>/buttonNorth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"AddCart\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"45def047-16f0-4963-b9cf-7d4303443b00\",\n                    \"path\": \"<Gamepad>/buttonWest\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"RepairCarts\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"2612d208-e19c-4d87-a257-a82e1dc41e7c\",\n                    \"path\": \"<Gamepad>/leftStickPress\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"RerollShop\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"f819765b-6ced-4c7a-b304-c08bdfcd0b3c\",\n                    \"path\": \"<Gamepad>/rightStickPress\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"Merge\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Drone\",\n            \"id\": \"cc9039a8-5321-4072-969f-c0befc837a8a\",\n            \"actions\": [\n                {\n                    \"name\": \"Move\",\n                    \"type\": \"Value\",\n                    \"id\": \"72e50412-6b22-4f4e-bc98-8b69f29cf24b\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Dash\",\n                    \"type\": \"Button\",\n                    \"id\": \"881cce44-d05c-4f9c-86b9-819996cdc1ef\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"2D Vector\",\n                    \"id\": \"e74721dc-ea5f-4a2f-b1f1-74031beb1545\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"a610340f-ccde-46da-bd87-14b933971386\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"9665839d-1cfe-4403-aeac-f80b8a60a928\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"b3438d75-ca65-4c1e-96ca-75b8afcb5dd1\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"809bfb65-ece9-4c3b-8dae-e68f0bb47250\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1ced0875-3be5-4f43-b262-774a694709dd\",\n                    \"path\": \"<Gamepad>/leftStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8896ab19-a485-437d-a2d4-ae19878e21ea\",\n                    \"path\": \"<Keyboard>/space\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Dash\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c0d6b334-9d36-4d46-9150-f33608c356ec\",\n                    \"path\": \"<Gamepad>/buttonNorth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"Dash\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Global\",\n            \"id\": \"97b0948f-4c07-4030-b9d8-3b6db0dde6cb\",\n            \"actions\": [\n                {\n                    \"name\": \"Pause\",\n                    \"type\": \"Button\",\n                    \"id\": \"038902e2-cfd2-47b8-9e91-c2036706f409\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Pointer Position\",\n                    \"type\": \"Value\",\n                    \"id\": \"3f6e05c5-9376-45a0-8e86-6b04dc600954\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Pointer Press\",\n                    \"type\": \"Button\",\n                    \"id\": \"67b0ec7c-84e6-4301-9ca8-90c36af067a1\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Pointer Scroll\",\n                    \"type\": \"Value\",\n                    \"id\": \"6e9de83a-a68a-4ab2-86e9-b2d8e2f7a0ca\",\n                    \"expectedControlType\": \"Delta\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"CameraMove\",\n                    \"type\": \"Value\",\n                    \"id\": \"8414970e-6dea-466a-9f11-34a8d8f8692a\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"RefundUpgrades\",\n                    \"type\": \"Button\",\n                    \"id\": \"1afc6ebb-ae2f-427c-918a-6c2c941975dd\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"88080e19-fbf5-4d6f-8157-6f3c2fa027ed\",\n                    \"path\": \"<Keyboard>/escape\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Pause\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"57c673f3-69de-4b8f-b7b7-5cbd4f3e4457\",\n                    \"path\": \"<Gamepad>/start\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"Pause\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1a3f3c8e-437f-45c0-83ea-1dfe151eab58\",\n                    \"path\": \"<Pointer>/position\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Pointer Position\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c00a99d0-2a69-4130-8fef-3a5b2f29fcbb\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";KBM\",\n                    \"action\": \"Pointer Press\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"b92c6c89-a0ce-4f4e-83a7-62b2aef92028\",\n                    \"path\": \"<Mouse>/scroll\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"KBM\",\n                    \"action\": \"Pointer Scroll\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"2c37cdf6-8d03-4b20-ae84-35de3927979c\",\n                    \"path\": \"<Gamepad>/leftStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"CameraMove\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"5fdf0ab6-9515-472c-b2a0-30d75ea1ea95\",\n                    \"path\": \"<Gamepad>/buttonNorth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"RefundUpgrades\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"LocoSelector\",\n            \"id\": \"95957809-1819-4136-90f7-a236943d929f\",\n            \"actions\": [\n                {\n                    \"name\": \"CycleLeft\",\n                    \"type\": \"Button\",\n                    \"id\": \"844a5e86-eb4c-4009-8910-3222583b537a\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"CycleRight\",\n                    \"type\": \"Button\",\n                    \"id\": \"965e9e2b-4072-4f65-97a7-c6eb1b5a2d8f\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"f8b14136-96c5-4fde-8508-b145ab9dc081\",\n                    \"path\": \"<Gamepad>/leftShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"CycleLeft\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"406b8c0b-d7f3-4495-84ca-aec60e733869\",\n                    \"path\": \"<Gamepad>/rightShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"CycleRight\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Cutscene\",\n            \"id\": \"30db616f-59d9-4147-9219-aec2ed5c8bb0\",\n            \"actions\": [\n                {\n                    \"name\": \"Skip\",\n                    \"type\": \"Button\",\n                    \"id\": \"4b4a9795-1ab7-4113-9d96-cb256c9bfe6b\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"0f537820-c46f-4d9f-912d-8a5546b939b3\",\n                    \"path\": \"<Keyboard>/space\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";KBM\",\n                    \"action\": \"Skip\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"5285b8fd-c728-49f6-a316-be16f678f2d9\",\n                    \"path\": \"<Gamepad>/buttonNorth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Controller\",\n                    \"action\": \"Skip\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        }\n    ],\n    \"controlSchemes\": [\n        {\n            \"name\": \"KBM\",\n            \"bindingGroup\": \"KBM\",\n            \"devices\": [\n                {\n                    \"devicePath\": \"<Keyboard>\",\n                    \"isOptional\": false,\n                    \"isOR\": false\n                },\n                {\n                    \"devicePath\": \"<Mouse>\",\n                    \"isOptional\": false,\n                    \"isOR\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Controller\",\n            \"bindingGroup\": \"Controller\",\n            \"devices\": [\n                {\n                    \"devicePath\": \"<Gamepad>\",\n                    \"isOptional\": false,\n                    \"isOR\": false\n                }\n            ]\n        }\n    ]\n}");
			m_Train = asset.FindActionMap("Train", throwIfNotFound: true);
			m_Train_MoveTrain = m_Train.FindAction("MoveTrain", throwIfNotFound: true);
			m_Train_JumpDown = m_Train.FindAction("JumpDown", throwIfNotFound: true);
			m_Train_JumpUp = m_Train.FindAction("JumpUp", throwIfNotFound: true);
			m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
			m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
			m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
			m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
			m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
			m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
			m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
			m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
			m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
			m_UI_TrackedDevicePosition = m_UI.FindAction("TrackedDevicePosition", throwIfNotFound: true);
			m_UI_TrackedDeviceOrientation = m_UI.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
			m_Station = asset.FindActionMap("Station", throwIfNotFound: true);
			m_Station_StartWave = m_Station.FindAction("StartWave", throwIfNotFound: true);
			m_Station_Cancel = m_Station.FindAction("Cancel", throwIfNotFound: true);
			m_Station_CycleLeft = m_Station.FindAction("CycleLeft", throwIfNotFound: true);
			m_Station_CycleRight = m_Station.FindAction("CycleRight", throwIfNotFound: true);
			m_Station_Submit = m_Station.FindAction("Submit", throwIfNotFound: true);
			m_Station_SwapLeft = m_Station.FindAction("SwapLeft", throwIfNotFound: true);
			m_Station_SwapRight = m_Station.FindAction("SwapRight", throwIfNotFound: true);
			m_Station_AddCart = m_Station.FindAction("AddCart", throwIfNotFound: true);
			m_Station_RepairCarts = m_Station.FindAction("RepairCarts", throwIfNotFound: true);
			m_Station_RerollShop = m_Station.FindAction("RerollShop", throwIfNotFound: true);
			m_Station_Merge = m_Station.FindAction("Merge", throwIfNotFound: true);
			m_Drone = asset.FindActionMap("Drone", throwIfNotFound: true);
			m_Drone_Move = m_Drone.FindAction("Move", throwIfNotFound: true);
			m_Drone_Dash = m_Drone.FindAction("Dash", throwIfNotFound: true);
			m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
			m_Global_Pause = m_Global.FindAction("Pause", throwIfNotFound: true);
			m_Global_PointerPosition = m_Global.FindAction("Pointer Position", throwIfNotFound: true);
			m_Global_PointerPress = m_Global.FindAction("Pointer Press", throwIfNotFound: true);
			m_Global_PointerScroll = m_Global.FindAction("Pointer Scroll", throwIfNotFound: true);
			m_Global_CameraMove = m_Global.FindAction("CameraMove", throwIfNotFound: true);
			m_Global_RefundUpgrades = m_Global.FindAction("RefundUpgrades", throwIfNotFound: true);
			m_LocoSelector = asset.FindActionMap("LocoSelector", throwIfNotFound: true);
			m_LocoSelector_CycleLeft = m_LocoSelector.FindAction("CycleLeft", throwIfNotFound: true);
			m_LocoSelector_CycleRight = m_LocoSelector.FindAction("CycleRight", throwIfNotFound: true);
			m_Cutscene = asset.FindActionMap("Cutscene", throwIfNotFound: true);
			m_Cutscene_Skip = m_Cutscene.FindAction("Skip", throwIfNotFound: true);
		}

		~InputActions()
		{
		}

		public void Dispose()
		{
			UnityEngine.Object.Destroy(asset);
		}

		public bool Contains(InputAction action)
		{
			return asset.Contains(action);
		}

		public IEnumerator<InputAction> GetEnumerator()
		{
			return asset.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Enable()
		{
			asset.Enable();
		}

		public void Disable()
		{
			asset.Disable();
		}

		public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
		{
			return asset.FindAction(actionNameOrId, throwIfNotFound);
		}

		public int FindBinding(InputBinding bindingMask, out InputAction action)
		{
			return asset.FindBinding(bindingMask, out action);
		}
	}
}
