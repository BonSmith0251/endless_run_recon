using System;
using Cysharp.Threading.Tasks;
using Monoblok.SA;
using Monoblok.Train;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

namespace Monoblok
{
	public class CameraManager : MonoBehaviour
	{
		private enum State
		{
			Playing,
			Station,
			MainMenu
		}

		private const int ActivePriority = 10;

		private const int InactivePriority = 0;

		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		private FramingCamera endWaveCamera;

		[SerializeField]
		private FramingCamera mainMenuCamera;

		[SerializeField]
		private CinemachineCamera playingCamera;

		[SerializeField]
		private RectTransform mainMenuSafeArea;

		[SerializeField]
		private RectTransform stationSafeArea;

		private State state;

		private CinemachineBasicMultiChannelPerlin shakeNoise;

		private bool subscribedToCartEvents;

		public event Action OnBlendComplete;

		private void OnDisable()
		{
			UnsubscribeFromCartEvents();
			CinemachineCore.BlendFinishedEvent.RemoveListener(HandleBlendFinishedEvent);
		}

		private void OnCartAdded(TrainCart cart)
		{
			ReframeCam().Forget();
		}

		private void OnCartRemoved(TrainCart cart)
		{
			ReframeCam().Forget();
		}

		private async UniTaskVoid ReframeCam()
		{
			if (state == State.Station)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(0.0));
				if (state == State.Station && !(trainRuntimeValue.Value == null))
				{
					endWaveCamera.FrameCamera(stationSafeArea, instant: false);
				}
			}
		}

		public void BlendToStationCamera(bool instant = true)
		{
			state = State.Station;
			endWaveCamera.FrameCamera(stationSafeArea, instant);
			SetActiveCamera(endWaveCamera.Camera);
			SubscribeToCartEvents();
		}

		public void BlendToMainMenuCamera(bool instant = true)
		{
			state = State.MainMenu;
			UnsubscribeFromCartEvents();
			mainMenuCamera.FrameCamera(mainMenuSafeArea, instant);
			SetActiveCamera(mainMenuCamera.Camera);
		}

		public void BlendToPlayingCamera()
		{
			state = State.Playing;
			UnsubscribeFromCartEvents();
			SetActiveCamera(playingCamera);
		}

		private void SetActiveCamera(CinemachineCamera activeCamera)
		{
			playingCamera.Priority = ((activeCamera == playingCamera) ? 10 : 0);
			endWaveCamera.Camera.Priority = ((activeCamera == endWaveCamera.Camera) ? 10 : 0);
			mainMenuCamera.Camera.Priority = ((activeCamera == mainMenuCamera.Camera) ? 10 : 0);
			CinemachineCore.BlendFinishedEvent.RemoveListener(HandleBlendFinishedEvent);
			CinemachineCore.BlendFinishedEvent.AddListener(HandleBlendFinishedEvent);
		}

		private void HandleBlendFinishedEvent(ICinemachineMixer mixer, ICinemachineCamera cam)
		{
			CinemachineCore.BlendFinishedEvent.RemoveListener(HandleBlendFinishedEvent);
			this.OnBlendComplete?.Invoke();
		}

		private void SubscribeToCartEvents()
		{
			if (!subscribedToCartEvents && !(trainRuntimeValue.Value == null))
			{
				trainRuntimeValue.Value.OnCartAdded += OnCartAdded;
				trainRuntimeValue.Value.OnCartRemoved += OnCartRemoved;
				subscribedToCartEvents = true;
			}
		}

		private void UnsubscribeFromCartEvents()
		{
			if (subscribedToCartEvents && !(trainRuntimeValue.Value == null))
			{
				trainRuntimeValue.Value.OnCartAdded -= OnCartAdded;
				trainRuntimeValue.Value.OnCartRemoved -= OnCartRemoved;
				subscribedToCartEvents = false;
			}
		}

		public void Shake(float dur)
		{
			if (shakeNoise == null)
			{
				shakeNoise = endWaveCamera.Camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
			}
			if (!(shakeNoise == null))
			{
				shakeNoise.AmplitudeGain = 1f;
				shakeNoise.FrequencyGain = 1f;
				Tween.Delay(shakeNoise, dur, delegate(CinemachineBasicMultiChannelPerlin t)
				{
					t.AmplitudeGain = 0f;
					t.FrequencyGain = 0f;
				});
			}
		}
	}
}
