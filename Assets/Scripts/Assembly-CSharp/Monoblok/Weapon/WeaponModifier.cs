using Monoblok.DamageSystem;

namespace Monoblok.Weapon
{
	public abstract class WeaponModifier
	{
		protected readonly Weapon weapon;

		protected WeaponModifier(Weapon weapon)
		{
			this.weapon = weapon;
		}

		internal virtual void RegisterStats(WeaponStatBuilder builder)
		{
		}

		internal virtual void UnregisterStats(WeaponStatBuilder builder)
		{
		}

		public virtual void OnEnable()
		{
			weapon.OnHit += HandleHit;
		}

		public virtual void OnDisable()
		{
			weapon.OnHit -= HandleHit;
		}

		public virtual void Update(float deltaTime)
		{
		}

		protected abstract void HandleHit(IDamageable target, Damage damage);
	}
}
