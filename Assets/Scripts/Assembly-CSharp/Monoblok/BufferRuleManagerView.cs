using System.Collections.Generic;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	public class BufferRuleManagerView : MonoBehaviour
	{
		[SerializeField]
		private WorldStatModifierContainer containerPrefab;

		[SerializeField]
		private RectTransform container;

		private Dictionary<BufferModuleBehavior, List<WorldStatModifierContainer>> containerMap = new Dictionary<BufferModuleBehavior, List<WorldStatModifierContainer>>();

		public void Register(BufferModuleBehavior module)
		{
			TrainCart cart = module.Slot.Cart;
			Dictionary<TrainCart, WorldStatModifierContainer> dictionary = new Dictionary<TrainCart, WorldStatModifierContainer>();
			List<WorldStatModifierContainer> list = new List<WorldStatModifierContainer>();
			foreach (ICartBufferRule bufferRule in module.BufferRules)
			{
				foreach (TrainCart cart2 in bufferRule.TargetProvider.GetCarts(cart))
				{
					if (!(cart2 == null))
					{
						if (!dictionary.TryGetValue(cart2, out var value))
						{
							value = Object.Instantiate(containerPrefab, container);
							value.Initialize(cart2);
							dictionary[cart2] = value;
							list.Add(value);
						}
						value.AddView(bufferRule.StatDefinition, bufferRule.BuffOrDebuff);
					}
				}
			}
			containerMap.Add(module, list);
		}

		public void Unregister(BufferModuleBehavior module)
		{
			if (!containerMap.TryGetValue(module, out var value))
			{
				return;
			}
			foreach (WorldStatModifierContainer item in value)
			{
				Object.Destroy(item.gameObject);
			}
			containerMap.Remove(module);
		}
	}
}
