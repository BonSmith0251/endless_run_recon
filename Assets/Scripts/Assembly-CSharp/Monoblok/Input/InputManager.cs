using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Monoblok.Input
{
	public class InputManager : MonoBehaviour
	{
		public enum InputDeviceType
		{
			Unsupported,
			KBM,
			Gamepad
		}

		[SerializeField]
		private InputReader inputReader;

		private static InputDeviceType currentDevice;

		private GameObject _lastSelected;

		public static InputDeviceType CurrentDevice => currentDevice;

		public static event Action<InputDeviceType> OnInputDeviceChanged;

		public static event Action<GameObject, GameObject> OnSelectionChanged;

		public void Initialize()
		{
			SetDevice(InputSystem.devices.FirstOrDefault());
			inputReader.Initialize();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void ResetCurrentDevice()
		{
			currentDevice = InputDeviceType.Unsupported;
		}

		private void OnEnable()
		{
			foreach (InputActionMap actionMap in InputSystem.actions.actionMaps)
			{
				foreach (InputAction action in actionMap.actions)
				{
					action.performed += OnAnyPerformed;
				}
			}
		}

		private void OnDisable()
		{
			foreach (InputActionMap actionMap in InputSystem.actions.actionMaps)
			{
				foreach (InputAction action in actionMap.actions)
				{
					action.performed -= OnAnyPerformed;
				}
			}
		}

		private void Update()
		{
			GameObject gameObject = EventSystem.current?.currentSelectedGameObject;
			if (!(gameObject == _lastSelected))
			{
				GameObject lastSelected = _lastSelected;
				_lastSelected = gameObject;
				InputManager.OnSelectionChanged?.Invoke(lastSelected, gameObject);
			}
		}

		private void OnAnyPerformed(InputAction.CallbackContext context)
		{
			InputDevice device = context.control.device;
			SetDevice(device);
			Cursor.lockState = ((!(Cursor.visible = currentDevice != InputDeviceType.Gamepad)) ? CursorLockMode.Locked : CursorLockMode.None);
		}

		private void SetDevice(InputDevice device)
		{
			InputDeviceType deviceType = GetDeviceType(device);
			if (currentDevice != deviceType)
			{
				currentDevice = deviceType;
				InputManager.OnInputDeviceChanged?.Invoke(currentDevice);
			}
		}

		private InputDeviceType GetDeviceType(InputDevice device)
		{
			if (!(device is Gamepad))
			{
				if (!(device is Keyboard))
				{
					if (device is Mouse)
					{
						return InputDeviceType.KBM;
					}
					return InputDeviceType.Unsupported;
				}
				return InputDeviceType.KBM;
			}
			return InputDeviceType.Gamepad;
		}
	}
}
