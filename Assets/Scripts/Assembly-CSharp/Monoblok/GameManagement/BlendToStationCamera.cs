using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class BlendToStationCamera : ICompletable
	{
		private readonly CameraManager cameraManager;

		public event Action OnComplete;

		public BlendToStationCamera(CameraManager cameraManager)
		{
			this.cameraManager = cameraManager;
		}

		public void Initialize()
		{
			cameraManager.BlendToStationCamera();
			cameraManager.OnBlendComplete += OnBlendComplete;
		}

		private void OnBlendComplete()
		{
			cameraManager.OnBlendComplete -= OnBlendComplete;
			this.OnComplete?.Invoke();
		}
	}
}
