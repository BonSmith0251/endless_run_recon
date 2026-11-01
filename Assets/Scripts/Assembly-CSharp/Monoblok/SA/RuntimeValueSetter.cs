using UnityEngine;

namespace Monoblok.SA
{
	public abstract class RuntimeValueSetter<T> : MonoBehaviour
	{
		private enum SetterMethod
		{
			Awake,
			OnEnable
		}

		[SerializeField]
		private SetterMethod setterMethod;

		[SerializeField]
		private RuntimeValue<T> container;

		[SerializeField]
		private T value;

		private void Awake()
		{
			if (setterMethod == SetterMethod.Awake)
			{
				SetValue();
			}
		}

		private void OnDestroy()
		{
			if (setterMethod == SetterMethod.Awake)
			{
				ClearValue();
			}
		}

		private void OnEnable()
		{
			if (setterMethod == SetterMethod.OnEnable)
			{
				SetValue();
			}
		}

		private void OnDisable()
		{
			if (setterMethod == SetterMethod.OnEnable)
			{
				ClearValue();
			}
		}

		private void SetValue()
		{
			if (!(container == null))
			{
				container.SetValue(value);
			}
		}

		private void ClearValue()
		{
			if (!(container == null))
			{
				container.SetValue(default(T));
			}
		}
	}
}
