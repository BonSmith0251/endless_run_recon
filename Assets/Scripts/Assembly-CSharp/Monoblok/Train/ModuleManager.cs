using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using PrimeTween;
using UnityEngine;
using ZLinq;
using ZLinq.Linq;

namespace Monoblok.Train
{
	public class ModuleManager : MonoBehaviour
	{
		[SerializeField]
		private TrainCart cart;

		[SerializeField]
		private EventReference addModuleRef;

		private bool showVisuals;

		private List<ModuleSlot> slots = new List<ModuleSlot>();

		public IReadOnlyList<ModuleSlot> Slots => slots;

		public IEnumerable<ModuleData> Modules => from s in slots
			where s.ModuleData != null
			select s.ModuleData;

		public void UpdateSortingOrder()
		{
			foreach (ModuleSlot slot in slots)
			{
				if (!slot.IsFree)
				{
					slot.Behavior.UpdateSortingOrder();
				}
			}
		}

		public void Initialize(IEnumerable<Vector2> slotLocalPositions, bool showVisuals = true)
		{
			ValueEnumerable<OrderBy<FromEnumerable<Vector2>, Vector2, float>, Vector2> valueEnumerable = from p in slotLocalPositions.AsValueEnumerable()
				orderby p.x
				select p;
			int num = 0;
			using (ValueEnumerator<OrderBy<FromEnumerable<Vector2>, Vector2, float>, Vector2> valueEnumerator = ValueEnumerableExtensions.GetEnumerator(in valueEnumerable))
			{
				while (valueEnumerator.MoveNext())
				{
					Vector2 current = valueEnumerator.Current;
					slots.Add(new ModuleSlot(cart, current, num));
					num++;
				}
			}
			this.showVisuals = showVisuals;
		}

		public bool TryAddModule(ModuleData data, bool silent = false, bool animate = true)
		{
			foreach (ModuleSlot slot in slots)
			{
				if (!slot.IsFree)
				{
					continue;
				}
				slot.AddModule(data, animate, showVisuals);
				if (!silent)
				{
					Sequence.Create().ChainDelay(0.15f).ChainCallback(delegate
					{
						RuntimeManager.PlayOneShot(addModuleRef);
					});
				}
				slot.Behavior.StatManager.AddGlobalModifier(cart.CartData, cart.CartData.ModuleStatModifierFactory);
				cart.Train.Refresh();
				return true;
			}
			return false;
		}

		public void EnableModules(bool enabled)
		{
			foreach (ModuleSlot slot in slots)
			{
				ModuleBehavior behavior = slot.Behavior;
				if (!(behavior == null))
				{
					behavior.SetEnabled(enabled);
				}
			}
		}

		public void DetachAll(out List<ModuleData> modules)
		{
			modules = new List<ModuleData>();
			foreach (ModuleSlot slot in slots)
			{
				if (!slot.IsFree)
				{
					ModuleData moduleData = slot.ModuleData;
					slot.RemoveModule();
					modules.Add(moduleData);
				}
			}
		}
	}
}
