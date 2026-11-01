using System;
using System.Collections.Generic;
using Monoblok.Core;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class LevelUpgradeManagerView : MonoBehaviour
	{
		[SerializeField]
		private LevelupRewardView upgradeViewPrefab;

		[SerializeField]
		private RectTransform container;

		[SerializeField]
		private CanvasGroup canvasGroup;

		private List<LevelupRewardView> currentViews = new List<LevelupRewardView>();

		public event Action<LevelupRewardView> OnRewardSelected;

		private void OnValidate()
		{
			if (canvasGroup == null)
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
		}

		public void Initialize(RewardOffer offer)
		{
			Clear();
			InitializeNewRewards(offer);
		}

		private void InitializeNewRewards(RewardOffer offer)
		{
			if (offer.NewRewards != null)
			{
				LevelupReward[] newRewards = offer.NewRewards;
				foreach (LevelupReward reward in newRewards)
				{
					LevelupRewardView levelupRewardView = UnityEngine.Object.Instantiate(upgradeViewPrefab, container);
					levelupRewardView.Initialize(reward);
					levelupRewardView.OnUpgradeSelected += HandleUpgradeSelected;
					currentViews.Add(levelupRewardView);
				}
			}
		}

		public void Show()
		{
			canvasGroup.alpha = 1f;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}

		public void Hide()
		{
			canvasGroup.alpha = 0f;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}

		public void Clear()
		{
			foreach (LevelupRewardView currentView in currentViews)
			{
				currentView.OnUpgradeSelected -= HandleUpgradeSelected;
			}
			container.Clear();
			currentViews.Clear();
		}

		private void HandleUpgradeSelected(LevelupRewardView view)
		{
			this.OnRewardSelected?.Invoke(view);
		}
	}
}
