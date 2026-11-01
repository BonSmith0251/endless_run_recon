using System;
using Monoblok.Input;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class GlobalControlsBlend : ICompletable
	{
		private readonly InputReader inputReader;

		private readonly bool enable;

		public event Action OnComplete;

		public GlobalControlsBlend(InputReader inputReader, bool enable)
		{
			this.inputReader = inputReader;
			this.enable = enable;
		}

		public void Initialize()
		{
			inputReader.EnableGlobalControls(enable);
			this.OnComplete?.Invoke();
		}
	}
}
