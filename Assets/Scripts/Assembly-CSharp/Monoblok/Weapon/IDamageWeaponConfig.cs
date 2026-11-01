using Monoblok.StatSystem;

namespace Monoblok.Weapon
{
	public interface IDamageWeaponConfig
	{
		StatInitializer BaseDamage { get; }

		StatInitializer CritChance { get; }
	}
}
