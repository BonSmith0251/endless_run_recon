using System;

namespace Monoblok.GameManagement
{
	public abstract class WinOrLoseCondition : IWinOrLoseCondition
	{
		private bool completed;

		public bool Completed => completed;

		public event Action OnConditionMet;

		public abstract void Initialize();

		public abstract void Dispose();

		protected void NotifyConditionMet()
		{
			if (!completed)
			{
				completed = true;
				this.OnConditionMet?.Invoke();
			}
		}
	}
}
