using System;
using UnityEngine;

namespace Monoblok.Core.UI
{
	public class OneButtonModalSequence : MonoBehaviour
	{
		[SerializeField]
		private OneButtonModal[] sequence;

		private int currentIndex;

		public event Action OnComplete;

		public void Play()
		{
			currentIndex = 0;
			ShowCurrent();
		}

		private void ShowCurrent()
		{
			if (currentIndex >= sequence.Length)
			{
				this.OnComplete?.Invoke();
			}
			else
			{
				sequence[currentIndex].Show(HandleConfirmed);
			}
		}

		private void HandleConfirmed()
		{
			currentIndex++;
			ShowCurrent();
		}
	}
}
