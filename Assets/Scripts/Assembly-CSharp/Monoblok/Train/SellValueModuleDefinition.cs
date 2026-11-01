using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Modules/Sell Value")]
	public class SellValueModuleDefinition : ModuleDefinition
	{
		[SerializeField]
		private SellValueModuleBehavior modulePrefab;

		[SerializeField]
		[Min(0f)]
		private int step;

		public override string Description => GetLocalizedDescription(step);

		public override ModuleBehavior CreateBehavior(Transform parent)
		{
			SellValueModuleBehavior sellValueModuleBehavior = Object.Instantiate(modulePrefab, parent);
			sellValueModuleBehavior.Initialize(step, base.Sprite);
			return sellValueModuleBehavior;
		}
	}
}
