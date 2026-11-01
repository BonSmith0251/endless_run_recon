using Monoblok.Input;
using UnityEngine;

namespace Monoblok
{
	public class CartOrderingManager : MonoBehaviour
	{
		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private CartSelector cartSelector;

		private Camera mainCam;

		private ICartOrderingStrategy orderingStrategy;

		private void Awake()
		{
			mainCam = Camera.main;
		}

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
			switch (type)
			{
			case InputManager.InputDeviceType.Unsupported:
				SetStrategy(null);
				break;
			case InputManager.InputDeviceType.KBM:
				SetStrategy(new MouseDragDropOrderingStrategy(inputReader, cartSelector, mainCam));
				break;
			case InputManager.InputDeviceType.Gamepad:
				SetStrategy(new CycleOrderingStrategy(cartSelector, inputReader));
				break;
			}
		}

		private void SetStrategy(ICartOrderingStrategy newStrategy)
		{
			if (orderingStrategy != newStrategy)
			{
				if (orderingStrategy != null)
				{
					orderingStrategy.Dispose();
				}
				orderingStrategy = newStrategy;
			}
		}

		private void Update()
		{
			if (orderingStrategy != null)
			{
				orderingStrategy.Update(Time.deltaTime);
			}
		}
	}
}
