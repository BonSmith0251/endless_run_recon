using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Monoblok.Input
{
	public class InputBindingView : MonoBehaviour
	{
		public enum InputBindingViewState
		{
			Disabled,
			Default,
			WaitingForInput
		}

		[SerializeField]
		private Button button;

		[SerializeField]
		private TMP_Text text;

		[SerializeField]
		private InputActionReference action;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private bool composite;

		[SerializeField]
		[ShowIf("composite")]
		private int index;

		[SerializeField]
		private Color defaultColor;

		[SerializeField]
		private Color waitingForinputColor;

		public InputActionReference Action => action;

		public int Index
		{
			get
			{
				if (!composite)
				{
					return 0;
				}
				return index;
			}
		}

		public event Action<InputBindingView> OnClicked;

		private void OnValidate()
		{
			if (button == null)
			{
				button = GetComponentInChildren<Button>();
			}
			if (text == null && button != null)
			{
				text = button.GetComponentInChildren<TMP_Text>();
			}
			if (!(text == null) && !(action == null))
			{
				text.text = action.action.GetBindingDisplayString(composite ? index : 0);
			}
		}

		private void Start()
		{
			SetState(InputBindingViewState.Default);
		}

		private void OnEnable()
		{
			button.onClick.AddListener(HandleClicked);
		}

		private void OnDisable()
		{
			button.onClick.RemoveAllListeners();
		}

		private void HandleClicked()
		{
			this.OnClicked?.Invoke(this);
		}

		public void SetState(InputBindingViewState state)
		{
			switch (state)
			{
			case InputBindingViewState.Disabled:
				button.interactable = false;
				break;
			case InputBindingViewState.Default:
			{
				button.interactable = true;
				InputAction inputAction = inputReader.Asset.FindAction(action.action.id);
				text.text = inputAction.GetBindingDisplayString(composite ? index : 0);
				text.color = defaultColor;
				break;
			}
			case InputBindingViewState.WaitingForInput:
				button.interactable = false;
				text.text = "Press any key...";
				text.color = waitingForinputColor;
				break;
			}
		}

		public void Focus()
		{
			EventSystem.current.SetSelectedGameObject(button.gameObject);
		}
	}
}
