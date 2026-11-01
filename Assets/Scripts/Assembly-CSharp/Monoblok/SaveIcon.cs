using PrimeTween;
using UnityEngine;

namespace Monoblok
{
	public class SaveIcon : MonoBehaviour
	{
		[SerializeField]
		private SaveLoadManager saveLoadManager;

		[SerializeField]
		private CanvasGroup saveIconGroup;

		private Sequence seq;

		private void OnEnable()
		{
			saveLoadManager.OnRunSaved += HandleRunSaved;
		}

		private void OnDisable()
		{
			saveLoadManager.OnRunSaved -= HandleRunSaved;
			seq.Stop();
			saveIconGroup.alpha = 0f;
		}

		private void HandleRunSaved()
		{
			seq = Sequence.Create(1, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true).Chain(Tween.Alpha(saveIconGroup, saveIconGroup.alpha, 1f)).Chain(Tween.Alpha(saveIconGroup, 1f, 0.5f, 0.5f, default(Easing), 4, CycleMode.Yoyo))
				.Chain(Tween.Alpha(saveIconGroup, 1f, 0f, 0.5f));
		}
	}
}
