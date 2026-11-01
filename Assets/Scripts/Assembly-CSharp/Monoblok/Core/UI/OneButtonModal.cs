using System;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Core.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class OneButtonModal : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private Button confirmButton;

		private Action onConfirm;

		private void OnValidate()
		{
			if (!(canvasGroup != null))
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
		}

		private void OnEnable()
		{
			confirmButton.onClick.AddListener(HandleConfirmClicked);
		}

		private void OnDisable()
		{
			confirmButton.onClick.RemoveAllListeners();
		}

		private void HandleConfirmClicked()
		{
			onConfirm?.Invoke();
			Hide();
		}

		public void Show(Action onConfirm)
		{
			this.onConfirm = onConfirm;
			base.gameObject.SetActive(value: true);
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.interactable = true;
		}

		private void Hide()
		{
			onConfirm = null;
			base.gameObject.SetActive(value: false);
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
		}
	}
}
