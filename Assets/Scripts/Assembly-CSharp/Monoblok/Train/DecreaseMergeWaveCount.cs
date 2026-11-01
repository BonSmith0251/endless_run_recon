using System;
using UnityEngine;

namespace Monoblok.Train
{
	[Serializable]
	public class DecreaseMergeWaveCount : ITemporaryModuleAction
	{
		public void Execute(TrainCart cart)
		{
			CartMergeConnection mergeConnection = cart.Unit.mergeConnection;
			if (mergeConnection != null)
			{
				int value = Mathf.Max(mergeConnection.WavesRemaining.Value - 1, 1);
				mergeConnection.WavesRemaining.SetValue(value);
			}
		}
	}
}
