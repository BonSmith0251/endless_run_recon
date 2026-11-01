using TMPro;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class AddCartActionView : StaticButtonActionView<AddCartAction>
	{
		[SerializeField]
		private GameObject performCountContainer;

		[SerializeField]
		private TMP_Text performCountText;

		protected override void OnBind(AddCartAction action)
		{
			base.OnBind(action);
			bool flag = action.MaxPerformCount > 0;
			performCountContainer.SetActive(flag);
			if (flag)
			{
				action.PerformCount.OnValueChanged += HandlePerformCountChanged;
				HandlePerformCountChanged(action.PerformCount.Value);
			}
		}

		protected override void OnUnbind()
		{
			base.OnUnbind();
			base.Action.PerformCount.OnValueChanged -= HandlePerformCountChanged;
		}

		private void HandlePerformCountChanged(int performCount)
		{
			if (!(performCountText == null))
			{
				performCountText.text = (base.Action.MaxPerformCount - performCount).ToString();
			}
		}
	}
}
