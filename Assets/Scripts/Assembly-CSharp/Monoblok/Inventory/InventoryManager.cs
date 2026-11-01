using UnityEngine;

namespace Monoblok.Inventory
{
	[DefaultExecutionOrder(-100)]
	public class InventoryManager : MonoBehaviour
	{
		[SerializeField]
		private CurrencyDefinition[] currencies;

		[SerializeField]
		private PlayerWallet wallet;

		public void Initialize()
		{
			wallet.Initialize(currencies);
		}
	}
}
