using System.Collections.Generic;
using Monoblok.Weapon;
using UnityEngine;

namespace Monoblok.Train
{
	public class WeaponModifierModuleBehavior : ModuleBehavior
	{
		private ITrainCartTargetProvider targetProvider;

		private WeaponModifierDefinition modifierDefinition;

		private readonly Dictionary<WeaponHandler, WeaponModifier> grantedModifiers = new Dictionary<WeaponHandler, WeaponModifier>();

		private readonly HashSet<TrainCart> subscribedCarts = new HashSet<TrainCart>();

		public void Initialize(Sprite sprite, ITrainCartTargetProvider targetProvider, WeaponModifierDefinition modifierDefinition)
		{
			this.targetProvider = targetProvider;
			this.modifierDefinition = modifierDefinition;
			spriteRenderer.sprite = sprite;
		}

		protected override void OnAttached()
		{
			base.Slot.Cart.Train.OnCartsChanged += OnCartsChanged;
		}

		protected override void OnDetached()
		{
			base.Slot.Cart.Train.OnCartsChanged -= OnCartsChanged;
		}

		protected override void OnModuleEnabled()
		{
			ApplyAll();
		}

		protected override void OnModuleDisabled()
		{
			RemoveAll();
		}

		private void OnCartsChanged()
		{
			RemoveAll();
			ApplyAll();
		}

		private void ApplyAll()
		{
			if (!base.ModuleEnabled)
			{
				return;
			}
			foreach (TrainCart cart in targetProvider.GetCarts(base.Slot.Cart))
			{
				if (cart == null)
				{
					continue;
				}
				subscribedCarts.Add(cart);
				cart.OnWeaponHandlerRegistered += Grant;
				cart.OnWeaponHandlerUnregistered += Revoke;
				foreach (WeaponHandler weaponHandler in cart.WeaponHandlers)
				{
					Grant(weaponHandler);
				}
			}
		}

		private void RemoveAll()
		{
			foreach (TrainCart subscribedCart in subscribedCarts)
			{
				if (!(subscribedCart == null))
				{
					subscribedCart.OnWeaponHandlerRegistered -= Grant;
					subscribedCart.OnWeaponHandlerUnregistered -= Revoke;
				}
			}
			subscribedCarts.Clear();
			foreach (KeyValuePair<WeaponHandler, WeaponModifier> grantedModifier in grantedModifiers)
			{
				if (!(grantedModifier.Key == null))
				{
					grantedModifier.Key.RemoveWeaponModifier(grantedModifier.Value);
				}
			}
			grantedModifiers.Clear();
		}

		private void Grant(WeaponHandler handler)
		{
			if (handler.TryAddWeaponModifier(modifierDefinition, out var modifier))
			{
				grantedModifiers[handler] = modifier;
			}
		}

		private void Revoke(WeaponHandler handler)
		{
			if (grantedModifiers.TryGetValue(handler, out var value))
			{
				handler.RemoveWeaponModifier(value);
				grantedModifiers.Remove(handler);
			}
		}
	}
}
