using System;
using Monoblok.Core;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class NotifyTrain : ISequenceElement
	{
		private readonly Monoblok.Train.Train train;

		public event Action OnComplete;

		public NotifyTrain(Monoblok.Train.Train train)
		{
			this.train = train;
		}

		public void Enter()
		{
			Debug.Log("Waiting for train...");
			WaitAndComplete();
		}

		private async void WaitAndComplete()
		{
			await train.OnArrivedToStation();
			this.OnComplete?.Invoke();
		}

		public void Exit()
		{
			Debug.Log("Train complete");
		}
	}
}
