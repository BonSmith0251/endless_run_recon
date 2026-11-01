using System.Collections.Generic;
using Monoblok.Core;
using UnityEngine;

namespace Monoblok.StatSystem
{
	public class StatCollectionView : MonoBehaviour
	{
		[SerializeField]
		private StatView statViewPrefab;

		[SerializeField]
		private RectTransform statContainer;

		private List<StatView> statViews = new List<StatView>();

		public void Initialize(IEnumerable<Stat> stats)
		{
			Clear();
			foreach (Stat stat in stats)
			{
				if (stat != null && stat.definition.Public)
				{
					StatView statView = Object.Instantiate(statViewPrefab, statContainer);
					statView.Initialize(stat);
					statViews.Add(statView);
				}
			}
		}

		private void Clear()
		{
			foreach (StatView statView in statViews)
			{
				statView.Dispose();
			}
			statContainer.Clear();
			statViews.Clear();
		}
	}
}
