using System.Collections.Generic;

namespace Monoblok.Train
{
	public interface ITrainCartTargetProvider
	{
		IEnumerable<TrainCart> GetCarts(TrainCart origin);

		string GetTargetPhrase();
	}
}
