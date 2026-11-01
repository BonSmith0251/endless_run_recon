using System;
using Monoblok.Core.UI;
using Monoblok.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	public class UpgradeGraphManagerView : MonoBehaviour
	{
		[SerializeField]
		private UpgradeTreeView treeView;

		[SerializeField]
		private Button exitButton;

		[SerializeField]
		private Button refundButton;

		[SerializeField]
		private YesNoModal notification;

		[SerializeField]
		private DragCamera dragCamera;

		public event Action OnExitButtonClicked;

		public event Action OnRefundClicked;

		public event Action OnNotificationAccepted;

		public event Action<UpgradeNodeView> OnNodeClicked;

		private void OnEnable()
		{
			exitButton.onClick.AddListener(HandleExitClicked);
			refundButton.onClick.AddListener(HandleRefundClicked);
		}

		private void OnDisable()
		{
			exitButton.onClick.RemoveAllListeners();
			refundButton.onClick.RemoveAllListeners();
		}

		private void HandleExitClicked()
		{
			this.OnExitButtonClicked?.Invoke();
		}

		private void HandleRefundClicked()
		{
			this.OnRefundClicked?.Invoke();
		}

		public void Initialize(UpgradeTreeInstance tree)
		{
			treeView.OnNodeClicked += HandleNodeClicked;
			treeView.OnNodeSelected += HandleNodeSelected;
			Bounds bounds = treeView.Initialize(tree);
			dragCamera.SetBounds(bounds);
		}

		private void HandleNodeClicked(UpgradeNodeView view)
		{
			this.OnNodeClicked?.Invoke(view);
		}

		private void HandleNodeSelected(UpgradeNodeView view)
		{
			if (InputManager.CurrentDevice == InputManager.InputDeviceType.Gamepad)
			{
				dragCamera.ForcePosition(view.transform.position);
			}
		}

		public void ShowNotification()
		{
			exitButton.interactable = false;
			notification.Show(HandleYesClicked, HandleNoClicked);
		}

		private void HandleYesClicked()
		{
			exitButton.interactable = true;
			this.OnNotificationAccepted?.Invoke();
		}

		private void HandleNoClicked()
		{
			exitButton.interactable = true;
		}
	}
}
