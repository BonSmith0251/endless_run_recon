using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Config/Laser")]
	public class LaserWeaponConfig : IntervalWeaponConfig
	{
		[field: Space]
		[field: Header("Laser Config")]
		[field: SerializeField]
		public Laser LaserPrefab { get; private set; }

		[field: SerializeField]
		public StatInitializer Width { get; private set; }

		protected override IntervalWeaponBehavior CreateIntervalBehavior(Weapon weapon)
		{
			return new LaserBehavior(weapon, this);
		}
	}
}
