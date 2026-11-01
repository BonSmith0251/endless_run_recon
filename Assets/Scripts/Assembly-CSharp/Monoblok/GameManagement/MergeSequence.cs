using System;
using Monoblok.Core;
using Monoblok.Train;
using PrimeTween;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class MergeSequence : ISequenceElement
	{
		private readonly CartMergeManager mergeManager;

		private float delay;

		public event Action OnComplete;

		public MergeSequence(CartMergeManager mergeManager)
		{
			this.mergeManager = mergeManager;
		}

		public void Enter()
		{
			Debug.Log("Waiting for merge sequence...");
			mergeManager.OnMergeSequenceComplete += HandleMergeSequenceComplete;
			delay = ((mergeManager.MergeConnections.Count > 0) ? 0.5f : 0f);
			mergeManager.EvaluateActiveMergeConnections();
		}

		private void HandleMergeSequenceComplete()
		{
			Tween.Delay(delay).OnComplete(this, delegate(MergeSequence t)
			{
				t.OnComplete?.Invoke();
			});
		}

		public void Exit()
		{
			mergeManager.OnMergeSequenceComplete -= HandleMergeSequenceComplete;
			Debug.Log("Merge sequence complete");
		}
	}
}
