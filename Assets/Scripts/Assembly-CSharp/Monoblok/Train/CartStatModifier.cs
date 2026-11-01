using System;
using System.Collections.Generic;
using Monoblok.Progression;
using Monoblok.StatSystem;
using UnityEngine;
using ZLinq;
using ZLinq.Linq;

namespace Monoblok.Train
{
	[Serializable]
	public class CartStatModifier : ITemporaryModuleAction
	{
		private class Handle
		{
			private readonly Monoblok.Progression.Progression progression;

			private readonly IEnumerable<TrainCart> carts;

			private readonly StatModifierFactory modifierFactory;

			private int wavesRemaining;

			private List<Stat> modifiedStats = new List<Stat>();

			public Handle(Monoblok.Progression.Progression progression, IEnumerable<TrainCart> carts, StatModifierFactory modifierFactory, int waveCount)
			{
				this.progression = progression;
				this.carts = carts;
				this.modifierFactory = modifierFactory;
				wavesRemaining = waveCount;
			}

			public void Execute()
			{
				StatModifier modifier = modifierFactory.GetModifier(this);
				ValueEnumerable<SelectMany<FromEnumerable<TrainCart>, TrainCart, ModuleSlot>, ModuleSlot> valueEnumerable = carts.AsValueEnumerable().SelectMany((TrainCart c) => c.ModuleManager.Slots);
				using (ValueEnumerator<SelectMany<FromEnumerable<TrainCart>, TrainCart, ModuleSlot>, ModuleSlot> valueEnumerator = ValueEnumerableExtensions.GetEnumerator(in valueEnumerable))
				{
					while (valueEnumerator.MoveNext())
					{
						ModuleSlot current = valueEnumerator.Current;
						if (current.IsFree)
						{
							continue;
						}
						foreach (Stat value in current.Behavior.StatManager.StatMap.Values)
						{
							value.AddModifier(modifier);
							modifiedStats.Add(value);
						}
					}
				}
				progression.OnJourneyEnded += HandleJourneyEnded;
			}

			private void HandleJourneyEnded()
			{
				wavesRemaining--;
				if (wavesRemaining > 0)
				{
					return;
				}
				foreach (Stat modifiedStat in modifiedStats)
				{
					modifiedStat.RemoveModifiersFromSource(this);
				}
				modifiedStats.Clear();
				progression.OnJourneyEnded -= HandleJourneyEnded;
			}
		}

		[SerializeReference]
		[SubclassSelector]
		private ITrainCartTargetProvider targetProvider;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		[Min(1f)]
		private int waveCount = 1;

		[SerializeField]
		private StatModifierFactory modifierFactory;

		public void Execute(TrainCart cart)
		{
			IEnumerable<TrainCart> carts = targetProvider.GetCarts(cart);
			new Handle(progression, carts, modifierFactory, waveCount).Execute();
		}
	}
}
