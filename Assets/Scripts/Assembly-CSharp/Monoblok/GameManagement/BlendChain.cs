using System;
using Monoblok.StateMachine;

namespace Monoblok.GameManagement
{
	public class BlendChain : ICompletable
	{
		private ICompletable[] chain;

		private int index;

		public event Action OnComplete;

		public BlendChain(params ICompletable[] completables)
		{
			chain = completables;
		}

		public void Initialize()
		{
			index = 0;
			MoveNext();
		}

		private void MoveNext()
		{
			if (index >= chain.Length)
			{
				this.OnComplete?.Invoke();
				return;
			}
			ICompletable next = chain[index];
			index++;
			next.OnComplete += OnNextComplete;
			next.Initialize();
			void OnNextComplete()
			{
				next.OnComplete -= OnNextComplete;
				MoveNext();
			}
		}
	}
}
