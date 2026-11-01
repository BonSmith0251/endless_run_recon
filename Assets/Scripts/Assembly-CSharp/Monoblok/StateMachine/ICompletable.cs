using System;

namespace Monoblok.StateMachine
{
	public interface ICompletable
	{
		event Action OnComplete;

		void Initialize();
	}
}
