using DigitalRuby.LightningBolt;
using FMODUnity;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	[CreateAssetMenu(menuName = "Game/Weapon Modifier/Lightning Chain")]
	public class LightningChainWeaponModifierDefinition : WeaponModifierDefinition
	{
		[field: SerializeField]
		public LightningBoltScript Prefab { get; private set; }

		[field: SerializeField]
		public StatInitializer ChainCount { get; private set; }

		[field: SerializeField]
		public StatInitializer Damage { get; private set; }

		[field: SerializeField]
		public StatInitializer ChainRange { get; private set; }

		[field: SerializeField]
		public float BoltDuration { get; private set; } = 0.08f;


		[field: SerializeField]
		public EventReference ChainRef { get; private set; }

		protected override WeaponModifier CreateModifierInstance(Weapon weapon)
		{
			return new LightningChainWeaponModifier(weapon, this);
		}
	}
}
