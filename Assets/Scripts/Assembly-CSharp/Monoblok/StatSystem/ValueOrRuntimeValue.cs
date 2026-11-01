using System;
using Monoblok.SA;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.StatSystem
{
	[Serializable]
	public class ValueOrRuntimeValue<T>
	{
		[SerializeField]
		[ShowIf("Show")]
		[AllowNesting]
		private T value;

		[SerializeField]
		[Expandable]
		[AllowNesting]
		private RuntimeValue<T> runtimeValue;

		public T Value
		{
			get
			{
				if (!(runtimeValue != null))
				{
					return value;
				}
				return runtimeValue.Value;
			}
		}

		public ValueOrRuntimeValue()
		{
		}

		public ValueOrRuntimeValue(T value)
		{
			this.value = value;
		}

		public ValueOrRuntimeValue(T value, RuntimeValue<T> runtimeValue)
			: this(value)
		{
			this.runtimeValue = runtimeValue;
		}

		public void Subscribe(Action<T> actionOnValueChanged)
		{
			if (!(runtimeValue == null))
			{
				runtimeValue.OnValueChanged += actionOnValueChanged;
			}
		}

		public void Unsubscribe(Action<T> actionOnValueChanged)
		{
			if (!(runtimeValue == null))
			{
				runtimeValue.OnValueChanged -= actionOnValueChanged;
			}
		}

		public ValueOrRuntimeValue<T> Clone()
		{
			return new ValueOrRuntimeValue<T>
			{
				value = value,
				runtimeValue = runtimeValue
			};
		}
	}
}
