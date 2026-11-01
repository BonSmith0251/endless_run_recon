using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.StatSystem
{
	public class StatManager : MonoBehaviour
	{
		private Dictionary<StatDefinition, Stat> statMap = new Dictionary<StatDefinition, Stat>();

		private Dictionary<StatDefinition, int> refCounts = new Dictionary<StatDefinition, int>();

		private readonly Dictionary<object, StatModifier> globalModifiers = new Dictionary<object, StatModifier>();

		public IReadOnlyDictionary<StatDefinition, Stat> StatMap => statMap;

		public event Action<Stat> OnStatRegistered;

		private void OnDestroy()
		{
			foreach (Stat value in statMap.Values)
			{
				value?.Dispose();
			}
		}

		public void Initialize(IEnumerable<StatInitializer> initializers)
		{
			foreach (StatInitializer initializer in initializers)
			{
				RegisterStat(initializer);
			}
		}

		public void AddGlobalModifier(object source, StatModifierFactory factory)
		{
			if (factory == null)
			{
				return;
			}
			RemoveGlobalModifier(source);
			StatModifier modifier = factory.GetModifier(source);
			globalModifiers[source] = modifier;
			foreach (Stat value in StatMap.Values)
			{
				value.AddModifier(modifier);
			}
		}

		public void RemoveGlobalModifier(object source)
		{
			if (!globalModifiers.Remove(source))
			{
				return;
			}
			foreach (Stat value in StatMap.Values)
			{
				value.RemoveModifiersFromSource(source);
			}
		}

		public Stat RegisterStat(StatInitializer statInitializer)
		{
			StatDefinition statDefinition = statInitializer.StatDefinition;
			if (!CheckNotRegistered(statDefinition))
			{
				return null;
			}
			return AddStat(statDefinition, statInitializer.GetStat());
		}

		public Stat RegisterStat(StatDefinition definition, float baseValue)
		{
			if (!CheckNotRegistered(definition))
			{
				return null;
			}
			return AddStat(definition, new Stat(definition, baseValue));
		}

		public Stat RegisterOrUpdateStat(StatInitializer statInitializer)
		{
			StatDefinition statDefinition = statInitializer.StatDefinition;
			if (statMap.TryGetValue(statDefinition, out var value))
			{
				value.OverrideBaseValue(statInitializer.BaseValue);
				return value;
			}
			return AddStat(statDefinition, statInitializer.GetStat());
		}

		public Stat AcquireStat(StatInitializer statInitializer)
		{
			StatDefinition statDefinition = statInitializer.StatDefinition;
			if (statMap.TryGetValue(statDefinition, out var value))
			{
				refCounts[statDefinition]++;
				return value;
			}
			refCounts[statDefinition] = 1;
			return AddStat(statDefinition, statInitializer.GetStat());
		}

		private bool CheckNotRegistered(StatDefinition definition)
		{
			if (!statMap.ContainsKey(definition))
			{
				return true;
			}
			return false;
		}

		private Stat AddStat(StatDefinition definition, Stat stat)
		{
			statMap.Add(definition, stat);
			foreach (StatModifier value in globalModifiers.Values)
			{
				stat.AddModifier(value);
			}
			this.OnStatRegistered?.Invoke(stat);
			return stat;
		}

		public void ReleaseStat(StatDefinition definition)
		{
			if (!refCounts.TryGetValue(definition, out var value))
			{
				return;
			}
			value--;
			if (value > 0)
			{
				refCounts[definition] = value;
				return;
			}
			refCounts.Remove(definition);
			if (statMap.TryGetValue(definition, out var value2))
			{
				value2.Dispose();
			}
			statMap.Remove(definition);
		}

		public bool TryGetStat(StatDefinition definition, out Stat stat)
		{
			if (!statMap.TryGetValue(definition, out stat))
			{
				return false;
			}
			return true;
		}

		public float GetStatValue(StatDefinition defition)
		{
			if (!TryGetStat(defition, out var stat))
			{
				Debug.LogError("No stat found for " + defition.Name);
				return 0f;
			}
			return stat.Value;
		}
	}
}
