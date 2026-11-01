using Monoblok.Input;
using UnityEngine;

namespace Monoblok
{
	public class CameraInputManager : MonoBehaviour
	{
		[SerializeField]
		private DragCamera dragCamera;

		[SerializeField]
		private WorldSpaceUISelector worldSpaceSelector;

		private void OnEnable()
		{
			InputManager.OnInputDeviceChanged += HandleInputDeviceChanged;
			worldSpaceSelector.OnHoverChanged += HandleHoverChanged;
			HandleInputDeviceChanged(InputManager.CurrentDevice);
		}

		private void OnDisable()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
			worldSpaceSelector.OnHoverChanged -= HandleHoverChanged;
		}

		private void HandleInputDeviceChanged(InputManager.InputDeviceType type)
		{
			bool active = type == InputManager.InputDeviceType.Gamepad;
			worldSpaceSelector.gameObject.SetActive(active);
			ICameraMoveStrategy strategy = type switch
			{
				InputManager.InputDeviceType.KBM => new MouseCameraMoveStrategy(dragCamera.TargetCamera, dragCamera.InputReader, dragCamera.DragSensitivity, dragCamera.TargetCamera.transform.position.z), 
				InputManager.InputDeviceType.Gamepad => new GamepadCameraMoveStrategy(dragCamera.InputReader, dragCamera.GamepadMoveSpeed), 
				_ => null, 
			};
			dragCamera.SetStrategy(strategy);
		}

		private void HandleHoverChanged(GameObject hovered)
		{
			if (!(hovered == null) && hovered.TryGetComponent<RectTransform>(out var component))
			{
				dragCamera.ForcePosition(component.position);
			}
		}
	}
}
