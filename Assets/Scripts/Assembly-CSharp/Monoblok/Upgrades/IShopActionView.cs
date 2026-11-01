using System;

namespace Monoblok.Upgrades
{
	public interface IShopActionView
	{
		Type ActionType { get; }

		event Action<ShopAction> OnActionRequested;

		event Action<ShopAction> OnPointerEntered;

		event Action<ShopAction> OnPointerExited;

		void Bind(ShopAction action);

		void Unbind();
	}
}
