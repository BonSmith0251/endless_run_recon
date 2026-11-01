using Monoblok.Train;
using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok.GameManagement
{
	[CreateAssetMenu(menuName = "Game/Meta Currency Calculator/Train Level To Meta Currency")]
	public class TrainLevelToMetaCurrency : MetaCurrencyCalculatorSO
	{
		[SerializeField]
		private TrainExperience trainXP;

		[SerializeField]
		private float multiplier;

		public override Price Calculate()
		{
			int amount = Mathf.RoundToInt((float)trainXP.CurrentLevel * multiplier);
			Price result = default(Price);
			result.Currency = currency;
			result.Amount = amount;
			return result;
		}
	}
}
