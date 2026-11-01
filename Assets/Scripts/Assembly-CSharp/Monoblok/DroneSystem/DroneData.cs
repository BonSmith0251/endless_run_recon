using Monoblok.Weapon;
using UnityEngine;

namespace Monoblok.DroneSystem
{
	[CreateAssetMenu(menuName = "Game/Drone Data")]
	public class DroneData : ScriptableObject
	{
		[field: SerializeField]
		public Sprite DroneSprite { get; private set; }

		[field: SerializeField]
		public WeaponConfig WeaponConfig { get; private set; }
	}
}
