using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Module Database")]
	public class ModuleDatabase : ScriptableObject
	{
		[SerializeField]
		private List<ModuleDefinition> modules = new List<ModuleDefinition>();

		public IReadOnlyList<ModuleDefinition> Modules => modules;

		public void AddModule(ModuleDefinition module)
		{
			modules.Add(module);
		}

		public void RemoveModule(ModuleDefinition module)
		{
			modules.Remove(module);
		}
	}
}
