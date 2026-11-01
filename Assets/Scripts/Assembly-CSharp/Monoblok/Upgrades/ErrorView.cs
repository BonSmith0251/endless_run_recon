using FMODUnity;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class ErrorView : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private TMP_Text errorText;

		[SerializeField]
		private float showDuration = 3f;

		[SerializeField]
		private EventReference errorRef;

		private Sequence showSeq;

		private void Awake()
		{
			canvasGroup.alpha = 0f;
		}

		public void Show(string errorMsg)
		{
			RuntimeManager.PlayOneShot(errorRef);
			errorText.text = errorMsg;
			showSeq.Stop();
			canvasGroup.alpha = 1f;
			showSeq = Sequence.Create().ChainDelay(showDuration).Chain(Tween.Alpha(canvasGroup, 0f, 1f));
		}
	}
}
