using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Cart Tier")]
	public class CartTier : ScriptableObject
	{
		[SerializeField]
		private LocalizedString localizedDisplayName;

		public string DisplayName => localizedDisplayName.GetLocalizedString();

		[field: SerializeField]
		public Color Color { get; private set; }

		[field: SerializeField]
		[field: Min(1f)]
		public int MergeWaveCount { get; private set; } = 1;


		[field: SerializeField]
		[field: Min(0f)]
		public int RepairCost { get; private set; } = 100;


		[field: SerializeField]
		[field: Min(0f)]
		public int MergeCost { get; private set; } = 100;

	}
}
