using System.Collections.Generic;
using Monoblok.DroneSystem;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Train
{
	public class DroneBayModule : ModuleBehavior
	{
		[SerializeField]
		private PlayerDronesRuntimeValue playerDrones;

		private IEnumerable<DroneData> droneDatas;

		private List<Drone> drones = new List<Drone>();

		public void Initialize(Sprite sprite, IEnumerable<DroneData> droneDatas)
		{
			this.droneDatas = droneDatas;
			spriteRenderer.sprite = sprite;
		}

		protected override void OnModuleEnabled()
		{
			AddDrones();
		}

		protected override void OnModuleDisabled()
		{
			RemoveDrones();
		}

		private void AddDrones()
		{
			foreach (DroneData droneData in droneDatas)
			{
				playerDrones.Value.TryAddDrone(droneData, statManager, out var drone);
				drones.Add(drone);
				base.Slot.Cart.RegisterWeaponHandler(drone.WeaponHandler);
			}
		}

		private void RemoveDrones()
		{
			foreach (Drone drone in drones)
			{
				base.Slot.Cart.UnregisterWeaponHandler(drone.WeaponHandler);
				playerDrones.Value.RemoveDrone(drone);
			}
			drones.Clear();
		}
	}
}
