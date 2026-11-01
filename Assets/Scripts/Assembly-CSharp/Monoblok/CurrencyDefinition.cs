using UnityEngine;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Currency")]
	public class CurrencyDefinition : IDScriptableObject
	{
		[field: SerializeField]
		public string CurrencyName { get; private set; }

		[field: SerializeField]
		public string DisplayName { get; private set; }

		[field: SerializeField]
		public Color TextColor { get; private set; }

		[field: SerializeField]
		public Sprite Sprite { get; private set; }
	}
}
