using System;
using Monoblok.Core;
using Monoblok.Core.UI;
using UnityEngine;

namespace Monoblok.GameManagement
{
	public class TutorialSequenceElement : ISequenceElement
	{
		private readonly OneButtonModalSequence tutorialSequence;

		public event Action OnComplete;

		public TutorialSequenceElement(OneButtonModalSequence tutorialSequence)
		{
			this.tutorialSequence = tutorialSequence;
		}

		public void Enter()
		{
			tutorialSequence.GetComponent<Canvas>().gameObject.SetActive(value: true);
			tutorialSequence.OnComplete += HandleTutorialComplete;
			tutorialSequence.Play();
		}

		private void HandleTutorialComplete()
		{
			this.OnComplete?.Invoke();
		}

		public void Exit()
		{
			tutorialSequence.OnComplete -= HandleTutorialComplete;
			tutorialSequence.GetComponent<Canvas>().gameObject.SetActive(value: false);
		}
	}
}
