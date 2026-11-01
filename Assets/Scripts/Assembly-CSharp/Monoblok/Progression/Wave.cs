using System;

namespace Monoblok.Progression
{
	public abstract class Wave
	{
		public readonly int WaveIndex;

		private readonly WaveData data;

		private bool isComplete;

		public bool IsComplete => isComplete;

		public event Action OnComplete;

		protected Wave(int waveIndex, WaveData data)
		{
			WaveIndex = waveIndex;
			this.data = data;
		}

		public void StartWave()
		{
			isComplete = false;
			OnStartWave();
			data.Initialize();
		}

		public void EndWave()
		{
			if (!isComplete)
			{
				isComplete = true;
				OnWaveEnded();
				data.End();
			}
		}

		public void UpdateWave(float deltaTime)
		{
			if (!isComplete)
			{
				OnUpdateWave(deltaTime);
			}
		}

		protected abstract void OnStartWave();

		protected abstract void OnUpdateWave(float deltaTime);

		protected void CompleteWave()
		{
			if (!isComplete)
			{
				isComplete = true;
				OnWaveComplete();
				OnWaveEnded();
				data.End();
				this.OnComplete?.Invoke();
			}
		}

		protected virtual void OnWaveComplete()
		{
		}

		protected virtual void OnWaveEnded()
		{
		}
	}
	public abstract class Wave<TData> : Wave where TData : WaveData
	{
		protected TData Data { get; }

		protected Wave(int waveIndex, TData data)
			: base(waveIndex, data)
		{
			Data = data;
		}
	}
}
