using System;
using System.Collections.Generic;

namespace Monoblok.Core
{
	[Serializable]
	public class Observable<T>
	{
		private T value;

		public T Value => value;

		public event Action<T> OnValueChanged;

		public Observable(T value)
		{
			this.value = value;
		}

		public void SetValue(T newValue)
		{
			if (!EqualityComparer<T>.Default.Equals(value, newValue))
			{
				value = newValue;
				this.OnValueChanged?.Invoke(value);
			}
		}

		public override string ToString()
		{
			return value.ToString();
		}
	}
}
