using UnityEngine;
using UnityEngine.Video;

namespace Monoblok
{
	public class WebGLVideoPlayerInjector : MonoBehaviour
	{
		[SerializeField]
		private VideoPlayer player;

		[SerializeField]
		private string fileName;

		private void Awake()
		{
		}
	}
}
