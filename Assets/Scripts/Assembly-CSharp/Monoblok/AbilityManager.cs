using System.Collections.Generic;
using FMODUnity;
using Monoblok.Progression;
using UnityEngine;
using ZLinq;

namespace Monoblok
{
	public class AbilityManager : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private List<AbilitySlotConfig> slotConfigs = new List<AbilitySlotConfig>();

		[SerializeField]
		private AbilityConfig[] initialAbilities;

		[SerializeField]
		private AbilityManagerView managerView;

		[Space]
		[SerializeField]
		private EventReference rechargeRef;

		private readonly List<AbilitySlot> _slots = new List<AbilitySlot>();

		private bool abilitiesEnabled;

		public IReadOnlyList<AbilitySlot> Slots => _slots;

		private void OnValidate()
		{
			if (initialAbilities.Length > slotConfigs.Count)
			{
				Debug.LogWarning("More initial abilities than slots", this);
			}
		}

		private void OnEnable()
		{
			progression.OnWaveEnded += HandleWaveEnded;
		}

		private void OnDisable()
		{
			progression.OnWaveEnded -= HandleWaveEnded;
		}

		private void HandleWaveEnded()
		{
			foreach (AbilitySlot slot in Slots)
			{
				if (slot.HasAbility)
				{
					slot.Ability.CancelAbility();
					slot.Ability.Restore();
				}
			}
		}

		public void EnableAbilities(bool enable)
		{
			foreach (AbilitySlot slot in _slots)
			{
				if (enable)
				{
					slot.EnableInput();
				}
				else
				{
					slot.DisableInput();
				}
			}
			abilitiesEnabled = enable;
		}

		public void Initialize()
		{
			foreach (AbilitySlot slot in Slots)
			{
				slot.OnAbilityChargeRestored -= HandleAbilityChargeRestored;
				slot.UnregisterAbility();
			}
			_slots.Clear();
			foreach (AbilitySlotConfig slotConfig in slotConfigs)
			{
				AbilitySlot abilitySlot = new AbilitySlot(slotConfig);
				abilitySlot.OnAbilityChargeRestored += HandleAbilityChargeRestored;
				_slots.Add(abilitySlot);
				managerView.RegisterSlot(abilitySlot);
				if (abilitiesEnabled)
				{
					abilitySlot.EnableInput();
				}
			}
			for (int i = 0; i < initialAbilities.Length; i++)
			{
				RegisterAbility(i, initialAbilities[i].GetAbility());
			}
		}

		private void HandleAbilityChargeRestored()
		{
			RuntimeManager.PlayOneShot(rechargeRef);
		}

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			foreach (AbilitySlot slot in _slots)
			{
				slot.Tick(deltaTime);
			}
		}

		public void RegisterAbility(Ability ability)
		{
			AbilitySlot abilitySlot = Slots.AsValueEnumerable().FirstOrDefault((AbilitySlot s) => !s.HasAbility);
			if (abilitySlot == null)
			{
				Debug.LogError("Trying to register " + ability.GetType().Name + ", but no free slots available");
			}
			else
			{
				abilitySlot.RegisterAbility(ability);
			}
		}

		public void RegisterAbility(int slotIndex, Ability ability)
		{
			if (slotIndex < 0 || slotIndex >= _slots.Count)
			{
				Debug.LogWarning($"AbilityManager: slot index {slotIndex} out of range.");
			}
			else
			{
				_slots[slotIndex].RegisterAbility(ability);
			}
		}

		public void UnregisterAbility(AbilitySlot slot)
		{
			slot.UnregisterAbility();
		}

		public void UnregisterAbility(Ability ability)
		{
			AbilitySlot abilitySlot = Slots.AsValueEnumerable().FirstOrDefault((AbilitySlot s) => s.Ability == ability);
			if (abilitySlot == null)
			{
				Debug.LogError("Trying to unregister " + ability.GetType().Name + ", but slot is not found");
			}
			else
			{
				abilitySlot.UnregisterAbility();
			}
		}

		public void UnregisterAbility(int slotIndex)
		{
			if (slotIndex >= 0 && slotIndex < _slots.Count)
			{
				_slots[slotIndex].UnregisterAbility();
			}
		}

		public void UnregisterAll()
		{
			foreach (AbilitySlot slot in _slots)
			{
				slot.UnregisterAbility();
			}
		}

		public AbilitySlot AddSlot(AbilitySlotConfig config)
		{
			AbilitySlot abilitySlot = new AbilitySlot(config);
			abilitySlot.OnAbilityChargeRestored += HandleAbilityChargeRestored;
			if (abilitiesEnabled)
			{
				abilitySlot.EnableInput();
			}
			_slots.Add(abilitySlot);
			return abilitySlot;
		}

		public void RemoveSlot(AbilitySlot slot)
		{
			slot.DisableInput();
			slot.OnAbilityChargeRestored -= HandleAbilityChargeRestored;
			_slots.Remove(slot);
		}

		public bool HasAbility(AbilityConfig ability)
		{
			return Slots.AsValueEnumerable().Any((AbilitySlot s) => s.Ability.Config == ability);
		}
	}
}
