using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	public abstract class StaticButtonActionView<T> : ShopActionView<T> where T : ShopAction
	{
		[SerializeField]
		private Button button;

		protected override void OnBind(T action)
		{
			button.onClick.AddListener(HandleButtonClicked);
		}

		protected override void OnUnbind()
		{
			button.onClick.RemoveAllListeners();
		}

		protected override void OnEligibilityChanged(EligibilityState state)
		{
			base.OnEligibilityChanged(state);
		}

		private void HandleButtonClicked()
		{
			RequestResolve();
		}
	}
}
