using Monoblok.Input;
using UnityEngine;

namespace Monoblok
{
	public class OrderCartsTooltip : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private GameObject gamepad;

		[SerializeField]
		private GameObject mouse;

		private void OnEnable()
		{
			InputManager.OnInputDeviceChanged += HandleInputDeviceChanged;
			HandleInputDeviceChanged(InputManager.CurrentDevice);
		}

		private void OnDisable()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
		}

		private void OnDestroy()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
		}

		private void HandleInputDeviceChanged(InputManager.InputDeviceType type)
		{
			canvasGroup.alpha = 1f;
			switch (type)
			{
			case InputManager.InputDeviceType.Unsupported:
				canvasGroup.alpha = 0f;
				break;
			case InputManager.InputDeviceType.KBM:
				mouse.SetActive(value: true);
				gamepad.SetActive(value: false);
				break;
			case InputManager.InputDeviceType.Gamepad:
				mouse.SetActive(value: false);
				gamepad.SetActive(value: true);
				break;
			}
		}
	}
}
