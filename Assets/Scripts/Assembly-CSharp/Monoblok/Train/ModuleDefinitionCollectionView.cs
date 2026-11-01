using System.Collections.Generic;
using Monoblok.Core;
using UnityEngine;

namespace Monoblok.Train
{
	public class ModuleDefinitionCollectionView : MonoBehaviour
	{
		[SerializeField]
		private ModuleDefinitionView viewPrefab;

		[SerializeField]
		private RectTransform container;

		public void Initialize(IEnumerable<ModuleDefinition> moduleDefinitions)
		{
			container.Clear();
			foreach (ModuleDefinition moduleDefinition in moduleDefinitions)
			{
				Object.Instantiate(viewPrefab, container).Initialize(moduleDefinition);
			}
		}
	}
}
