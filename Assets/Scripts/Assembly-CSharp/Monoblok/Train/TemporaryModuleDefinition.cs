using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Modules/Temporary")]
	public class TemporaryModuleDefinition : ModuleDefinition
	{
		public enum ExecutionTime
		{
			Instant,
			OnDelivered
		}

		[SerializeField]
		private TemporaryModule modulePrefab;

		[SerializeField]
		private ExecutionTime executionTime = ExecutionTime.OnDelivered;

		[SerializeReference]
		[SubclassSelector]
		private ITemporaryModuleAction action;

		public override ModuleBehavior CreateBehavior(Transform parent)
		{
			TemporaryModule temporaryModule = Object.Instantiate(modulePrefab, parent);
			temporaryModule.Initialize(executionTime, action.Execute, base.Sprite);
			return temporaryModule;
		}
	}
}
