using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monoblok.Input
{
	public class InputDeviceChangeSelector : MonoBehaviour
	{
		public enum SelectorType
		{
			Explicit,
			Automatic
		}

		[SerializeField]
		private SelectorType type;

		[SerializeField]
		[ShowIf("type", SelectorType.Explicit)]
		private Selectable selectable;

		public void OnEnable()
		{
			InputManager.OnInputDeviceChanged += HandleInputDeviceChanged;
			HandleInputDeviceChanged(InputManager.CurrentDevice);
		}

		private void OnDestroy()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
		}

		private void OnDisable()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
		}

		private void HandleInputDeviceChanged(InputManager.InputDeviceType type)
		{
			Debug.Log("Device changed", base.gameObject);
			if (type == InputManager.InputDeviceType.Gamepad)
			{
				MakeSelection();
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}

		private void MakeSelection()
		{
			GameObject selectedGameObject = null;
			switch (type)
			{
			case SelectorType.Explicit:
				selectedGameObject = selectable.gameObject;
				break;
			case SelectorType.Automatic:
				selectedGameObject = GetComponentsInChildren<Selectable>().FirstOrDefault((Selectable s) => s.interactable).gameObject;
				break;
			}
			EventSystem.current.SetSelectedGameObject(selectedGameObject);
		}
	}
}
