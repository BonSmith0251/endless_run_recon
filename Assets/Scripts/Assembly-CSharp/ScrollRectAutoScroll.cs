using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour
{
	[SerializeField]
	private float scrollMargin = 0.3f;

	private ScrollRect scrollRect;

	private GameObject lastSelected;

	private RectTransform lastSelectedRect;

	private void Update()
	{
		InitializeIfNeeded();
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (!(currentSelectedGameObject == null))
		{
			if (currentSelectedGameObject != lastSelected)
			{
				lastSelected = currentSelectedGameObject;
				lastSelectedRect = currentSelectedGameObject.GetComponent<RectTransform>();
			}
			if (!(lastSelectedRect == null) && lastSelectedRect.IsChildOf(scrollRect.content))
			{
				ScrollToItem(lastSelectedRect);
			}
		}
	}

	private void InitializeIfNeeded()
	{
		if (!(scrollRect != null))
		{
			scrollRect = GetComponent<ScrollRect>();
			if (scrollRect == null)
			{
				Debug.LogError("Scroll rect not found");
			}
		}
	}

	private void ScrollToItem(RectTransform selectedRect)
	{
		float height = scrollRect.content.rect.height;
		float height2 = scrollRect.viewport.rect.height;
		if (height <= height2)
		{
			return;
		}
		float y = selectedRect.localPosition.y;
		float height3 = selectedRect.rect.height;
		float num = y + height3 / 2f;
		float num2 = y - height3 / 2f;
		float num3 = (height - height2) * scrollRect.normalizedPosition.y - height;
		float num4 = num3 + height2;
		float num5;
		if (num > num4)
		{
			num5 = num - height2 + height3 * scrollMargin;
		}
		else
		{
			if (!(num2 < num3))
			{
				return;
			}
			num5 = num2 - height3 * scrollMargin;
		}
		float value = (num5 + height) / (height - height2);
		scrollRect.normalizedPosition = new Vector2(0f, Mathf.Clamp01(value));
	}
}
