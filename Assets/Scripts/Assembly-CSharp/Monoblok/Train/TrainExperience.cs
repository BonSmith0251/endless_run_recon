using System;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Train/XP")]
	public class TrainExperience : ScriptableObject
	{
		[SerializeField]
		private int baseXPToNextLevel;

		[SerializeField]
		private float xpMultiplier;

		private int currentLevel;

		private int totalXP;

		private int currentLevelXP;

		private int xpToNextLevel;

		public int CurrentLevel => currentLevel;

		public int TotalXP => totalXP;

		public int CurrentLevelXP => currentLevelXP;

		public int XPToNextLevel => xpToNextLevel;

		public event Action OnXPGained;

		public event Action OnLevelUp;

		public void Initialize()
		{
			currentLevel = 0;
			totalXP = 0;
			currentLevelXP = 0;
			xpToNextLevel = baseXPToNextLevel;
			this.OnXPGained?.Invoke();
			this.OnLevelUp?.Invoke();
		}

		public void GainXP(int xp)
		{
			totalXP += xp;
			currentLevelXP += xp;
			this.OnXPGained?.Invoke();
			if (currentLevelXP >= xpToNextLevel)
			{
				LevelUp();
			}
		}

		public TrainXPSaveData GetSaveData()
		{
			return new TrainXPSaveData
			{
				currentLevel = currentLevel,
				totalXP = totalXP,
				currentLevelXP = currentLevelXP,
				xpToNextLevel = xpToNextLevel
			};
		}

		public void LoadSaveData(TrainXPSaveData data)
		{
			currentLevel = data.currentLevel;
			currentLevelXP = data.currentLevelXP;
			totalXP = data.totalXP;
			xpToNextLevel = data.xpToNextLevel;
		}

		private void LevelUp()
		{
			currentLevel++;
			currentLevelXP = 0;
			xpToNextLevel += Mathf.RoundToInt((float)baseXPToNextLevel * xpMultiplier * (float)currentLevel);
			this.OnLevelUp?.Invoke();
		}
	}
}
