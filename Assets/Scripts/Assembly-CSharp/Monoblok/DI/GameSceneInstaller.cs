using Monoblok.DamageSystem;
using Monoblok.Environment;
using Reflex.Core;
using UnityEngine;

namespace Monoblok.DI
{
	public class GameSceneInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField]
		private DamageNumberRenderer damageNumberRenderer;

		[SerializeField]
		private EnvironmentController environmentController;

		[SerializeField]
		private TracksManager tracksManager;

		[SerializeField]
		private DangerManager dangerManager;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterValue(damageNumberRenderer);
			containerBuilder.RegisterValue(environmentController);
			containerBuilder.RegisterValue(tracksManager);
			containerBuilder.RegisterValue(dangerManager);
		}
	}
}
