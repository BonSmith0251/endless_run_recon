using System;
using FMODUnity;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.GameManagement
{
	[RequireComponent(typeof(RectTransform))]
	public class JourneyAdvancer : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private Button advanceButton;

		[Header("Config")]
		[SerializeField]
		private TweenSettings<Vector2> showSettings;

		[SerializeField]
		private TweenSettings<Vector2> hideSettings;

		[SerializeField]
		private EventReference clickRef;

		public event Action OnAdvanceClicked;

		private void OnValidate()
		{
			if (rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();
			}
		}

		private void OnEnable()
		{
			advanceButton.onClick.AddListener(HandleAdvanceClicked);
		}

		private void OnDisable()
		{
			advanceButton.onClick.RemoveAllListeners();
		}

		private void HandleAdvanceClicked()
		{
			this.OnAdvanceClicked?.Invoke();
			RuntimeManager.PlayOneShot(clickRef);
		}
	}
}
