using System;
using PrimeTween;
using UnityEngine;

namespace Monoblok
{
	[RequireComponent(typeof(CanvasGroup))]
	public class Curtain : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private float fadeDur = 1f;

		public event Action OnFadeComplete;

		private void OnValidate()
		{
			if (!(canvasGroup != null))
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
		}

		public void FadeIn()
		{
			canvasGroup.blocksRaycasts = true;
			Tween.Alpha(canvasGroup, 1f, fadeDur).OnComplete(this, delegate(Curtain t)
			{
				t.OnFadeComplete?.Invoke();
			});
		}

		public void CutToBlack()
		{
			canvasGroup.blocksRaycasts = true;
			canvasGroup.alpha = 1f;
		}

		public void FadeOut()
		{
			Tween.Alpha(canvasGroup, 0f, fadeDur).OnComplete(this, delegate(Curtain t)
			{
				t.canvasGroup.blocksRaycasts = false;
				t.OnFadeComplete?.Invoke();
			});
		}
	}
}
