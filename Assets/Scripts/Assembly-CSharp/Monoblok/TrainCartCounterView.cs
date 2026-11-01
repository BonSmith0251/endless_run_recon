using Monoblok.SA;
using Monoblok.Train;
using TMPro;
using UnityEngine;

namespace Monoblok
{
	public class TrainCartCounterView : MonoBehaviour
	{
		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		private TMP_Text counterText;

		// Retain the exact publisher: the runtime-value asset can reset before UI teardown.
		private Monoblok.Train.Train subscribedTrain;

		private void OnEnable()
		{
			subscribedTrain = trainRuntimeValue.Value;
			subscribedTrain.OnCartAdded += HandleCartAdded;
			subscribedTrain.OnCartRemoved += HandleCartRemoved;
			subscribedTrain.OnMaxCartsChanged += HandleMaxCartsChanged;
			UpdateText();
		}

		private void OnDisable()
		{
			if (ReferenceEquals(subscribedTrain, null)) return;
			subscribedTrain.OnCartAdded -= HandleCartAdded;
			subscribedTrain.OnCartRemoved -= HandleCartRemoved;
			subscribedTrain.OnMaxCartsChanged -= HandleMaxCartsChanged;
			subscribedTrain = null;
		}

		private void HandleMaxCartsChanged(int maxCarts)
		{
			UpdateText();
		}

		private void HandleCartAdded(TrainCart cart)
		{
			UpdateText();
		}

		private void HandleCartRemoved(TrainCart cart)
		{
			UpdateText();
		}

		private void UpdateText()
		{
			Monoblok.Train.Train value = trainRuntimeValue.Value;
			counterText.text = value.CartCount + "/" + value.MaxCartCount;
		}
	}
}
