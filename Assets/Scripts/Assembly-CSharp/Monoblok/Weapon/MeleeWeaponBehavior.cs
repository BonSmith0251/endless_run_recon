using System;
using System.Collections.Generic;
using Monoblok.DamageSystem;
using Monoblok.Movement;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class MeleeWeaponBehavior : WeaponBehavior
	{
		private readonly MeleeWeaponConfig config;

		private readonly List<MeleeWeapon> meleeWeapons = new List<MeleeWeapon>();

		private Stat speed;

		private Stat count;

		private float staggerEndTime = -1f;

		public MeleeWeaponBehavior(Weapon weapon, MeleeWeaponConfig config)
			: base(weapon)
		{
			this.config = config;
		}

		private void HandleHit(Health health)
		{
			weapon.DealDamage(health, Damage.DamageType.Contact);
			staggerEndTime = Time.time + config.StaggerDuration;
		}

		public override void Update(float deltaTime, ITarget target)
		{
			float num = speed.Value * GetStaggerMultiplier();
			for (int i = 0; i < meleeWeapons.Count; i++)
			{
				MeleeWeapon meleeWeapon = meleeWeapons[i];
				if (!(meleeWeapon == null))
				{
					float angleOffset = 360f / (float)meleeWeapons.Count * (float)i * (MathF.PI / 180f);
					meleeWeapon.Spin(num, weapon.Range, angleOffset);
				}
			}
		}

		private float GetStaggerMultiplier()
		{
			float num = Time.time - staggerEndTime;
			if (num < 0f)
			{
				return config.StaggerSpeedMultiplier;
			}
			if (num < config.StaggerRecoverDuration)
			{
				float t = num / config.StaggerRecoverDuration;
				return Mathf.Lerp(config.StaggerSpeedMultiplier, 1f, t);
			}
			return 1f;
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			builder.Add(config.Speed, out speed).Add(config.Count, out count);
			count.OnValueChanged += HandleCountChanged;
			HandleCountChanged(count.Value);
		}

		private void HandleCountChanged(float value)
		{
			int num = (int)value;
			for (int num2 = meleeWeapons.Count - 1; num2 >= 0; num2--)
			{
				MeleeWeapon meleeWeapon = meleeWeapons[num2];
				meleeWeapon.OnHit -= HandleHit;
				UnityEngine.Object.Destroy(meleeWeapon.gameObject);
			}
			meleeWeapons.Clear();
			for (int i = 0; i < num; i++)
			{
				MeleeWeapon meleeWeapon2 = UnityEngine.Object.Instantiate(config.WeaponPrefab, weapon.origin);
				meleeWeapon2.Initialize(weapon.layer);
				meleeWeapon2.OnHit += HandleHit;
				meleeWeapons.Add(meleeWeapon2);
			}
		}

		public override void OnEnable()
		{
			foreach (MeleeWeapon meleeWeapon in meleeWeapons)
			{
				meleeWeapon.gameObject.SetActive(value: true);
			}
		}

		public override void OnDisable()
		{
			foreach (MeleeWeapon meleeWeapon in meleeWeapons)
			{
				meleeWeapon.gameObject.SetActive(value: false);
			}
		}
	}
}
