using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Monoblok.Input
{
	public class InputActions_UI : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
	{
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

		public InputActions_UI()
		{
			asset = InputActionAsset.FromJson("{\r\n    \"version\": 1,\r\n    \"name\": \"Input Actions_UI\",\r\n    \"maps\": [],\r\n    \"controlSchemes\": [\r\n        {\r\n            \"name\": \"KBM\",\r\n            \"bindingGroup\": \"KBM\",\r\n            \"devices\": [\r\n                {\r\n                    \"devicePath\": \"<Keyboard>\",\r\n                    \"isOptional\": false,\r\n                    \"isOR\": false\r\n                },\r\n                {\r\n                    \"devicePath\": \"<Mouse>\",\r\n                    \"isOptional\": false,\r\n                    \"isOR\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"Controller\",\r\n            \"bindingGroup\": \"Controller\",\r\n            \"devices\": [\r\n                {\r\n                    \"devicePath\": \"<Gamepad>\",\r\n                    \"isOptional\": false,\r\n                    \"isOR\": false\r\n                }\r\n            ]\r\n        }\r\n    ]\r\n}");
		}

		~InputActions_UI()
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
