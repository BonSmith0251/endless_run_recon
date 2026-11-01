using System;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Train Config")]
	public class TrainConfig : ScriptableObject
	{
		[SerializeField]
		private string id;

		public string ID => id;

		[field: SerializeField]
		public CartData LocoData { get; private set; }

		[field: SerializeField]
		public CartData[] InitialCarts { get; private set; }

		[field: SerializeField]
		public ModuleDefinition InitialModule { get; private set; }

		[field: SerializeField]
		public StatInitializer[] Stats { get; private set; }

		[field: SerializeField]
		public AbilityConfig AbiltiyConfig { get; private set; }

		private void OnValidate()
		{
			if (string.IsNullOrEmpty(id))
			{
				id = Guid.NewGuid().ToString();
			}
		}
	}
}
