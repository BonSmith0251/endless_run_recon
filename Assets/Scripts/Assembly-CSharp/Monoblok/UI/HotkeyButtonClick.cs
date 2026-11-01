using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Monoblok.UI
{
	public class HotkeyButtonClick : MonoBehaviour
	{
		[SerializeField]
		private Button button;

		[SerializeField]
		private InputActionReference actionRef;

		private void OnEnable()
		{
			actionRef.action.performed += HandlePerformed;
		}

		private void OnDisable()
		{
			actionRef.action.performed -= HandlePerformed;
		}

		private void HandlePerformed(InputAction.CallbackContext context)
		{
			if (button.interactable)
			{
				button.onClick.Invoke();
			}
		}
	}
}
