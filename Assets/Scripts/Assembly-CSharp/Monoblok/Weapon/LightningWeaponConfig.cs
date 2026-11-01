using DigitalRuby.LightningBolt;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Config/Lightning")]
	public class LightningWeaponConfig : IntervalWeaponConfig
	{
		[field: SerializeField]
		public LightningBoltScript Prefab { get; private set; }

		protected override IntervalWeaponBehavior CreateIntervalBehavior(Weapon weapon)
		{
			return new LightningBehavior(weapon, this);
		}
	}
}
