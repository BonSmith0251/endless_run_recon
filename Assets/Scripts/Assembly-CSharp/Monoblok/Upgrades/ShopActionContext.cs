using Monoblok.Inventory;
using Monoblok.Train;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	public class ShopActionContext
	{
		public readonly Shop shop;

		public readonly PlayerWallet wallet;

		public readonly CartSelector cartSelector;

		public readonly CartMergeManager mergeManager;

		public readonly Monoblok.Train.Train train;

		public readonly AbilityManager abilityManager;

		public readonly LocalizedString insufficientFundsMessage;

		public ShopActionContext(Shop shop, CartSelector cartSelector, CartMergeManager mergeManager, PlayerWallet wallet, Monoblok.Train.Train train, AbilityManager abilityManager, LocalizedString insufficientFundsMessage)
		{
			this.shop = shop;
			this.cartSelector = cartSelector;
			this.mergeManager = mergeManager;
			this.wallet = wallet;
			this.train = train;
			this.abilityManager = abilityManager;
			this.insufficientFundsMessage = insufficientFundsMessage;
		}
	}
}
