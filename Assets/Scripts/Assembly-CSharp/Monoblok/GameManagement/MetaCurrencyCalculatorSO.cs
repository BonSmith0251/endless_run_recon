using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public abstract class MetaCurrencyCalculatorSO : ScriptableObject, IMetaCurrencyCalculator
	{
		[SerializeField]
		protected CurrencyDefinition currency;

		public CurrencyDefinition Currency => currency;

		public abstract Price Calculate();
	}
}
