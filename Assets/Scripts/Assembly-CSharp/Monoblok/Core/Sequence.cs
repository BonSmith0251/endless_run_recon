using System;
using System.Collections.Generic;
using System.Linq;

namespace Monoblok.Core
{
	public class Sequence
	{
		private readonly List<ISequenceElement> elements;

		private int index = -1;

		private ISequenceElement Current
		{
			get
			{
				if (index < 0 || index >= elements.Count)
				{
					return null;
				}
				return elements[index];
			}
		}

		public event Action OnComplete;

		public Sequence(IEnumerable<ISequenceElement> elements, bool autoRun = false)
		{
			this.elements = elements.ToList();
			if (autoRun)
			{
				Run();
			}
		}

		public void Run()
		{
			index = -1;
			MoveNext();
		}

		private void MoveNext()
		{
			ISequenceElement current = Current;
			if (current != null)
			{
				current.OnComplete -= MoveNext;
				current.Exit();
			}
			index++;
			if (Current == null)
			{
				this.OnComplete?.Invoke();
				return;
			}
			Current.OnComplete += MoveNext;
			Current.Enter();
		}
	}
}
