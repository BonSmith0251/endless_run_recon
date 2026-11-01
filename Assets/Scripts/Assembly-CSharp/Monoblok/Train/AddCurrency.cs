using System;
using Monoblok.Inventory;
using UnityEngine;

namespace Monoblok.Train
{
	[Serializable]
	public class AddCurrency : ITemporaryModuleAction
	{
		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private CurrencyDefinition currency;

		[SerializeField]
		private int amount;

		public void Execute(TrainCart cart)
		{
			wallet.AddCurrency(currency, amount);
		}
	}
}
