using Monoblok.DamageSystem;
using Monoblok.Movement;
using Monoblok.StatSystem;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Weapon
{
	public class LaserBehavior : IntervalWeaponBehavior
	{
		private readonly LaserWeaponConfig config;

		private readonly ObjectPool<Laser> laserPool;

		private Stat width;

		public LaserBehavior(Weapon weapon, LaserWeaponConfig config)
			: base(weapon, config)
		{
			this.config = config;
			laserPool = new ObjectPool<Laser>(Create, OnGet, OnRelease, OnDestroy);
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			base.RegisterStats(builder);
			builder.Add(config.Width, out width);
		}

		private Laser Create()
		{
			return Object.Instantiate(config.LaserPrefab);
		}

		private void OnGet(Laser laser)
		{
			laser.gameObject.SetActive(value: true);
			laser.transform.SetParent(weapon.origin);
			laser.OnReadyToRelease -= laserPool.Release;
			laser.OnReadyToRelease += laserPool.Release;
			laser.OnHit -= HandleLaserHit;
			laser.OnHit += HandleLaserHit;
		}

		private void OnRelease(Laser laser)
		{
			laser.gameObject.SetActive(value: false);
			laser.transform.SetParent(null);
			laser.OnReadyToRelease -= laserPool.Release;
			laser.OnHit -= HandleLaserHit;
		}

		private void OnDestroy(Laser laser)
		{
			if (laser != null) Object.Destroy(laser.gameObject);
			laser.OnReadyToRelease -= laserPool.Release;
			laser.OnHit -= HandleLaserHit;
		}

		private void HandleLaserHit(IDamageable damageable)
		{
			weapon.DealDamage(damageable, Damage.DamageType.Projectile);
		}

		protected override void OnTick(ITarget target)
		{
			Vector2 normalized = ((Vector2)target.Position - (Vector2)weapon.origin.position).normalized;
			Laser laser = laserPool.Get();
			laser.transform.position = weapon.origin.position;
			laser.Initialize(normalized, weapon.layer, width.Value, weapon.Range);
		}
	}
}
