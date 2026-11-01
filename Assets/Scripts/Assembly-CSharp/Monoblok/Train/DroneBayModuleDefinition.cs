using Monoblok.DroneSystem;
using UnityEngine;
using UnityEngine.Localization;
using ZLinq;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Modules/Drone bay")]
	public class DroneBayModuleDefinition : ModuleDefinition
	{
		[SerializeField]
		[HideInInspector]
		private DroneBayModule modulePrefab;

		[SerializeField]
		private DroneData[] droneDatas;

		[SerializeField]
		private LocalizedString lastDroneError;

		public override ModuleBehavior CreateBehavior(Transform parent)
		{
			DroneBayModule droneBayModule = Object.Instantiate(modulePrefab, parent);
			droneBayModule.Initialize(base.Sprite, droneDatas);
			return droneBayModule;
		}

		public override bool CanBeSold(TrainCart cart, out string errorMsg)
		{
			errorMsg = string.Empty;
			if ((from s in cart.Train.Carts.AsValueEnumerable().SelectMany((TrainCart c) => c.ModuleManager.Slots)
				where !s.IsFree
				where s.ModuleData.Definition is DroneBayModuleDefinition
				select s).Count() <= 1)
			{
				errorMsg = lastDroneError.GetLocalizedString();
				return false;
			}
			return true;
		}
	}
}
