using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.StatSystem
{
	public class StatInitializerCollectionView : MonoBehaviour
	{
		[SerializeField]
		private StatInitializerView initializerViewPrefab;

		[SerializeField]
		private RectTransform container;

		private List<StatInitializerView> initializerViews = new List<StatInitializerView>();

		public void Initialize(IEnumerable<StatInitializer> initializers)
		{
			Clear();
			foreach (StatInitializer initializer in initializers)
			{
				if (initializer != null)
				{
					StatInitializerView statInitializerView = Object.Instantiate(initializerViewPrefab, container);
					statInitializerView.Initialize(initializer);
					initializerViews.Add(statInitializerView);
				}
			}
		}

		private void Clear()
		{
			foreach (StatInitializerView initializerView in initializerViews)
			{
				if (!(initializerView == null))
				{
					Object.Destroy(initializerView.gameObject);
				}
			}
			initializerViews.Clear();
		}
	}
}
