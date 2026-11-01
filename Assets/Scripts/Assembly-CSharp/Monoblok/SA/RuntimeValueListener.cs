using UltEvents;
using UnityEngine;

namespace Monoblok.SA
{
	public abstract class RuntimeValueListener<T> : MonoBehaviour
	{
		[SerializeField]
		private RuntimeValue<T> runtimeValue;

		[SerializeField]
		private UltEvent<T> onValueChanged;

		[SerializeField]
		private bool invokeOnEnable = true;

		public UltEvent<T> OnValueChanged => onValueChanged;

		private void OnEnable()
		{
			if (!(runtimeValue == null))
			{
				runtimeValue.OnValueChanged += HandleValueChange;
				if (invokeOnEnable)
				{
					HandleValueChange(runtimeValue.Value);
				}
			}
		}

		private void OnDisable()
		{
			if (!(runtimeValue == null))
			{
				runtimeValue.OnValueChanged -= HandleValueChange;
			}
		}

		private void HandleValueChange(T t)
		{
			onValueChanged?.Invoke(t);
		}
	}
}
