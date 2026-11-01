using UnityEngine;
using UnityEngine.InputSystem;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Abilities/Slot Config")]
	public class AbilitySlotConfig : ScriptableObject
	{
		[SerializeField]
		private string displayName = "Slot";

		[SerializeField]
		private InputActionReference hotkey;

		public string DisplayName => displayName;

		public InputActionReference Hotkey => hotkey;
	}
}
