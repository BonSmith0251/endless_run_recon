using Monoblok.Input;
using UnityEngine;

namespace Monoblok
{
	public class DragCameraTooltip : MonoBehaviour
	{
		[SerializeField]
		private GameObject KBM;

		[SerializeField]
		private GameObject controller;

		private void OnEnable()
		{
			InputManager.OnInputDeviceChanged += HandleInputDeviceChanged;
			HandleInputDeviceChanged(InputManager.CurrentDevice);
		}

		private void OnDisable()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
		}

		private void HandleInputDeviceChanged(InputManager.InputDeviceType type)
		{
			switch (type)
			{
			case InputManager.InputDeviceType.KBM:
				KBM.SetActive(value: true);
				controller.SetActive(value: false);
				break;
			case InputManager.InputDeviceType.Gamepad:
				KBM.SetActive(value: false);
				controller.SetActive(value: true);
				break;
			case InputManager.InputDeviceType.Unsupported:
				break;
			}
		}
	}
}
