using UnityEngine;

namespace Monoblok.GameManagement
{
	public class GameTimeManager : MonoBehaviour
	{
		private float timeSpent;

		private bool running;

		public float TimeSpent => timeSpent;

		public void ResetTime()
		{
			timeSpent = 0f;
		}

		public void StartTime()
		{
			running = true;
		}

		public void StopTime()
		{
			running = false;
		}

		private void Update()
		{
			if (running)
			{
				timeSpent += Time.deltaTime;
			}
		}

		public void LoadData(float time)
		{
			timeSpent = time;
		}

		public float SaveData()
		{
			return timeSpent;
		}
	}
}
