using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Train
{
	public class ModuleDefinitionView : MonoBehaviour
	{
		[SerializeField]
		private Image icon;

		[SerializeField]
		private TMP_Text moduleNameText;

		[SerializeField]
		private TMP_Text moduleDescipritonText;

		public void Initialize(ModuleDefinition moduleDefinition)
		{
			icon.sprite = moduleDefinition.Sprite;
			moduleNameText.text = moduleDefinition.DisplayName;
			moduleDescipritonText.text = moduleDefinition.Description;
		}
	}
}
