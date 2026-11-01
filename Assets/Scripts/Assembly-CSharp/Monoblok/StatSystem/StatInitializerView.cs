using TMPro;
using UnityEngine;

namespace Monoblok.StatSystem
{
	public class StatInitializerView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text statNameText;

		[SerializeField]
		private TMP_Text baseValueText;

		public void Initialize(StatInitializer initializer)
		{
			statNameText.text = initializer.StatDefinition.Name;
			baseValueText.text = initializer.BaseValue.ToString();
		}
	}
}
