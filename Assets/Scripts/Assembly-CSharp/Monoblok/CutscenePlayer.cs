using System;
using Monoblok.GameManagement;
using Monoblok.Input;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Monoblok
{
	public class CutscenePlayer : MonoBehaviour
	{
		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private float skipDuration = 2f;

		[SerializeField]
		private Curtain curtain;

		[SerializeField]
		private MusicLayerController musicLayerController;

		[SerializeField]
		private RectTransform mainContainer;

		[SerializeField]
		private Slider slider;

		[SerializeField]
		private PlayableDirector test;

		private float t;

		private PlayableDirector currentDirector;

		private bool isSkipping;

		private CurtainBlend currentBlend;

        // Teardown must not convert a director stop into a new gameplay transition.
        private bool isQuitting;

        private void OnEnable()
        {
            if (currentDirector != null)
            {
                currentDirector.stopped -= HandleDirectorStopped;
                currentDirector.stopped += HandleDirectorStopped;
            }
        }

        private void OnDisable()
        {
            DetachDirectorStopped();
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
            DetachDirectorStopped();
        }

        private void DetachDirectorStopped()
        {
            if (currentDirector != null)
                currentDirector.stopped -= HandleDirectorStopped;
        }


		public event Action OnCutsceneStopped;

		[Button(null, EButtonEnableMode.Always)]
		private void Test()
		{
			inputReader.EnableCutsceneControls(enable: true);
			PlayCutscene(test);
		}

		public void PlayCutscene(PlayableDirector director)
		{
			currentDirector = director;
			musicLayerController.Pause();
			currentBlend = new CurtainBlend(curtain, delegate
			{
				EnableMainContainer(enabled: true);
			}, 1f);
			currentBlend.OnComplete += PlayCurrent;
			currentBlend.Initialize();
		}

		private void EnableMainContainer(bool enabled)
		{
			mainContainer.gameObject.SetActive(enabled);
		}

		private void PlayCurrent()
		{
			currentBlend.OnComplete -= PlayCurrent;
			currentBlend = null;
			currentDirector.gameObject.SetActive(value: true);
			currentDirector.Play();
			isSkipping = false;
			t = 0f;
			slider.value = t;
			currentDirector.stopped -= HandleDirectorStopped;
			currentDirector.stopped += HandleDirectorStopped;
		}

		private void HandleSkipPressed()
		{
			if (!isSkipping)
			{
				isSkipping = true;
				t = 0f;
				slider.value = t;
			}
		}

		private void HandleSkipReleased()
		{
			if (isSkipping)
			{
				isSkipping = false;
				t = 0f;
				slider.value = t;
			}
		}

		private void HandleDirectorStopped(PlayableDirector director)
		{
            // Unity also sends stopped while leaving Play mode or destroying a director.
            // Normal completion and user skip continue through the original fade below.
            if (isQuitting || !Application.isPlaying || !isActiveAndEnabled)
                return;

			currentBlend = new CurtainBlend(curtain, delegate
			{
				EnableMainContainer(enabled: false);
			}, 1f);
			currentBlend.OnComplete += StopCurrent;
			currentBlend.Initialize();
		}

		private void StopCurrent()
		{
			if (currentBlend != null)
			{
				currentBlend.OnComplete -= StopCurrent;
				currentBlend = null;
			}
			musicLayerController.Resume();
			if (!(currentDirector == null))
			{
				currentDirector.gameObject.SetActive(value: false);
				currentDirector.stopped -= HandleDirectorStopped;
				currentDirector = null;
				this.OnCutsceneStopped?.Invoke();
			}
		}

		private void Update()
		{
			if (currentDirector == null || currentDirector.state != PlayState.Playing)
			{
				return;
			}
			if (inputReader.SkipPressed)
			{
				HandleSkipPressed();
			}
			else if (inputReader.SkipReleased)
			{
				HandleSkipReleased();
			}
			if (isSkipping)
			{
				t += Time.deltaTime;
				slider.value = t / skipDuration;
				if (t >= skipDuration)
				{
					currentDirector.Stop();
					t = 0f;
					slider.value = t;
					isSkipping = false;
				}
			}
		}
	}
}
