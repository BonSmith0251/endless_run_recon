using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	public abstract class UpgradeData : ScriptableObject
	{
		[SerializeField]
		private LocalizedString localizedName;

		[SerializeField]
		private LocalizedString localizedDescription;

		[SerializeField]
		protected Sprite sprite;

		[SerializeField]
		private Sprite uiIcon;

		public string UpgradeName => localizedName.GetLocalizedString();

		public virtual string Description => localizedDescription.GetLocalizedString();

		public Sprite Sprite => sprite;

		public Sprite UIIcon => uiIcon;

		protected string GetLocalizedDescription(params object[] arguments)
		{
			return localizedDescription.GetLocalizedString(arguments);
		}

		public abstract IUpgrade GetUpgrade();
	}
}
