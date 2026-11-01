using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Monoblok.Inventory;
using Monoblok.SA;
using Monoblok.Train;
using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok
{
	public class CartTradeManager : MonoBehaviour
	{
		[SerializeField]
		private CartTradeOfferFactory offerFactory;

		[SerializeField]
		private CartTradeView view;

		[SerializeField]
		private CartSelector cartSelector;

		[SerializeField]
		private AbilityManager abilityManager;

		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private CurrencyDefinition currency;

		[SerializeField]
		private GameEvent tradeEvent;

		[SerializeField]
		private EventReference tradeSfxRef;

		private List<CartTradeReward> selectedRewards = new List<CartTradeReward>();

		private Dictionary<TrainCart, CartTradeReward[]> offerCache = new Dictionary<TrainCart, CartTradeReward[]>();

		private TrainCart currentCart;

		private TradeContext ctx;

		public event Action OnTradeComplete;

		private void OnEnable()
		{
			view.OnSelected += HandleSelected;
			view.OnCanceled += HandleCanceled;
		}

		private void OnDisable()
		{
			view.OnSelected -= HandleSelected;
			view.OnCanceled -= HandleCanceled;
		}

		private void Awake()
		{
			ctx = new TradeContext
			{
				abilityManager = abilityManager
			};
		}

		public bool CanTradeCart(TrainCart cart)
		{
			if (cart.CartData.NextTier == null)
			{
				return cart.CartData.CanMerge;
			}
			return false;
		}

		public void InitializeTrade(TrainCart cart)
		{
			if (cart == null || !CanTradeCart(cart))
			{
				return;
			}
			PruneDeadOfferCacheEntries();
			cartSelector.DisableSelection();
			ctx.target = cart;
			ctx.train = cart.Train;
			currentCart = cart;
			int amount = cart.ModuleManager.Modules.Select((ModuleData m) => m.Worth.Amount.Value).Sum();
			ctx.basePrice = new Price
			{
				Currency = currency,
				Amount = amount
			};
			if (!offerCache.TryGetValue(cart, out var value))
			{
				value = offerFactory.GetRewards(ctx);
				offerCache[cart] = value;
			}
			else
			{
				CartTradeReward[] array = value;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RefreshPrice(ctx.basePrice);
				}
			}
			view.Initialize(value);
		}

		private void PruneDeadOfferCacheEntries()
		{
			List<TrainCart> list = null;
			foreach (TrainCart key in offerCache.Keys)
			{
				if (!(key != null))
				{
					if (list == null)
					{
						list = new List<TrainCart>();
					}
					list.Add(key);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (TrainCart item in list)
			{
				offerCache.Remove(item);
			}
		}

		public void RemoveAllRewards()
		{
			foreach (CartTradeReward selectedReward in selectedRewards)
			{
				selectedReward?.UnDo();
			}
			selectedRewards.Clear();
			offerCache.Clear();
		}

		public List<CartTradeRewardSaveData> GetSaveData()
		{
			List<CartTradeRewardSaveData> list = new List<CartTradeRewardSaveData>();
			foreach (CartTradeReward selectedReward in selectedRewards)
			{
				if (selectedReward != null)
				{
					list.Add(new CartTradeRewardSaveData
					{
						id = selectedReward.Config.ID
					});
				}
			}
			return list;
		}

		public void LoadSaveData(List<CartTradeRewardSaveData> data)
		{
			selectedRewards.Clear();
			if (data == null)
			{
				return;
			}
			foreach (CartTradeRewardSaveData datum in data)
			{
				CartTradeRewardConfig rewardConfigById = offerFactory.GetRewardConfigById(datum.id);
				if (!(rewardConfigById == null))
				{
					CartTradeReward reward = rewardConfigById.GetReward(default(Price));
					reward.Do();
					selectedRewards.Add(reward);
				}
			}
		}

		private void HandleSelected(CartTradeRewardView rewardView)
		{
			if (currentCart == null)
			{
				this.OnTradeComplete?.Invoke();
				cartSelector.EnableSelection();
				return;
			}
			Monoblok.Train.Train train = currentCart.Train;
			offerCache.Remove(currentCart);
			train.RemoveCart(currentCart);
			cartSelector.ForceSelection(train.Locomotive);
			CartTradeReward reward = rewardView.Reward;
			reward.Do();
			wallet.Gain(rewardView.Reward.Price);
			selectedRewards.Add(reward);
			view.Clear();
			view.Hide();
			currentCart = null;
			cartSelector.EnableSelection();
			tradeEvent.TriggerGameEvent();
			RuntimeManager.PlayOneShot(tradeSfxRef);
			this.OnTradeComplete?.Invoke();
		}

		private void HandleCanceled()
		{
			currentCart = null;
			view.Clear();
			view.Hide();
			cartSelector.EnableSelection();
		}
	}
}
