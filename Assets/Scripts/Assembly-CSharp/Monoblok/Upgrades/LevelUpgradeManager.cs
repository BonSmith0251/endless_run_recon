using System.Collections.Generic;
using Monoblok.Train;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class LevelUpgradeManager : MonoBehaviour
	{
		[SerializeField]
		private TrainExperience experience;

		[SerializeField]
		[Expandable]
		private LevelUpgradeFactory upgradeFactory;

		[SerializeField]
		private LevelUpgradeManagerView managerView;

		[SerializeField]
		private ScreenSizeKnockback knockback;

		[SerializeField]
		private int offerCount = 3;

		private List<LevelupReward> activeRewards = new List<LevelupReward>();

		private void OnEnable()
		{
			experience.OnLevelUp += OnLevelUp;
			managerView.OnRewardSelected += HandleRewardSelected;
		}

		private void OnDisable()
		{
			experience.OnLevelUp -= OnLevelUp;
			managerView.OnRewardSelected -= HandleRewardSelected;
		}

		private void OnLevelUp()
		{
			Sequence.Create(1, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true).ChainCallback(knockback, delegate(ScreenSizeKnockback t)
			{
				t.TriggerKnockback();
			}).ChainDelay(0.5f)
				.Chain(Tween.GlobalTimeScale(0f, 0.3f, Ease.OutCirc))
				.OnComplete(this, delegate(LevelUpgradeManager t)
				{
					t.OfferRewards();
				});
		}

		private void OfferRewards()
		{
			LevelupReward[] array = null;
			LevelupReward[] unique = upgradeFactory.GetUnique(offerCount - ((array != null) ? array.Length : 0));
			RewardOffer offer = new RewardOffer.Builder().WithNewRewards(unique).WithUpgrades(array).Build();
			managerView.Initialize(offer);
			managerView.Show();
		}

		private void HandleRewardSelected(LevelupRewardView view)
		{
			GainReward(view);
			managerView.Hide();
			Tween.GlobalTimeScale(1f, 0.3f, Ease.OutCirc);
		}

		private void GainReward(LevelupRewardView view)
		{
			if (!(view == null))
			{
				LevelupReward reward = view.Reward;
				if (reward != null)
				{
					reward.Gain();
					activeRewards.Add(reward);
				}
			}
		}
	}
}
