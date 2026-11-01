using System;
using Monoblok.Progression;
using Monoblok.StatSystem;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class WeaponHandler : MonoBehaviour
	{
		[SerializeField]
		private Transform origin;

		[SerializeField]
		private WeaponConfig initialWeapon;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private WeaponTargetProvider targetProvider;

		[SerializeField]
		[Layer]
		private int targetLayer;

		[SerializeField]
		private StatManager statManager;

		[SerializeField]
		private bool startEnabled;

		private Weapon weapon;

		private bool canShoot = true;

		private WeaponConfig config;

		public StatManager StatManager => statManager;

		public WeaponTargetProvider TargetProvider => targetProvider;

		private void Awake()
		{
			Initialize(initialWeapon);
			if (progression == null)
			{
				Debug.LogError("No progression assigned");
				return;
			}
			progression.OnWaveStarted += OnWaveStarted;
			progression.OnWaveEnded += OnWaveEnded;
		}

		private void OnDestroy()
		{
			if (!(progression == null))
			{
				progression.OnWaveStarted -= OnWaveStarted;
				progression.OnWaveEnded -= OnWaveEnded;
			}
		}

		private void Start()
		{
			if (startEnabled && progression.WaveInProgress)
			{
				EnableWeapon();
			}
		}

		private void OnWaveStarted()
		{
			EnableWeapon();
		}

		private void OnWaveEnded()
		{
			DisableWeapon();
		}

		[Button(null, EButtonEnableMode.Always)]
		public void EnableWeapon()
		{
			if (weapon != null)
			{
				weapon.Enable();
			}
		}

		public void DisableWeapon()
		{
			if (weapon != null)
			{
				weapon.Disable();
			}
		}

		public void Initialize(WeaponConfig config)
		{
			if (!(config == null))
			{
				if (weapon != null)
				{
					throw new NotImplementedException("Changing weapons is currently unsupported");
				}
				this.config = config;
				weapon = new Weapon(config, statManager, origin, targetLayer);
				weapon.OnRangeChanged += HandleRangeChanged;
				HandleRangeChanged(weapon.Range);
			}
		}

		private void HandleRangeChanged(float range)
		{
			targetProvider.OnRangeChanged(range);
		}

		public void RemoveWeapon()
		{
			weapon = null;
		}

		public void RegisterStatManager(StatManager statManager)
		{
			this.statManager = statManager;
			Initialize(config);
		}

		public void SetCanShoot(bool canShoot)
		{
			this.canShoot = canShoot;
		}

		public bool TryAddWeaponModifier(WeaponModifierDefinition definition, out WeaponModifier modifier)
		{
			if (weapon == null)
			{
				modifier = null;
				return false;
			}
			return weapon.TryAddModifier(definition, out modifier);
		}

		public void RemoveWeaponModifier(WeaponModifier modifier)
		{
			weapon?.RemoveModifier(modifier);
		}

		private void Update()
		{
			if (canShoot && weapon != null && !(targetProvider == null) && (!(progression != null) || progression.WaveInProgress))
			{
				weapon.Update(Time.deltaTime, targetProvider.Target);
			}
		}
	}
}
