using Monoblok.Upgrades;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	public abstract class ModuleDefinition : IDScriptableObject
	{
		[SerializeField]
		private string fallbackName;

		[SerializeField]
		private string fallbackDescription;

		[SerializeField]
		private LocalizedString localizedName;

		[SerializeField]
		private LocalizedString localizedDescription;

		[SerializeReference]
		[SubclassSelector]
		private IRollEligibility[] eligibilities;

		public string DisplayName
		{
			get
			{
				if (localizedName != null && !localizedName.IsEmpty)
				{
					return localizedName.GetLocalizedString();
				}
				return fallbackName;
			}
		}

		public virtual string Description
		{
			get
			{
				if (localizedDescription != null && !localizedDescription.IsEmpty)
				{
					return localizedDescription.GetLocalizedString();
				}
				return fallbackDescription;
			}
		}

		[field: SerializeField]
		public Sprite Sprite { get; private set; }

		[field: SerializeField]
		public Price BasePrice { get; private set; }

		[field: SerializeField]
		public bool IsOffered { get; private set; } = true;


		public void SetOffered(bool isOffered)
		{
			IsOffered = isOffered;
		}

		public bool IsEligibleToRoll(ShopActionContext ctx)
		{
			if (eligibilities == null || eligibilities.Length == 0)
			{
				return true;
			}
			IRollEligibility[] array = eligibilities;
			foreach (IRollEligibility rollEligibility in array)
			{
				if (rollEligibility != null && !rollEligibility.IsElgible(ctx))
				{
					return false;
				}
			}
			return true;
		}

		protected string GetLocalizedDescription(params object[] arguments)
		{
			if (localizedDescription != null && !localizedDescription.IsEmpty)
			{
				return localizedDescription.GetLocalizedString(arguments);
			}
			return fallbackDescription;
		}

		public abstract ModuleBehavior CreateBehavior(Transform parent);

		public ModuleData GetRuntimeData()
		{
			return new ModuleData(this, BasePrice);
		}

		public virtual bool CanBePlaced(TrainCart cart, out string errorMsg)
		{
			errorMsg = string.Empty;
			return true;
		}

		public virtual bool CanBeSold(TrainCart cart, out string errorMsg)
		{
			errorMsg = string.Empty;
			return true;
		}
	}
}
