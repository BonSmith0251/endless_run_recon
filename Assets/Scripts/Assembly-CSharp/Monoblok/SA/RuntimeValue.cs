using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.SA
{
	public abstract class RuntimeValue<T> : ScriptableObject, IResettable
	{
		[SerializeField]
		private T defaultValue;

		[SerializeField]
		[ReadOnly]
		private T value;

		public T Value => value;

		public event Action<T> OnValueChanged;

		public void SetValue(T newValue)
		{
			if (!EqualityComparer<T>.Default.Equals(value, newValue))
			{
				value = newValue;
				this.OnValueChanged?.Invoke(value);
			}
		}

		public void ApplyReset()
		{
			SetValue(defaultValue);
		}
	}
}
