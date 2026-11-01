using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Monoblok.DamageSystem;
using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class ConeWeaponBehavior : WeaponBehavior
	{
		private readonly ConeWeapon coneWeapon;

		private readonly float tickInterval;

		private float tickTimer;

		private EventInstance fireRefInstance;

		public ConeWeaponBehavior(Weapon weapon, ConeWeaponConfig config)
			: base(weapon)
		{
			coneWeapon = Object.Instantiate(config.Prefab, weapon.statManager.transform);
			coneWeapon.transform.localPosition = Vector3.zero;
			coneWeapon.Initialize(weapon.layer);
			tickInterval = config.TickInterval;
			if (!config.FireRef.IsNull)
			{
				fireRefInstance = RuntimeManager.CreateInstance(config.FireRef);
			}
		}

		public override void OnDisable()
		{
			Stop();
		}

		public override void OnTargetLost()
		{
			Stop();
		}

		private void Stop()
		{
			coneWeapon.Stop();
			fireRefInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			tickTimer = 0f;
		}

		public override void Update(float deltaTime, ITarget target)
		{
			fireRefInstance.getPlaybackState(out var state);
			if (state != 0)
			{
				fireRefInstance.start();
			}
			Vector2 normalized = ((Vector2)target.Position - (Vector2)weapon.origin.position).normalized;
			coneWeapon.Fire(normalized, weapon.Range);
			IReadOnlyList<Health> targets = coneWeapon.Targets;
			if (targets.Count == 0)
			{
				return;
			}
			tickTimer += deltaTime;
			while (tickTimer >= tickInterval)
			{
				tickTimer -= tickInterval;
				for (int num = targets.Count - 1; num >= 0; num--)
				{
					weapon.DealDamage(targets[num], Damage.DamageType.Projectile);
				}
			}
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
		}
	}
}
