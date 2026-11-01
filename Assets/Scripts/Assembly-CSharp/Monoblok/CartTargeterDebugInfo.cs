using TMPro;
using UnityEngine;

namespace Monoblok
{
	public class CartTargeterDebugInfo : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text targetIndexText;

		[SerializeField]
		private TMP_Text selectedIndexText;

		public void SetTargetIndex(int index)
		{
			targetIndexText.text = index.ToString();
		}

		public void SetSelectedIndex(int index)
		{
			selectedIndexText.text = index.ToString();
		}
	}
}
