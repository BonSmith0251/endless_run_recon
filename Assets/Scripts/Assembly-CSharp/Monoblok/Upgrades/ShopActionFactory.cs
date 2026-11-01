using System;
using System.Collections.Generic;
using System.Linq;
using Monoblok.Inventory;
using Monoblok.Train;
using UnityEngine;
using UnityEngine.Localization;
using ZLinq;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Shop/Action Factory")]
	public class ShopActionFactory : ScriptableObject
	{
		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private LocalizedString insufficientFundsMessage;

		[Header("Modules")]
		[SerializeField]
		private ModuleDatabase moduleDatabase;

		[SerializeField]
		[Min(0f)]
		private float affordableWeightMultiplier = 2f;

		[SerializeField]
		[Min(0f)]
		private float unaffordableWeightMultiplier = 0.25f;

		[SerializeField]
		[Min(0.01f)]
		[Tooltip("Falloff range as a fraction of the player's wallet amount, e.g. 1 = a module costing 2x the wallet amount is fully saturated.")]
		private float affordabilityFalloffPercent = 1f;

		[SerializeField]
		[Min(0.01f)]
		[Tooltip("Floor for the falloff range so early-game (near-zero wallet) pricing still has a gradient instead of an immediate cliff.")]
		private float minimumFalloffRange = 50f;

		[SerializeField]
		private LocalizedString noFreeSlotsMessage;

		[Header("Static Actions")]
		[SerializeField]
		private List<ShopActionConfig> staticActionConfigs;

		public LocalizedString InsufficientFundsMessage => insufficientFundsMessage;

		private List<ModuleDefinition> OfferedModules => moduleDatabase.Modules.Where((ModuleDefinition m) => m.IsOffered).ToList();

		public List<AddModuleAction> GetModuleActions(ShopActionContext ctx, int count, ModuleRollState rollState)
		{
			List<ModuleDefinition> offeredModules = OfferedModules;
			List<ModuleDefinition> eligible = offeredModules.Where((ModuleDefinition m) => m.IsEligibleToRoll(ctx)).ToList();
			if (eligible.Count < count)
			{
				int count2 = count - eligible.Count;
				eligible.AddRange((from m in offeredModules.AsValueEnumerable()
					where !eligible.Contains(m)
					select m).Shuffle().Take(count2).ToArray());
			}
			List<ModuleDefinition> list = PickModules(eligible, offeredModules, count, rollState, GetWalletAmount);
			List<AddModuleAction> list2 = new List<AddModuleAction>();
			foreach (ModuleDefinition item in list)
			{
				list2.Add(new AddModuleAction(item, ctx, item.BasePrice, noFreeSlotsMessage));
			}
			return list2;
		}

		private int GetWalletAmount(ModuleDefinition module)
		{
			return wallet.GetCount(module.BasePrice.Currency);
		}

		private List<ModuleDefinition> PickModules(List<ModuleDefinition> initialPool, List<ModuleDefinition> fullPool, int count, ModuleRollState rollState, Func<ModuleDefinition, int> walletAmount)
		{
			List<ModuleDefinition> list = initialPool;
			List<ModuleDefinition> list2 = new List<ModuleDefinition>();
			HashSet<ModuleDefinition> hashSet = new HashSet<ModuleDefinition>();
			while (list2.Count < count)
			{
				if (list.Count == 0)
				{
					list = fullPool.ToList();
				}
				ModuleDefinition item = WeightedPick(list, rollState, walletAmount);
				list2.Add(item);
				hashSet.Add(item);
				list.Remove(item);
			}
			foreach (ModuleDefinition item2 in fullPool)
			{
				if (hashSet.Contains(item2))
				{
					rollState.OnOffered(item2);
				}
				else
				{
					rollState.OnNotOffered(item2);
				}
			}
			return list2;
		}

		private ModuleDefinition WeightedPick(List<ModuleDefinition> candidates, ModuleRollState rollState, Func<ModuleDefinition, int> walletAmount)
		{
			float num = 0f;
			foreach (ModuleDefinition candidate in candidates)
			{
				num += GetAffordanceAdjustedWeight(candidate, rollState, walletAmount(candidate));
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			float num3 = 0f;
			foreach (ModuleDefinition candidate2 in candidates)
			{
				num3 += GetAffordanceAdjustedWeight(candidate2, rollState, walletAmount(candidate2));
				if (num2 <= num3)
				{
					return candidate2;
				}
			}
			return candidates[candidates.Count - 1];
		}

		private float GetAffordanceAdjustedWeight(ModuleDefinition module, ModuleRollState rollState, int walletAmount)
		{
			float weight = rollState.GetWeight(module);
			float num = module.BasePrice.Amount - walletAmount;
			float num2 = Mathf.Max((float)walletAmount * affordabilityFalloffPercent, minimumFalloffRange);
			float num3 = Mathf.Lerp(t: 1f - Mathf.Clamp01(num / num2), a: unaffordableWeightMultiplier, b: affordableWeightMultiplier);
			return weight * num3;
		}

		public List<ShopAction> GetStaticActions(ShopActionContext ctx)
		{
			List<ShopAction> list = new List<ShopAction>();
			foreach (ShopActionConfig staticActionConfig in staticActionConfigs)
			{
				list.Add(staticActionConfig.GetAction(ctx));
			}
			return list;
		}
	}
}
