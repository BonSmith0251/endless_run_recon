using Monoblok.StatSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	[RequireComponent(typeof(RectTransform))]
	public class WorldStatModifierView : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private Image statIcon;

		[SerializeField]
		private Image buffIcon;

		[SerializeField]
		private Image debuffIcon;

		public void Initialize(StatDefinition def, bool buffOrDebuff)
		{
			statIcon.sprite = def.Icon;
			buffIcon.gameObject.SetActive(buffOrDebuff);
			debuffIcon.gameObject.SetActive(!buffOrDebuff);
		}
	}
}
