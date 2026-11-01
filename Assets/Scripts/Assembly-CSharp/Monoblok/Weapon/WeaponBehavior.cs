using Monoblok.Movement;

namespace Monoblok.Weapon
{
	public abstract class WeaponBehavior : IWeaponBehavior
	{
		protected readonly Weapon weapon;

		public WeaponBehavior(Weapon weapon)
		{
			this.weapon = weapon;
		}

		internal abstract void RegisterStats(WeaponStatBuilder builder);

		public virtual void OnTargetLost()
		{
		}

		public virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
		}

		public abstract void Update(float deltaTime, ITarget target);
	}
}
