using System;
using System.Collections.Generic;
using Monoblok.DamageSystem;
using Monoblok.Movement;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class Weapon
	{
		private readonly IWeaponBehavior behavior;

		private readonly WeaponConfig config;

		private readonly List<WeaponModifier> modifiers = new List<WeaponModifier>();

		private readonly Dictionary<Type, WeaponModifier> modifiersByDefinitionType = new Dictionary<Type, WeaponModifier>();

		private readonly Dictionary<Type, int> modifierRefCounts = new Dictionary<Type, int>();

		public readonly StatManager statManager;

		public readonly Transform origin;

		public readonly int layer;

		private bool hasTarget;

		private bool isEnabled;

		private Stat baseDamage;

		private Stat critChance;

		private Stat range;

		public float Range => range.Value;

		public event Action<float> OnRangeChanged;

		public event Action<IDamageable, Damage> OnHit;

		public Weapon(WeaponConfig config, StatManager statManager, Transform origin, int layer)
		{
			this.statManager = statManager;
			this.config = config;
			this.origin = origin;
			this.layer = layer;
			WeaponStatBuilder weaponStatBuilder = new WeaponStatBuilder(statManager);
			weaponStatBuilder.Add(config.Range, out range);
			if (config is IDamageWeaponConfig damageWeaponConfig)
			{
				weaponStatBuilder.Add(damageWeaponConfig.BaseDamage, out baseDamage).Add(damageWeaponConfig.CritChance, out critChance);
			}
			behavior = config.CreateBehavior(this);
			foreach (WeaponModifierDefinition modifier2 in config.Modifiers)
			{
				TryAddModifier(modifier2, out var _);
			}
			statManager.TryGetStat(config.Range.StatDefinition, out var stat);
			stat.OnValueChanged += HandleRangeValueChanged;
		}

		private void HandleRangeValueChanged(float range)
		{
			this.OnRangeChanged?.Invoke(range);
		}

		public bool TryAddModifier(WeaponModifierDefinition definition, out WeaponModifier modifier)
		{
			Type type = definition.GetType();
			if (modifiersByDefinitionType.TryGetValue(type, out modifier))
			{
				modifierRefCounts[type]++;
				return true;
			}
			modifier = definition.CreateModifier(this);
			modifiers.Add(modifier);
			modifiersByDefinitionType.Add(type, modifier);
			modifierRefCounts.Add(type, 1);
			if (isEnabled)
			{
				modifier.OnEnable();
			}
			return true;
		}

		public void RemoveModifier(WeaponModifier modifier)
		{
			Type type = null;
			foreach (KeyValuePair<Type, WeaponModifier> item in modifiersByDefinitionType)
			{
				if (item.Value == modifier)
				{
					type = item.Key;
					break;
				}
			}
			if (!(type == null) && --modifierRefCounts[type] <= 0)
			{
				modifierRefCounts.Remove(type);
				modifiersByDefinitionType.Remove(type);
				modifiers.Remove(modifier);
				modifier.OnDisable();
				modifier.UnregisterStats(new WeaponStatBuilder(statManager));
			}
		}

		public void Update(float deltaTime, ITarget target)
		{
			if (!isEnabled)
			{
				return;
			}
			for (int i = 0; i < modifiers.Count; i++)
			{
				modifiers[i].Update(deltaTime);
			}
			if (target == null)
			{
				if (hasTarget)
				{
					behavior.OnTargetLost();
				}
				hasTarget = false;
			}
			else
			{
				behavior.Update(deltaTime, target);
				hasTarget = true;
			}
		}

		public void Enable()
		{
			isEnabled = true;
			behavior?.OnEnable();
			for (int i = 0; i < modifiers.Count; i++)
			{
				modifiers[i].OnEnable();
			}
		}

		public void Disable()
		{
			isEnabled = false;
			behavior?.OnDisable();
			for (int i = 0; i < modifiers.Count; i++)
			{
				modifiers[i].OnDisable();
			}
		}

		internal Damage CalculateDamage(Damage.DamageType type)
		{
			if (baseDamage == null)
			{
				return new Damage(0, 0, type);
			}
			int addition = 0;
			if (UnityEngine.Random.value < critChance.Value)
			{
				addition = Mathf.CeilToInt((float)(int)baseDamage.Value * 0.5f);
			}
			return new Damage((int)baseDamage.Value, addition, type);
		}

		public void DealDamage(IDamageable target, Damage.DamageType type)
		{
			if (target != null)
			{
				Damage damage = CalculateDamage(type);
				target.Damage(damage);
				this.OnHit?.Invoke(target, damage);
			}
		}
	}
}
