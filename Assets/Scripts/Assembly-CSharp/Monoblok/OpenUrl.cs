using UnityEngine;

namespace Monoblok
{
	// Offline build: external links (X, Discord, Steam store) no longer open a browser.
	// The URL stays serialized for reference.
	public class OpenUrl : MonoBehaviour
	{
		[SerializeField]
		private string url;

		public void Open()
		{
		}
	}
}
