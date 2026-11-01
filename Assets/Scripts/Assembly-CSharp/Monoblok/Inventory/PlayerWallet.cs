using System;
using System.Collections.Generic;
using System.Linq;
using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok.Inventory
{
	[CreateAssetMenu(menuName = "Game/Inventory/Player Wallet")]
	public class PlayerWallet : ScriptableObject
	{
		private Dictionary<CurrencyDefinition, CurrencyContainer> currencyMap;

		public void Initialize(IEnumerable<CurrencyDefinition> definitions)
		{
			currencyMap = new Dictionary<CurrencyDefinition, CurrencyContainer>();
			foreach (CurrencyDefinition definition in definitions)
			{
				CurrencyContainer value = new CurrencyContainer(new Currency(definition));
				currencyMap[definition] = value;
			}
			Debug.Log("Initialized wallet");
		}

		public void AddCurrency(CurrencyDefinition def, int amount)
		{
			if (TryGetContainer(def, out var container))
			{
				int value = container.Count.Value + amount;
				container.Count.SetValue(value);
			}
		}

		public void RemoveCurrency(CurrencyDefinition def, int amount)
		{
			if (TryGetContainer(def, out var container))
			{
				int a = container.Count.Value - amount;
				a = Mathf.Max(a, 0);
				container.Count.SetValue(a);
			}
		}

		public void ResetCurrency(string displayName)
		{
			if (TryGetContainer(displayName, out var container))
			{
				container.Count.SetValue(0);
			}
		}

		public void ResetCurrency(CurrencyDefinition def)
		{
			if (TryGetContainer(def, out var container))
			{
				container.Count.SetValue(0);
				Debug.Log("Reset currency " + def.DisplayName);
			}
		}

		public void ResetAll()
		{
			foreach (KeyValuePair<CurrencyDefinition, CurrencyContainer> item in currencyMap)
			{
				item.Value.Count.SetValue(0);
			}
		}

		public void Subscribe(CurrencyDefinition def, Action<int> onCountChanged)
		{
			if (TryGetContainer(def, out var container))
			{
				container.Count.OnValueChanged += onCountChanged;
			}
		}

		public void Unsubscribe(CurrencyDefinition def, Action<int> onCountChanged)
		{
			if (TryGetContainer(def, out var container))
			{
				container.Count.OnValueChanged -= onCountChanged;
			}
		}

		public int GetCount(CurrencyDefinition def)
		{
			if (!TryGetContainer(def, out var container))
			{
				return 0;
			}
			return container.Count.Value;
		}

		public int GetCount(string displayName)
		{
			if (!TryGetContainer(displayName, out var container))
			{
				return 0;
			}
			return container.Count.Value;
		}

		public bool CanAfford(RuntimePrice price)
		{
			if (GetCount(price.Currency) < price.Amount.Value)
			{
				return false;
			}
			return true;
		}

		public void Spend(RuntimePrice price)
		{
			RemoveCurrency(price.Currency, price.Amount.Value);
		}

		public void Gain(RuntimePrice price)
		{
			AddCurrency(price.Currency, price.Amount.Value);
		}

		public void LoadSaveData(List<CurrencySaveData> data)
		{
			foreach (CurrencySaveData datum in data)
			{
				string id = datum.id;
				CurrencyContainer currencyContainer = currencyMap.Values.FirstOrDefault((CurrencyContainer c) => c.Currency.ID == id);
				if (currencyContainer == null)
				{
					Debug.LogError("Couldn't find currency with id " + id);
				}
				else
				{
					currencyContainer.Count.SetValue(datum.amount);
				}
			}
		}

		private bool TryGetContainer(CurrencyDefinition def, out CurrencyContainer container)
		{
			if (!currencyMap.TryGetValue(def, out container))
			{
				Debug.LogError("Trying to add " + def.DisplayName + " but it's not registered in inventory");
				return false;
			}
			return true;
		}

		private bool TryGetContainer(string displayName, out CurrencyContainer container)
		{
			container = currencyMap.Values.FirstOrDefault((CurrencyContainer c) => c.Currency.DisplayName == displayName);
			if (container == null)
			{
				Debug.LogError("Couldn't find currency with Display Name " + displayName);
				return false;
			}
			return true;
		}
	}
}
