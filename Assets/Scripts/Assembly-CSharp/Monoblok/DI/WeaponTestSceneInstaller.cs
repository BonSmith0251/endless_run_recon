using Monoblok.DamageSystem;
using Reflex.Core;
using UnityEngine;

namespace Monoblok.DI
{
	public class WeaponTestSceneInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField]
		private DamageNumberRenderer damageNumberRenderer;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterValue(damageNumberRenderer);
		}
	}
}
