using UnityEngine;
using UnityEngine.InputSystem;

namespace Monoblok.Upgrades
{
	public abstract class ShopActionConfig : ScriptableObject
	{
		[SerializeField]
		private CurrencyDefinition currency;

		[SerializeField]
		private InputActionReference hotkey;

		[SerializeField]
		private ShopActionMetaData metaData;

		public InputActionReference Hotkey => hotkey;

		public ShopActionMetaData MetaData => metaData;

		protected Price ConstructPrice(int amount)
		{
			Price result = default(Price);
			result.Currency = currency;
			result.Amount = amount;
			return result;
		}

		public abstract ShopAction GetAction(ShopActionContext ctx);
	}
}
