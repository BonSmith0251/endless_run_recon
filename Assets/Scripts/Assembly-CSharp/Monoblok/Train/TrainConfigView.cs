using UnityEngine;

namespace Monoblok.Train
{
	public class TrainConfigView : MonoBehaviour
	{
		[SerializeField]
		private AbilityInfoView abilityInfoView;

		[SerializeField]
		private ModuleDefinitionCollectionView moduleDefinitionCollectionView;

		public void Initialize(TrainConfig config)
		{
			abilityInfoView.Initialize(config.AbiltiyConfig);
			moduleDefinitionCollectionView.Initialize(new ModuleDefinition[1] { config.InitialModule });
		}
	}
}
