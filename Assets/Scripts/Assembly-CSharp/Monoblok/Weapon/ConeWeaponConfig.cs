using FMODUnity;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Config/Cone")]
	public class ConeWeaponConfig : WeaponConfig
	{
		[field: SerializeField]
		public EventReference FireRef { get; private set; }

		[field: SerializeField]
		public ConeWeapon Prefab { get; private set; }

		[field: SerializeField]
		public float TickInterval { get; private set; } = 0.5f;


		protected override WeaponBehavior CreateBehaviorInstance(Weapon weapon)
		{
			return new ConeWeaponBehavior(weapon, this);
		}
	}
}
