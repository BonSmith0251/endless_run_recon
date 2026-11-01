using UnityEngine;

namespace Monoblok
{
	[ExecuteInEditMode]
	[DefaultExecutionOrder(10000)]
	public class ScaleToCamera : MonoBehaviour
	{
		[SerializeField]
		private Camera cam;

		[SerializeReference]
		[SubclassSelector]
		private IScaler scaler;

		private void Awake()
		{
			if (cam == null)
			{
				cam = Camera.main;
			}
		}

		private void LateUpdate()
		{
			Scale();
		}

		private void Scale()
		{
			if (!(cam == null) && scaler != null)
			{
				float num = cam.orthographicSize * 2f;
				float x = num * cam.aspect;
				scaler.Scale(new Vector2(x, num));
			}
		}
	}
}
