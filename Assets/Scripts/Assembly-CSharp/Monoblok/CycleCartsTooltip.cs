using Monoblok.Input;
using UnityEngine;

namespace Monoblok
{
	public class CycleCartsTooltip : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

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
			Debug.Log("Changing");
			bool flag = type == InputManager.InputDeviceType.Gamepad;
			canvasGroup.alpha = (flag ? 1 : 0);
		}
	}
}
