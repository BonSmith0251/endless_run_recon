using UnityEngine;

namespace Monoblok.StatSystem
{
	public class GlobalStatManager : MonoBehaviour
	{
		[SerializeField]
		private StatInitializer[] stats;

		[SerializeField]
		private StatManager statManager;

		[SerializeField]
		private GlobalStatView view;

		public StatManager StatManager => statManager;

		private void Awake()
		{
			statManager.Initialize(stats);
		}

		public void RefreshView()
		{
			view.Initialize(this);
		}
	}
}
