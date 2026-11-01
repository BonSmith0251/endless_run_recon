using System;

namespace Monoblok.Core
{
	public interface ISequenceElement
	{
		event Action OnComplete;

		void Enter();

		void Exit();
	}
}
