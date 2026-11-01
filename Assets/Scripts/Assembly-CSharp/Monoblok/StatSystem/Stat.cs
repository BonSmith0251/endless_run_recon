using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.StatSystem
{
	public class Stat : IDisposable
	{
		private readonly ValueOrRuntimeValue<float> baseValue;

		private float? baseValueOverride;

		public readonly StatDefinition definition;

		private List<StatModifier> modifiers = new List<StatModifier>();

		private float value;

		public float BaseValue => baseValueOverride ?? baseValue.Value;

		public float Value => value;

		public bool IsAtMax => value >= definition.MaxValue;

		public event Action<float> OnValueChanged;

		public Stat(StatDefinition definition, ValueOrRuntimeValue<float> baseValue)
		{
			this.definition = definition;
			this.baseValue = baseValue.Clone();
			this.baseValue.Subscribe(OnBaseValueChanged);
			value = baseValue.Value;
		}

		public Stat(StatDefinition definition, float baseValue)
		{
			this.definition = definition;
			this.baseValue = new ValueOrRuntimeValue<float>(baseValue);
			this.baseValue.Subscribe(OnBaseValueChanged);
			value = this.baseValue.Value;
		}

		private void OnBaseValueChanged(float newBaseValue)
		{
			RecalculateValue();
		}

		public void OverrideBaseValue(float newBaseValue)
		{
			baseValueOverride = newBaseValue;
			RecalculateValue();
		}

		public void AddModifier(StatModifier modifier)
		{
			modifiers.Add(modifier);
			RecalculateValue();
		}

		public void RemoveModifiersFromSource(object source)
		{
			modifiers.RemoveAll((StatModifier m) => m.Source == source);
			RecalculateValue();
		}

		private void RecalculateValue()
		{
			float num = value;
			modifiers.Sort((StatModifier a, StatModifier b) => a.Order.CompareTo(b.Order));
			float a2 = BaseValue;
			foreach (StatModifier modifier in modifiers)
			{
				a2 = modifier.Modify(a2);
			}
			value = Mathf.Min(a2, definition.MaxValue);
			if (num != value)
			{
				this.OnValueChanged?.Invoke(value);
			}
		}

		public void Dispose()
		{
			baseValue.Unsubscribe(OnBaseValueChanged);
		}
	}
}
