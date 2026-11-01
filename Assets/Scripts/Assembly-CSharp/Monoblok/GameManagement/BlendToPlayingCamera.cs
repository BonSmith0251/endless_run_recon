using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class BlendToPlayingCamera : ICompletable
	{
		private readonly CameraManager cameraManager;

		public event Action OnComplete;

		public BlendToPlayingCamera(CameraManager cameraManager)
		{
			this.cameraManager = cameraManager;
		}

		public void Initialize()
		{
			cameraManager.BlendToPlayingCamera();
			cameraManager.OnBlendComplete += OnBlendComplete;
		}

		private void OnBlendComplete()
		{
			cameraManager.OnBlendComplete -= OnBlendComplete;
			this.OnComplete?.Invoke();
		}
	}
}
