using System.Linq;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monoblok
{
	public class AbilitySlotView : MonoBehaviour
	{
		[SerializeField]
		private AbilityView abilityViewPrefab;

		[SerializeField]
		private RectTransform abilityContainer;

		[SerializeField]
		private ShakeSettings rechargeSettings;

		[SerializeField]
		private Transform rechargeTweenTarget;

		private AbilitySlot slot;

		private AbilityView currentAbilityView;

		private void OnDestroy()
		{
			if (slot != null)
			{
				slot.OnAbilityChanged -= HandleAbilityChanged;
				slot.OnAbilityChargeRestored -= HandleRestored;
			}
		}

		public void Initialize(AbilitySlot slot)
		{
			this.slot = slot;
			HandleAbilityChanged(slot.Ability);
			slot.OnAbilityChanged += HandleAbilityChanged;
			slot.OnAbilityChargeRestored += HandleRestored;
		}

		private void HandleRestored()
		{
			Tween.PunchScale(rechargeTweenTarget, rechargeSettings);
		}

		public void Tick()
		{
		}

		private void HandleAbilityChanged(Ability ability)
		{
			if (currentAbilityView != null)
			{
				Object.Destroy(currentAbilityView.gameObject);
			}
			if (ability != null)
			{
				currentAbilityView = Object.Instantiate(abilityViewPrefab, abilityContainer);
				currentAbilityView.Initialize(ability);
			}
		}

		private static string ResolveHotkeyLabel(AbilitySlot slot)
		{
			InputAction hotkeyAction = slot.HotkeyAction;
			if (hotkeyAction == null)
			{
				return string.Empty;
			}
			InputBinding inputBinding = hotkeyAction.bindings.FirstOrDefault((InputBinding b) => !b.isComposite && !b.isPartOfComposite);
			if (inputBinding == default(InputBinding))
			{
				return string.Empty;
			}
			return InputControlPath.ToHumanReadableString(inputBinding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
		}
	}
}
