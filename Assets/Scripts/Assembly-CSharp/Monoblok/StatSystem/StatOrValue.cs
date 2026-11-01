using System;
using UnityEngine;

namespace Monoblok.StatSystem
{
	[Serializable]
	public class StatOrValue
	{
		[SerializeField]
		private float baseValue;

		[SerializeField]
		private StatDefinition statDefinition;

		private Stat stat;

		private Action<float> actionOnStatChange;

		public bool IsBound => stat != null;

		public float Value
		{
			get
			{
				if (stat == null)
				{
					return baseValue;
				}
				return stat.Value;
			}
		}

		public StatOrValue Clone()
		{
			return new StatOrValue
			{
				baseValue = baseValue,
				statDefinition = statDefinition,
				stat = stat
			};
		}

		public void Bind(StatManager statManager, Action<float> actionOnStatChange = null)
		{
			if (!(statManager == null) && !(statDefinition == null) && statManager.TryGetStat(statDefinition, out var stat))
			{
				this.actionOnStatChange = actionOnStatChange;
				this.stat = stat;
				this.stat.OnValueChanged -= HandleValueChanged;
				this.stat.OnValueChanged += HandleValueChanged;
			}
		}

		private void HandleValueChanged(float value)
		{
			actionOnStatChange?.Invoke(value);
		}

		public void SetBaseValue(float value)
		{
			if (IsBound)
			{
				throw new InvalidOperationException("Cannot set base value: " + statDefinition.name + " is already bound to a stat.");
			}
			baseValue = value;
		}

		public static implicit operator float(StatOrValue sov)
		{
			return sov.Value;
		}
	}
}
