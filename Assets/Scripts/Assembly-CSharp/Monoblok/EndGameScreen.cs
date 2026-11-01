using System;
using Monoblok.GameManagement;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class EndGameScreen : MonoBehaviour
	{
		[SerializeField]
		private Canvas canvas;

		[SerializeField]
		private GameObject winObject;

		[SerializeField]
		private GameObject loseObject;

		[SerializeField]
		private TMP_Text trainLevelText;

		[SerializeField]
		private TMP_Text metaCurrencyText;

		[SerializeField]
		private TMP_Text timeText;

		[SerializeField]
		private TMP_Text stationsReachedText;

		[SerializeField]
		private TMP_Text enemiesDefeatedText;

		[SerializeField]
		private TMP_Text coinsCollectedText;

		[SerializeField]
		private Button upgradesButton;

		[SerializeField]
		private Button endlessButton;

		public event Action OnUpgradesClicked;

		public event Action OnEndlessClicked;

		private void OnEnable()
		{
			upgradesButton.onClick.AddListener(HandleUpgradesClicked);
			endlessButton.onClick.AddListener(HandleEndlessClicked);
		}

		private void OnDisable()
		{
			upgradesButton.onClick.RemoveAllListeners();
			endlessButton.onClick.RemoveAllListeners();
		}

		private void HandleUpgradesClicked()
		{
			this.OnUpgradesClicked?.Invoke();
		}

		private void HandleEndlessClicked()
		{
			this.OnEndlessClicked?.Invoke();
		}

		public void Initialize(EndGameSummary summary)
		{
			trainLevelText.text = summary.gainedLevels.ToString();
			Sequence.Create(1, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true).Chain(PrimeTweenExitensions.Counter(metaCurrencyText, 0, summary.metaCurrency.Amount, 1.5f)).Chain(Tween.PunchScale(metaCurrencyText.transform, Vector3.one * 0.8f, 0.3f, 2f));
			switch (summary.state)
			{
			case EndGameSummary.State.SoftWin:
				winObject.SetActive(value: true);
				loseObject.SetActive(value: false);
				break;
			case EndGameSummary.State.Lost:
				winObject.SetActive(value: false);
				loseObject.SetActive(value: true);
				break;
			}
			timeText.text = TimeSpan.FromSeconds(summary.timeSpent).ToString("mm\\:ss");
			stationsReachedText.text = summary.stationsReached.ToString();
			enemiesDefeatedText.text = summary.enemiesDefeated.ToString();
			coinsCollectedText.text = summary.coinsCollected.ToString();
		}

		public void Show(bool showEndless)
		{
			endlessButton.gameObject.SetActive(showEndless);
			upgradesButton.interactable = true;
			canvas.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			upgradesButton.interactable = false;
			canvas.gameObject.SetActive(value: false);
		}
	}
}
