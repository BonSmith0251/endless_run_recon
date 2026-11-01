using UnityEngine;

namespace Monoblok
{
	public class ScreenSizeKnockback : RadialKnockback
	{
		protected override float GetRadius()
		{
			float orthographicSize = Camera.main.orthographicSize;
			float num = orthographicSize * Camera.main.aspect;
			return Mathf.Sqrt(num * num + orthographicSize * orthographicSize);
		}
	}
}
