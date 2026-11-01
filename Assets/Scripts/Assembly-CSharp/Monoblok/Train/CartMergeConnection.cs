using System;
using Monoblok.Core;

namespace Monoblok.Train
{
	public class CartMergeConnection : IDisposable
	{
		private readonly Observable<int> wavesRemaining;

		private TrainUnit mergedUnit;

		public TrainCart Parent { get; }

		public TrainCart Child { get; }

		public TrainUnit MergedUnit => mergedUnit;

		public bool IsComplete => wavesRemaining.Value <= 0;

		public Observable<int> WavesRemaining => wavesRemaining;

		public CartMergeConnection(TrainCart parent, TrainCart child, int wavesRequired)
		{
			Parent = parent;
			Child = child;
			wavesRemaining = new Observable<int>(wavesRequired);
			mergedUnit = parent.Train.GroupUnits(parent.Unit, child.Unit);
			mergedUnit.mergeConnection = this;
		}

		public CartMergeConnection(TrainUnit existingUnit, TrainCart parent, TrainCart child, int wavesRemaining)
		{
			Parent = parent;
			Child = child;
			this.wavesRemaining = new Observable<int>(wavesRemaining);
			mergedUnit = existingUnit;
			mergedUnit.mergeConnection = this;
		}

		public void OnWaveCompleted()
		{
			int value = wavesRemaining.Value - 1;
			wavesRemaining.SetValue(value);
		}

		public void Dispose()
		{
		}
	}
}
