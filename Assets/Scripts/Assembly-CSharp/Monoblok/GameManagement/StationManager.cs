using System.Collections.Generic;
using Monoblok.Core;
using Monoblok.StatSystem;
using Monoblok.Train;
using Monoblok.Upgrades;
using PrimeTween;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class StationManager : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Train.Train train;

		[SerializeField]
		private Shop shop;

		[SerializeField]
		private CartSelector cartSelector;

		[SerializeField]
		private CartInspector cartInspector;

		[SerializeField]
		private GlobalStatManager globalStatManager;

		[SerializeField]
		private StationView stationView;

		[SerializeField]
		private CartMergeManager cartMergeManager;

		private Monoblok.Core.Sequence stationSequence;

		private void Awake()
		{
			List<ISequenceElement> elements = new List<ISequenceElement>
			{
				new NotifyTrain(train),
				new MergeSequence(cartMergeManager)
			};
			stationSequence = new Monoblok.Core.Sequence(elements);
		}

		public void EnterStation()
		{
			stationSequence.OnComplete -= OpenStation;
			stationSequence.OnComplete += OpenStation;
			stationSequence.Run();
		}

		private void OpenStation()
		{
			cartSelector.EnableSelection();
			shop.OpenShop();
			stationView.Show();
		}

		public Tween ExitStation()
		{
			cartSelector.DisableSelection();
			Tween tween = shop.CloseShop();
			Tween tween2 = stationView.Hide();
			return Tween.Delay(tween.duration + tween2.duration);
		}
	}
}
