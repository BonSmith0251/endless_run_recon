using System;

namespace Monoblok.GameManagement
{
	public interface IWinOrLoseCondition
	{
		event Action OnConditionMet;

		void Initialize();

		void Dispose();
	}
}
