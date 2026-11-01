using UnityEngine;

namespace Monoblok
{
	public class Float : MonoBehaviour
	{
		private float height;

		private float duration;

		private float time;

		private void OnEnable()
		{
			base.transform.localPosition = Vector3.zero;
			height = Random.Range(1f, 2f);
			duration = Random.Range(0.5f, 1f);
			time = 0f;
		}

		private void Update()
		{
			time += Time.deltaTime;
			float t = Mathf.PingPong(time / duration, 1f);
			t = Mathf.SmoothStep(0f, 1f, t);
			base.transform.localPosition = Vector2.up * (height * t);
		}
	}
}
