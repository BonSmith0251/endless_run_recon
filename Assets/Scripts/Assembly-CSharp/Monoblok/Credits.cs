using System;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class Credits : MonoBehaviour
	{
		[SerializeField]
		private Button backButton;

		public event Action OnBackCLicked;

		private void OnEnable()
		{
			backButton.onClick.AddListener(HandleBackClicked);
		}

		private void OnDisable()
		{
			backButton.onClick.RemoveAllListeners();
		}

		private void HandleBackClicked()
		{
			this.OnBackCLicked?.Invoke();
		}
	}
}
