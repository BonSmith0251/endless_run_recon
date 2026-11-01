using System;
using Monoblok.StateMachine;
using UnityEngine.UI;

namespace Monoblok.GameManagement
{
	public class ButtonClickedCondition : ICondition
	{
		private readonly Button button;

		public event Action OnMet;

		public ButtonClickedCondition(Button button)
		{
			this.button = button;
		}

		public void Initialize()
		{
			button.onClick.AddListener(HandleButtonClicked);
		}

		private void HandleButtonClicked()
		{
			this.OnMet?.Invoke();
		}

		public void Dispose()
		{
			button.onClick.RemoveListener(HandleButtonClicked);
		}
	}
}
