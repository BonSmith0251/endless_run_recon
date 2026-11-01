using FMOD.Studio;
using FMODUnity;
using Monoblok.Progression;
using PrimeTween;
using UnityEngine;

namespace Monoblok
{
	public class MusicLayerController : MonoBehaviour
	{
		[SerializeField]
		private EventReference musicEvent;

		[SerializeField]
		[ParamRef]
		private int paramRef;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private float fadeDuration = 1f;

		private readonly string LayerParamName = "MusicLayer";

		private readonly float WaveParamValue;

		private readonly float StationParamValue = 1f;

		private EventInstance musicEventInstance;

		private void OnEnable()
		{
			progression.OnJourneyStarted += HandleJourneyStarted;
			progression.OnWaveEnded += HandleWaveEnded;
		}

		private void OnDisable()
		{
			progression.OnJourneyStarted -= HandleJourneyStarted;
			progression.OnWaveEnded -= HandleWaveEnded;
		}

		private void Start()
		{
			musicEventInstance = RuntimeManager.CreateInstance(musicEvent);
			musicEventInstance.setParameterByName(LayerParamName, StationParamValue);
			musicEventInstance.start();
		}

		private void OnDestroy()
		{
			musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		private void HandleJourneyStarted()
		{
			TweenParameter(WaveParamValue);
		}

		private void HandleWaveEnded()
		{
			TweenParameter(StationParamValue);
		}

		private void TweenParameter(float target)
		{
			musicEventInstance.getParameterByName(LayerParamName, out var value);
			Tween.Custom(this, value, target, fadeDuration, delegate(MusicLayerController t, float v)
			{
				t.musicEventInstance.setParameterByName(t.LayerParamName, v);
			});
		}

		public void Pause()
		{
			if (musicEventInstance.isValid())
			{
				musicEventInstance.getVolume(out var volume);
				Tween.Custom(this, volume, 0f, 1f, delegate(MusicLayerController t, float v)
				{
					t.SetVolume(v);
				});
			}
		}

		public void Resume()
		{
			if (musicEventInstance.isValid())
			{
				musicEventInstance.getVolume(out var volume);
				Tween.Custom(this, volume, 1f, 1f, delegate(MusicLayerController t, float v)
				{
					t.SetVolume(v);
				});
			}
		}

		private void SetVolume(float volume)
		{
			musicEventInstance.setVolume(volume);
		}
	}
}
