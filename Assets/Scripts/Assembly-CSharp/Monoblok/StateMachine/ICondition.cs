using System;

namespace Monoblok.StateMachine
{
	public interface ICondition
	{
		event Action OnMet;

		void Initialize();

		void Dispose();
	}
}
