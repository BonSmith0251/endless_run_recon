using UnityEngine;

namespace Monoblok
{
	public class EnableGameObjectBlendable : EndlessModeBlendable
	{
		[SerializeField]
		private GameObject go;

		[SerializeField]
		private bool targetState;

		public override void Blend()
		{
			go.SetActive(targetState);
		}

		public override void UnBlend()
		{
			go.SetActive(!targetState);
		}
	}
}
