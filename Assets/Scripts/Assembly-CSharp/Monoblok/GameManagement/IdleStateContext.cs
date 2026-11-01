using Monoblok.Inventory;
using Monoblok.Progression;
using Monoblok.Train;

namespace Monoblok.GameManagement
{
	public class IdleStateContext
	{
		public readonly Monoblok.Train.Train train;

		public readonly StationManager stationManager;

		public readonly WinLoseConditionManager winLoseConditionManager;

		public readonly ProgressionManager progressionManager;

		public readonly InventoryManager inventoryManager;

		public readonly CartMergeManager mergeManager;

		public readonly AbilityManager abilityManager;

		public IdleStateContext(Monoblok.Train.Train train, StationManager stationManager, WinLoseConditionManager winLoseConditionManager, ProgressionManager progressionManager, InventoryManager inventoryManager, CartMergeManager mergeManager, AbilityManager abilityManager)
		{
			this.train = train;
			this.stationManager = stationManager;
			this.winLoseConditionManager = winLoseConditionManager;
			this.progressionManager = progressionManager;
			this.inventoryManager = inventoryManager;
			this.mergeManager = mergeManager;
			this.abilityManager = abilityManager;
		}
	}
}
