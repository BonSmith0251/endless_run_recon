using System.Collections.Generic;
using Monoblok.Core;
using Monoblok.Inventory;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	public class ModuleCollectionView : MonoBehaviour
	{
		[SerializeField]
		private ModuleUIView moduleViewPrefab;

		[SerializeField]
		private RectTransform moduleContainer;

		[SerializeField]
		private bool draggableModules;

		public void Initialize(IEnumerable<ModuleData> modules)
		{
			Clear();
			foreach (ModuleData module in modules)
			{
				ModuleUIView moduleUIView = Object.Instantiate(moduleViewPrefab, moduleContainer);
				moduleUIView.draggable = draggableModules;
				moduleUIView.Initialize(module);
			}
		}

		private void Clear()
		{
			moduleContainer.Clear();
		}
	}
}
