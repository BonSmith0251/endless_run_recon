using UnityEngine;
using UnityEngine.Events;

namespace Monoblok.Input
{
	public class InputDeviceChangedCallback : MonoBehaviour
	{
		[SerializeField]
		private bool notifyOnEnable = true;

		[SerializeField]
		private UnityEvent gamepadEvent;

		[SerializeField]
		private UnityEvent kbmEvent;

		private void OnEnable()
		{
			InputManager.OnInputDeviceChanged += HandleDeviceChanged;
			if (notifyOnEnable)
			{
				HandleDeviceChanged(InputManager.CurrentDevice);
			}
		}

		private void OnDisable()
		{
			InputManager.OnInputDeviceChanged -= HandleDeviceChanged;
		}

		private void HandleDeviceChanged(InputManager.InputDeviceType type)
		{
			switch (type)
			{
			case InputManager.InputDeviceType.KBM:
				kbmEvent?.Invoke();
				break;
			case InputManager.InputDeviceType.Gamepad:
				gamepadEvent?.Invoke();
				break;
			case InputManager.InputDeviceType.Unsupported:
				break;
			}
		}
	}
}
