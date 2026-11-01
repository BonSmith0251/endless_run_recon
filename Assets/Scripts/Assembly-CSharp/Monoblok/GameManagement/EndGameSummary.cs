using Monoblok.Upgrades;

namespace Monoblok.GameManagement
{
	public class EndGameSummary
	{
		public enum State
		{
			SoftWin,
			Lost
		}

		public State state;

		public int gainedLevels;

		public int stationsReached;

		public float timeSpent;

		public int enemiesDefeated;

		public int coinsCollected;

		public Price metaCurrency;
	}
}
