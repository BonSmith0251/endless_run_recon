using System;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class ThanksForPlayingScreen : MonoBehaviour
	{
		[SerializeField]
		private Canvas canvas;

		[SerializeField]
		private Button continueButton;

		public event Action OnContinueClicked;

		private void OnEnable()
		{
			continueButton.onClick.AddListener(HandleContinueClicked);
		}

		private void OnDisable()
		{
			continueButton.onClick.RemoveListener(HandleContinueClicked);
		}

		private void HandleContinueClicked()
		{
			this.OnContinueClicked?.Invoke();
		}

		public void Show()
		{
			canvas.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			canvas.gameObject.SetActive(value: false);
		}
	}
}
