using System;
using System.Collections.Generic;
using FMODUnity;
using Monoblok.DamageSystem;
using Monoblok.Movement;
using Monoblok.StatSystem;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Weapon
{
	public class ProjectileBehavior : IntervalWeaponBehavior
	{
		protected readonly ProjectileWeaponConfig config;

		protected readonly ObjectPool<Projectile> projectilePool;

		private readonly List<Projectile> activeProjectiles = new List<Projectile>();

		private Stat pierceCount;

		public ProjectileBehavior(Weapon weapon, ProjectileWeaponConfig config)
			: base(weapon, config)
		{
			this.config = config;
			projectilePool = new ObjectPool<Projectile>(Create, OnGet, OnRelease, OnDestroy);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			for (int num = activeProjectiles.Count - 1; num >= 0; num--)
			{
				activeProjectiles[num].ForceRelease();
			}
		}

		private Projectile Create()
		{
			Projectile projectile = UnityEngine.Object.Instantiate(config.ProjectilePrefab);
			projectile.OnReadyToRelease = (Action<Projectile>)Delegate.Combine(projectile.OnReadyToRelease, new Action<Projectile>(projectilePool.Release));
			projectile.OnHit += HandleProjectileHit;
			return projectile;
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			base.RegisterStats(builder);
			builder.Add(config.PierceCount, out pierceCount);
		}

		private void OnGet(Projectile projectile)
		{
			projectile.gameObject.SetActive(value: true);
		}

		private void OnRelease(Projectile projectile)
		{
			activeProjectiles.Remove(projectile);
			projectile.gameObject.SetActive(value: false);
		}

		private void OnDestroy(Projectile projectile)
		{
			projectile.OnHit -= HandleProjectileHit;
			projectile.OnReadyToRelease = (Action<Projectile>)Delegate.Remove(projectile.OnReadyToRelease, new Action<Projectile>(projectilePool.Release));
			activeProjectiles.Remove(projectile);
			if (projectile != null) UnityEngine.Object.Destroy(projectile.gameObject);
		}

		protected override void OnTick(ITarget target)
		{
			Vector2 normalized = ((Vector2)target.Position - (Vector2)weapon.origin.position).normalized;
			ProjectileData data = BuildData(normalized);
			SpawnProjectile(data);
		}

		protected ProjectileData BuildData(Vector2 direction)
		{
			return new ProjectileData(weapon.origin.position, direction, config.SpeedConstant, weapon.Range, (int)pierceCount.Value, weapon.layer);
		}

		protected void SpawnProjectile(ProjectileData data)
		{
			projectilePool.Get(out var v);
			v.transform.position = weapon.origin.position;
			v.Initialize(data);
			activeProjectiles.Add(v);
		}

		private void HandleProjectileHit(IDamageable damageable)
		{
			weapon.DealDamage(damageable, Damage.DamageType.Projectile);
			EventReference impactReference = config.ImpactReference;
			if (!impactReference.IsNull)
			{
				RuntimeManager.PlayOneShot(impactReference);
			}
		}
	}
}
