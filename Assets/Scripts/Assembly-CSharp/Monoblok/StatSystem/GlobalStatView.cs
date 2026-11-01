using UnityEngine;

namespace Monoblok.StatSystem
{
	[RequireComponent(typeof(RectTransform))]
	public class GlobalStatView : MonoBehaviour
	{
		[SerializeField]
		private StatCollectionView statCollectionView;

		public void Initialize(GlobalStatManager manager)
		{
			statCollectionView.Initialize(manager.StatManager.StatMap.Values);
		}
	}
}
