using System;
using System.Collections.Generic;
using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Train
{
	public class BufferModuleBehavior : ModuleBehavior
	{
		[SerializeField]
		private BufferRuleManagerViewRuntimeValue rendererRuntimeValue;

		private IEnumerable<ICartBufferRule> bufferRules;

		private List<Stat> modifiedStats = new List<Stat>();

		private List<(StatManager manager, Action<Stat> handler)> statSubscriptions = new List<(StatManager, Action<Stat>)>();

		public IEnumerable<ICartBufferRule> BufferRules => bufferRules;

		public void Initialize(Sprite sprite, IEnumerable<ICartBufferRule> bufferRules)
		{
			this.bufferRules = bufferRules;
			spriteRenderer.sprite = sprite;
		}

		protected override void OnAttached()
		{
			base.Slot.Cart.Train.OnCartsChanged += OnCartsChanged;
		}

		protected override void OnDetached()
		{
			base.Slot.Cart.Train.OnCartsChanged -= OnCartsChanged;
		}

		public override void OnCartTargetStarted()
		{
			if (!(rendererRuntimeValue == null))
			{
				rendererRuntimeValue.Value.Register(this);
			}
		}

		public override void OnCartTargetEnded()
		{
			if (!(rendererRuntimeValue == null))
			{
				rendererRuntimeValue.Value.Unregister(this);
			}
		}

		protected override void OnModuleEnabled()
		{
			ApplyBuffs();
		}

		protected override void OnModuleDisabled()
		{
			RemoveBuffs();
		}

		private void OnCartsChanged()
		{
			RemoveBuffs();
			ApplyBuffs();
		}

		private void ApplyBuffs()
		{
			if (!base.ModuleEnabled)
			{
				return;
			}
			foreach (ICartBufferRule bufferRule in bufferRules)
			{
				foreach (TrainCart cart in bufferRule.TargetProvider.GetCarts(base.Slot.Cart))
				{
					if (cart == null)
					{
						continue;
					}
					StatManager[] componentsInChildren = cart.GetComponentsInChildren<StatManager>();
					foreach (StatManager statManager in componentsInChildren)
					{
						if (statManager.TryGetStat(bufferRule.StatDefinition, out var stat))
						{
							ApplyBuffToStat(bufferRule, stat);
						}
						SubscribeToFutureStats(bufferRule, statManager);
					}
				}
			}
		}

		private void ApplyBuffToStat(ICartBufferRule buffer, Stat stat)
		{
			float magnitude = buffer.ModifierFactory.GetMagnitude();
			StatModifier scaledModifier = buffer.ModifierFactory.GetScaledModifier(this, magnitude);
			stat.AddModifier(scaledModifier);
			modifiedStats.Add(stat);
		}

		private void SubscribeToFutureStats(ICartBufferRule buffer, StatManager statManager)
		{
			statManager.OnStatRegistered += Handler;
			statSubscriptions.Add((statManager, Handler));
			void Handler(Stat stat)
			{
				if ((object)stat.definition == buffer.StatDefinition)
				{
					ApplyBuffToStat(buffer, stat);
				}
			}
		}

		private void RemoveBuffs()
		{
			foreach (var (statManager, value) in statSubscriptions)
			{
				if (!(statManager == null))
				{
					statManager.OnStatRegistered -= value;
				}
			}
			statSubscriptions.Clear();
			foreach (Stat modifiedStat in modifiedStats)
			{
				modifiedStat?.RemoveModifiersFromSource(this);
			}
			modifiedStats.Clear();
		}
	}
}
