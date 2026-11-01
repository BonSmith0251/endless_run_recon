using Monoblok.Progression;
using UnityEngine;

namespace Monoblok
{
	// Offline build: achievements and stats were Steam services and are disabled. The serialized
	// sink and progression references are kept so the original data stays intact.
	public class AchievementManager : MonoBehaviour
	{
		[SerializeField]
		private AchievementSinkData[] sinkDatas;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		public void Initialize()
		{
		}
	}
}
