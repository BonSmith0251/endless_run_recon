using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	public class LevelupRewardView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text descriptionText;

		[SerializeField]
		private Image image;

		[SerializeField]
		private Button button;

		private LevelupReward reward;

		public LevelupReward Reward => reward;

		public event Action<LevelupRewardView> OnUpgradeSelected;

		public void Initialize(LevelupReward reward)
		{
			this.reward = reward;
			descriptionText.text = reward.Description;
			button.onClick.AddListener(HandleClick);
		}

		private void HandleClick()
		{
			this.OnUpgradeSelected?.Invoke(this);
		}
	}
}
