using System;
using UnityEngine.InputSystem;

namespace Monoblok
{
	public class AbilitySlot
	{
		private readonly InputActionReference _hotkeyReference;

		private Ability _ability;

		public string DisplayName { get; }

		public Ability Ability => _ability;

		public bool HasAbility => _ability != null;

		public InputAction HotkeyAction => _hotkeyReference?.action;

		public event Action<Ability> OnAbilityChanged;

		public event Action OnAbilityChargeRestored;

		public AbilitySlot(AbilitySlotConfig config)
		{
			DisplayName = config.DisplayName;
			_hotkeyReference = config.Hotkey;
		}

		public AbilitySlot(string displayName, InputActionReference hotkey)
		{
			DisplayName = displayName;
			_hotkeyReference = hotkey;
		}

		public void RegisterAbility(Ability ability)
		{
			_ability?.Dispose();
			_ability = ability;
			ability.OnChargeRestored += HandleChargeRestored;
			this.OnAbilityChanged?.Invoke(_ability);
		}

		public void UnregisterAbility()
		{
			if (_ability != null)
			{
				_ability.OnChargeRestored -= HandleChargeRestored;
			}
			_ability?.Dispose();
			_ability = null;
			this.OnAbilityChanged?.Invoke(null);
		}

		private void HandleChargeRestored()
		{
			this.OnAbilityChargeRestored?.Invoke();
		}

		public void EnableInput()
		{
			if (!(_hotkeyReference == null))
			{
				_hotkeyReference.action.performed += OnHotkeyPerformed;
			}
		}

		public void DisableInput()
		{
			if (!(_hotkeyReference == null))
			{
				_hotkeyReference.action.performed -= OnHotkeyPerformed;
			}
		}

		public void Tick(float deltaTime)
		{
			_ability?.Tick(deltaTime);
		}

		private void OnHotkeyPerformed(InputAction.CallbackContext ctx)
		{
			if (HasAbility)
			{
				_ability.TryUse();
			}
		}
	}
}
