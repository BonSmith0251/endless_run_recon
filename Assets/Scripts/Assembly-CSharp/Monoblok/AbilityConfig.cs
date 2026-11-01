using FMODUnity;
using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok
{
	public abstract class AbilityConfig : ScriptableObject
	{
		[SerializeField]
		private LocalizedString localizedName;

		[SerializeField]
		private LocalizedString localizedDescription;

		[SerializeField]
		private GlobalStatManagerRuntimeValue globalStatManagerRuntimeValue;

		public string AbilityName => localizedName.GetLocalizedString();

		public string AbilityDescription => localizedDescription.GetLocalizedString();

		[field: SerializeField]
		public Sprite Icon { get; private set; }

		[field: SerializeField]
		public StatInitializer Cooldown { get; private set; }

		[field: SerializeField]
		public StatInitializer MaxCharges { get; private set; }

		[field: SerializeField]
		public EventReference PerformRef { get; private set; }

		[field: SerializeField]
		public GameEvent PerformEvent { get; private set; }

		public GlobalStatManager GlobalStatManager => globalStatManagerRuntimeValue.Value;

		public StatManager StatManager => GlobalStatManager.StatManager;

		public abstract Ability GetAbility();
	}
}
