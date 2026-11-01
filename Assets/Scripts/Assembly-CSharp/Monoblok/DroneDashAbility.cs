using Monoblok.DroneSystem;
using Monoblok.StatSystem;

namespace Monoblok
{
	public class DroneDashAbility : Ability
	{
		private readonly PlayerDrones playerDrones;

		private readonly StatInitializer dashDistanceInitializer;

		private readonly Stat dashDistance;

		public DroneDashAbility(DroneDashAbilityConfig config)
			: base(config)
		{
			playerDrones = config.PlayerDrones;
			dashDistanceInitializer = config.DashDistance;
			dashDistance = base.StatManager.AcquireStat(dashDistanceInitializer);
		}

		protected override void Execute()
		{
			playerDrones.Controller.PerformDash(dashDistance.Value);
		}

		protected override void ReleaseStats()
		{
			base.StatManager.ReleaseStat(dashDistanceInitializer.StatDefinition);
		}
	}
}
