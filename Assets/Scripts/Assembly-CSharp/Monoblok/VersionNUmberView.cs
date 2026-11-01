using TMPro;
using UnityEngine;

namespace Monoblok
{
	public class VersionNUmberView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text versionText;

		private void Awake()
		{
			versionText.text = Application.version;
		}
	}
}
