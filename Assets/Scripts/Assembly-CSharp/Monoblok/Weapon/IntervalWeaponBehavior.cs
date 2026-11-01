using FMODUnity;
using Monoblok.Movement;
using Monoblok.StatSystem;

namespace Monoblok.Weapon
{
	public abstract class IntervalWeaponBehavior : WeaponBehavior
	{
		private readonly IntervalWeaponConfig config;

		private float t;

		private Stat fireRate;

		public IntervalWeaponBehavior(Weapon weapon, IntervalWeaponConfig config)
			: base(weapon)
		{
			this.config = config;
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			builder.Add(config.FireRate, out fireRate);
		}

		public override void Update(float deltaTime, ITarget target)
		{
			t += deltaTime;
			float num = 1f / fireRate.Value;
			while (t >= num)
			{
				PlaySound();
				OnTick(target);
				t -= num;
			}
		}

		private void PlaySound()
		{
			if (!config.SFXRef.IsNull)
			{
				RuntimeManager.PlayOneShot(config.SFXRef, weapon.origin.position);
			}
		}

		protected abstract void OnTick(ITarget target);
	}
}
