using UnityEngine;

namespace Monoblok
{
	public class Currency
	{
		public readonly string ID;

		public readonly string Name;

		public readonly string DisplayName;

		public readonly Sprite Sprite;

		public readonly Color TextColor;

		public Currency(CurrencyDefinition definition)
		{
			ID = definition.ID;
			Name = definition.CurrencyName;
			DisplayName = definition.DisplayName;
			Sprite = definition.Sprite;
			TextColor = definition.TextColor;
		}
	}
}
