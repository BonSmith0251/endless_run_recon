using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.StatSystem
{
	[CreateAssetMenu(menuName = "Game/Stat")]
	public class StatDefinition : ScriptableObject
	{
		[SerializeField]
		private LocalizedString localizedName;

		public string Name => localizedName.GetLocalizedString();

		public LocalizedString LocalizedName => localizedName;

		[field: SerializeField]
		public string Description { get; private set; }

		[field: SerializeField]
		public bool Public { get; private set; } = true;


		[field: SerializeField]
		public StatValueType StatValueType { get; private set; }

		[field: SerializeField]
		public float MaxValue { get; private set; } = float.MaxValue;


		[field: SerializeField]
		public Sprite Icon { get; private set; }

		public float GetValue(StatManager statManager)
		{
			if (statManager == null)
			{
				throw new Exception("Statmanager is null");
			}
			return statManager.GetStatValue(this);
		}
	}
}
