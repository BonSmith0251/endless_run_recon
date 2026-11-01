using System;
using System.Collections.Generic;
using System.Linq;
using Monoblok.StatSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Train
{
	public class TrainConfigSelectorView : MonoBehaviour
	{
		[SerializeField]
		private Button selectButton;

		[SerializeField]
		private Button upgradesButton;

		[SerializeField]
		private Button backButton;

		[SerializeField]
		private Button cycleLeft;

		[SerializeField]
		private Button cycleRight;

		[SerializeField]
		private TrainConfigView trainConfigView;

		[SerializeField]
		private TMP_Text locoNameText;

		[SerializeField]
		private StatInitializerCollectionView statCollectionView;

		private List<(TrainConfig config, bool isUnlocked)> pool;

		private int index;

		public event Action<TrainConfig, bool> OnConfigSelected;

		public event Action OnStartClicked;

		public event Action OnBackClicked;

		public event Action OnUpgradesClicked;

		private void OnEnable()
		{
			cycleLeft.onClick.AddListener(CyclePrevious);
			cycleRight.onClick.AddListener(CycleNext);
			selectButton.onClick.AddListener(HandleStartClicked);
			upgradesButton.onClick.AddListener(HandleUpgradesClicked);
			backButton.onClick.AddListener(HandleBackClicked);
		}

		private void OnDisable()
		{
			cycleLeft.onClick.RemoveListener(CyclePrevious);
			cycleRight.onClick.RemoveListener(CycleNext);
			selectButton.onClick.RemoveListener(HandleStartClicked);
			upgradesButton.onClick.RemoveListener(HandleUpgradesClicked);
			backButton.onClick.RemoveListener(HandleBackClicked);
		}

		private void HandleBackClicked()
		{
			this.OnBackClicked?.Invoke();
		}

		private void HandleStartClicked()
		{
			this.OnStartClicked?.Invoke();
		}

		private void HandleUpgradesClicked()
		{
			this.OnUpgradesClicked?.Invoke();
		}

		public void Initialize(IEnumerable<(TrainConfig config, bool isUnlocked)> pool)
		{
			this.pool = pool.ToList();
			index = 0;
			InitCurrent();
		}

		public void RefreshPool(IEnumerable<(TrainConfig config, bool isUnlocked)> pool)
		{
			this.pool = pool.ToList();
			index = Mathf.Clamp(index, 0, this.pool.Count - 1);
			InitCurrent();
		}

		private void CycleNext()
		{
			index = (index + 1) % pool.Count;
			InitCurrent();
		}

		private void CyclePrevious()
		{
			index = (index - 1 + pool.Count) % pool.Count;
			InitCurrent();
		}

		private void InitCurrent()
		{
			(TrainConfig, bool) tuple = pool[index];
			this.OnConfigSelected?.Invoke(tuple.Item1, tuple.Item2);
			locoNameText.text = tuple.Item1.LocoData.CartName;
			trainConfigView.Initialize(tuple.Item1);
			statCollectionView.Initialize(tuple.Item1.Stats);
			selectButton.interactable = tuple.Item2;
			locoNameText.color = (tuple.Item2 ? Color.white : Color.gray);
		}
	}
}
