using Monoblok.StatSystem;

namespace Monoblok.Weapon
{
	public class WeaponStatBuilder
	{
		private readonly StatManager statManager;

		public WeaponStatBuilder(StatManager statManager)
		{
			this.statManager = statManager;
		}

		public WeaponStatBuilder Add(StatInitializer statInitializer, out Stat weaponStat)
		{
			weaponStat = statManager.AcquireStat(statInitializer);
			return this;
		}

		public WeaponStatBuilder Remove(StatInitializer statInitializer)
		{
			statManager.ReleaseStat(statInitializer.StatDefinition);
			return this;
		}
	}
}
