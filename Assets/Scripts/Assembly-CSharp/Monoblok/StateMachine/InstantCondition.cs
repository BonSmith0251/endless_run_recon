using System;

namespace Monoblok.StateMachine
{
	public class InstantCondition : ICondition
	{
		public event Action OnMet;

		public void Initialize()
		{
			this.OnMet?.Invoke();
		}

		public void Dispose()
		{
		}
	}
}
