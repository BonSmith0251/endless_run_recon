using System;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Core.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class YesNoModal : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private Button yesButton;

		[SerializeField]
		private Button noButton;

		private Action onYes;

		private Action onNo;

		private void OnValidate()
		{
			if (!(canvasGroup != null))
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
		}

		private void OnEnable()
		{
			yesButton.onClick.AddListener(HandleYesClicked);
			noButton.onClick.AddListener(HandleNoClicked);
		}

		private void OnDisable()
		{
			yesButton.onClick.RemoveAllListeners();
			noButton.onClick.RemoveAllListeners();
		}

		private void HandleYesClicked()
		{
			onYes?.Invoke();
			Hide();
		}

		private void HandleNoClicked()
		{
			onNo?.Invoke();
			Hide();
		}

		public void Show(Action onYes, Action onNo)
		{
			this.onYes = onYes;
			this.onNo = onNo;
			base.gameObject.SetActive(value: true);
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.interactable = true;
		}

		private void Hide()
		{
			onYes = null;
			onNo = null;
			base.gameObject.SetActive(value: false);
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
		}
	}
}
