using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.DamageSystem
{
	public class DamageNumber : MonoBehaviour
	{
		private enum AnimState
		{
			Idle,
			Moving,
			ScalingDown
		}

		[SerializeField]
		private TMP_Text text;

		[SerializeField]
		private RectTransform rectTransform;

		[Header("Animation")]
		[SerializeField]
		private float moveDuration = 0.2f;

		[SerializeField]
		private float scaleDownDuration = 0.1f;

		private const float BackEaseOvershoot = 1.70158f;

		private ObjectPool<DamageNumber> pool;

		private AnimState state;

		private float timer;

		private float startY;

		private float endY;

		public RectTransform RectTransform => rectTransform;

		public event Action<DamageNumber> Released;

		public void Initialize(ObjectPool<DamageNumber> pool)
		{
			this.pool = pool;
		}

		public void Release()
		{
			state = AnimState.Idle;
			timer = 0f;
			this.Released?.Invoke(this);
			if (pool == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				pool.Release(this);
			}
		}

		public void SetText(string text)
		{
			this.text.SetText(text);
		}

		public void SetColor(Color color)
		{
			text.color = color;
		}

		public void Animate(float yPositionGain)
		{
			timer = 0f;
			startY = rectTransform.anchoredPosition.y;
			endY = startY + yPositionGain;
			state = AnimState.Moving;
		}

		private void Update()
		{
			switch (state)
			{
			case AnimState.Moving:
				UpdateMove();
				break;
			case AnimState.ScalingDown:
				UpdateScaleDown();
				break;
			}
		}

		private void UpdateMove()
		{
			timer += Time.deltaTime;
			float num = ((moveDuration > 0f) ? Mathf.Clamp01(timer / moveDuration) : 1f);
			float t = 1f - (1f - num) * (1f - num);
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition.y = Mathf.LerpUnclamped(startY, endY, t);
			rectTransform.anchoredPosition = anchoredPosition;
			if (num >= 1f)
			{
				timer = 0f;
				state = AnimState.ScalingDown;
			}
		}

		private void UpdateScaleDown()
		{
			timer += Time.deltaTime;
			float num = ((scaleDownDuration > 0f) ? Mathf.Clamp01(timer / scaleDownDuration) : 1f);
			float t = 2.70158f * num * num * num - 1.70158f * num * num;
			float num2 = Mathf.LerpUnclamped(1f, 0f, t);
			base.transform.localScale = Vector3.one * num2;
			if (num >= 1f)
			{
				state = AnimState.Idle;
				Release();
			}
		}
	}
}
