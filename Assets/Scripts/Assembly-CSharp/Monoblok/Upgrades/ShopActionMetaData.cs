using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Shop/Action Metadata")]
	public class ShopActionMetaData : ScriptableObject
	{
		[SerializeField]
		private LocalizedString localizedActionName;

		[SerializeField]
		private LocalizedString localizedActionDescription;

		[field: SerializeField]
		public Sprite Icon { get; private set; }

		public string ActionName => localizedActionName.GetLocalizedString();

		public string ActionDescription => localizedActionDescription.GetLocalizedString();

		[field: SerializeField]
		public EventReference ResolveEventRef { get; private set; }
	}
}
