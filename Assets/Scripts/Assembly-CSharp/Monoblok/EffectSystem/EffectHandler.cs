using System;
using System.Collections.Generic;
using Monoblok.DamageSystem;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.EffectSystem
{
	public class EffectHandler : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer[] affectedRenderers;

		[SerializeField]
		private StatManager statManager;

		[SerializeField]
		private Health health;

		private readonly List<(EffectInstance effect, EffectVisual visual)> effects = new List<(EffectInstance, EffectVisual)>();

		private readonly Queue<Action> pendingActions = new Queue<Action>();

		private bool isUpdating;

		private EffectContext ctx;

		public SpriteRenderer[] AffectedRenderers => affectedRenderers;

		private void Awake()
		{
			ctx = new EffectContext
			{
				StatManager = statManager,
				Health = health
			};
		}

		public void AddEffect<TPayload>(EffectDefinition<TPayload> definition, object source, TPayload payload)
		{
			if (isUpdating)
			{
				pendingActions.Enqueue(delegate
				{
					AddEffect(definition, source, payload);
				});
			}
			else if (definition.StackingStrategy.TryApply(definition, GetEffectInstances(), ctx, source) != StackResult.Stacked)
			{
				EffectInstance effectInstance = definition.CreateInstance(source, payload);
				EffectVisual effectVisual = ((definition.VisualData == null) ? null : definition.VisualData.GetVisual());
				effects.Add((effectInstance, effectVisual));
				effectInstance.OnApply(ctx);
				effectVisual?.Apply(this);
			}
		}

		public void RemoveFromSource(object source)
		{
			if (isUpdating)
			{
				pendingActions.Enqueue(delegate
				{
					RemoveFromSource(source);
				});
			}
			else
			{
				ProcessRemoveFromSource(source);
			}
		}

		private void ProcessRemoveFromSource(object source)
		{
			for (int num = effects.Count - 1; num >= 0; num--)
			{
				if (effects[num].effect.Source == source)
				{
					effects[num].effect.OnRemove(ctx);
					effects[num].visual?.Remove(this);
					SwapRemoveAt(num);
				}
			}
		}

		public void ClearEffects()
		{
			if (isUpdating)
			{
				pendingActions.Enqueue(ClearEffects);
				return;
			}
			foreach (var effect in effects)
			{
				effect.effect.OnRemove(ctx);
				effect.visual?.Remove(this);
			}
			effects.Clear();
			pendingActions.Clear();
		}

		private void Update()
		{
			isUpdating = true;
			for (int num = effects.Count - 1; num >= 0; num--)
			{
				EffectInstance item = effects[num].effect;
				item.OnUpdate(ctx, Time.deltaTime);
				if (item.RemainingTime != -1f)
				{
					item.RemainingTime -= Time.deltaTime;
					if (!(item.RemainingTime > 0f))
					{
						item.OnRemove(ctx);
						effects[num].visual?.Remove(this);
						SwapRemoveAt(num);
					}
				}
			}
			isUpdating = false;
			while (pendingActions.Count > 0)
			{
				pendingActions.Dequeue()();
			}
		}

		private void SwapRemoveAt(int i)
		{
			effects[i] = effects[effects.Count - 1];
			effects.RemoveAt(effects.Count - 1);
		}

		private IEnumerable<EffectInstance> GetEffectInstances()
		{
			foreach (var effect in effects)
			{
				yield return effect.effect;
			}
		}
	}
}
