using FMOD.Studio;
using FMODUnity;
using Monoblok.Progression;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok.Environment
{
	public class TrainSounds : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private TrainController controller;

		[Header("Events")]
		[SerializeField]
		private EventReference run;

		[SerializeField]
		private EventReference depart;

		[SerializeField]
		private EventReference stop;

		[SerializeField]
		private EventReference brake;

		[SerializeField]
		private EventReference accelerate;

		private EventInstance runInstance;

		private EventInstance brakeInstance;

		private EventInstance accelerateInstance;

		private void Awake()
		{
			runInstance = RuntimeManager.CreateInstance(run);
			brakeInstance = RuntimeManager.CreateInstance(brake);
			accelerateInstance = RuntimeManager.CreateInstance(accelerate);
		}

		private void OnEnable()
		{
			progression.OnJourneyStarted += OnJourneyStarted;
			progression.OnJourneyEnded += OnJourneyEnded;
			progression.OnWaveEnded += OnWaveEnded;
			controller.OnAccelerationStarted += HandleAccelerationStarted;
			controller.OnAccelerationEnded += HandleAccelerationEnded;
			controller.OnBrakeStarted += HandleBrakeStarted;
			controller.OnBrakeEnded += HandleBrakeEnded;
		}

		private void OnDisable()
		{
			progression.OnJourneyStarted -= OnJourneyStarted;
			progression.OnJourneyEnded -= OnJourneyEnded;
			progression.OnWaveEnded -= OnWaveEnded;
			controller.OnAccelerationStarted -= HandleAccelerationStarted;
			controller.OnAccelerationEnded -= HandleAccelerationEnded;
			controller.OnBrakeStarted -= HandleBrakeStarted;
			controller.OnBrakeEnded -= HandleBrakeEnded;
		}

		public void StopAll()
		{
			runInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			brakeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			accelerateInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		private void OnJourneyStarted()
		{
			RuntimeManager.PlayOneShot(depart, base.transform.position);
			runInstance.start();
		}

		private void OnWaveEnded()
		{
			RuntimeManager.PlayOneShot(stop, base.transform.position);
			runInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		private void OnJourneyEnded()
		{
		}

		private void HandleAccelerationStarted()
		{
			accelerateInstance.start();
		}

		private void HandleAccelerationEnded()
		{
			accelerateInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		private void HandleBrakeStarted()
		{
			brakeInstance.start();
		}

		private void HandleBrakeEnded()
		{
			brakeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
}
