using System;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class RectMaskProgress : MonoBehaviour, IProgress<float>
	{
		[SerializeField]
		private RectMask2D mask;

		private float size;

		private void Start()
		{
			Vector4 padding = mask.padding;
			size = GetComponent<RectTransform>().rect.size.x;
			padding.y = size;
			mask.padding = padding;
		}

		public void Report(float value)
		{
			Vector4 padding = mask.padding;
			padding.y = size * value;
			mask.padding = padding;
		}
	}
}
