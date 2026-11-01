using Monoblok.Train;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	public class AddModuleActionView : ShopActionView<AddModuleAction>
	{
		[SerializeField]
		private Button button;

		[SerializeField]
		private Image moduleImage;

		[SerializeField]
		private TMP_Text moduleNameText;

		[SerializeField]
		private TMP_Text moduleDescriptionText;

		[SerializeField]
		private LockButton lockButton;

		[SerializeField]
		private GameObject soldObject;

		public GameObject MainContainer => button.gameObject;

		protected override void OnBind(AddModuleAction action)
		{
			ModuleDefinition moduleDefinition = action.ModuleDefinition;
			moduleImage.sprite = moduleDefinition.Sprite;
			moduleNameText.text = moduleDefinition.DisplayName;
			moduleDescriptionText.text = moduleDefinition.Description;
			action.IsResolved.OnValueChanged += OnIsResolvedChanged;
			OnIsResolvedChanged(action.IsResolved.Value);
			button.onClick.AddListener(HandleClick);
			if ((bool)lockButton)
			{
				lockButton.SetState(action.IsLocked);
				lockButton.OnClicked += HandleLockClicked;
			}
			button.interactable = true;
		}

		private void HandleLockClicked()
		{
			base.Action.ToggleLock();
			lockButton.SetState(base.Action.IsLocked);
		}

		private void OnIsResolvedChanged(bool isResolved)
		{
			soldObject.SetActive(isResolved);
			lockButton.gameObject.SetActive(!isResolved);
			if (isResolved)
			{
				button.interactable = false;
				button.onClick.RemoveAllListeners();
				if ((bool)lockButton)
				{
					lockButton.OnClicked -= HandleLockClicked;
				}
				base.Action.IsResolved.OnValueChanged -= OnIsResolvedChanged;
			}
		}

		private void HandleClick()
		{
			RequestResolve();
		}

		protected override void OnUnbind()
		{
			button.onClick.RemoveAllListeners();
			if ((bool)lockButton)
			{
				lockButton.OnClicked -= HandleLockClicked;
			}
			if (base.Action != null)
			{
				base.Action.IsResolved.OnValueChanged -= OnIsResolvedChanged;
			}
		}

		protected override void OnEligibilityChanged(EligibilityState state)
		{
			base.OnEligibilityChanged(state);
		}
	}
}
