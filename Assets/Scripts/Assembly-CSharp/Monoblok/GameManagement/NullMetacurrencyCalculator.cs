using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok.GameManagement
{
	[CreateAssetMenu(menuName = "Game/Meta Currency Calculator/Null")]
	public class NullMetacurrencyCalculator : MetaCurrencyCalculatorSO
	{
		public override Price Calculate()
		{
			return default(Price);
		}
	}
}
