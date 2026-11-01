using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.SA
{
	public abstract class RuntimeCollection<T> : ScriptableObject
	{
		private HashSet<T> collection = new HashSet<T>();

		public IReadOnlyCollection<T> Collection => collection;

		public event Action<IReadOnlyCollection<T>> OnCollectionChanged;

		public event Action<T> OnAdded;

		public event Action<T> OnRemoved;

		public void Register(T value)
		{
			if (collection.Add(value))
			{
				this.OnAdded?.Invoke(value);
				this.OnCollectionChanged?.Invoke(collection);
			}
		}

		public void Unregister(T value)
		{
			if (collection.Remove(value))
			{
				this.OnRemoved?.Invoke(value);
				this.OnCollectionChanged?.Invoke(collection);
			}
		}

		[Button(null, EButtonEnableMode.Always)]
		private void Clear()
		{
			collection.Clear();
			this.OnCollectionChanged?.Invoke(collection);
		}
	}
}
