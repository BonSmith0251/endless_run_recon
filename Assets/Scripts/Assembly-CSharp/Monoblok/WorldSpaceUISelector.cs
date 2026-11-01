using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monoblok
{
	public class WorldSpaceUISelector : MonoBehaviour
	{
		[SerializeField]
		private Camera cam;

		[SerializeField]
		private float detectionRadiusWorld = 0.5f;

		[SerializeField]
		private Image reticle;

		[SerializeField]
		private LayerMask uiLayerMask;

		private GameObject currentHovered;

		public event Action<GameObject> OnHoverChanged;

		private void OnEnable()
		{
			reticle.gameObject.SetActive(value: true);
		}

		private void OnDisable()
		{
			reticle.gameObject.SetActive(value: false);
		}

		private void Update()
		{
			UpdateHover();
		}

		private void UpdateHover()
		{
			GameObject gameObject = FindBestHit();
			if (!(gameObject == currentHovered))
			{
				PointerEventData eventData = BuildCentrePointerEvent();
				if (currentHovered != null)
				{
					ExecuteEvents.Execute(currentHovered, eventData, ExecuteEvents.deselectHandler);
				}
				currentHovered = gameObject;
				EventSystem.current.SetSelectedGameObject(currentHovered);
				if (currentHovered != null)
				{
					ExecuteEvents.Execute(currentHovered, eventData, ExecuteEvents.selectHandler);
				}
				this.OnHoverChanged?.Invoke(currentHovered);
			}
		}

		private GameObject FindBestHit()
		{
			Collider2D collider2D = Physics2D.OverlapCircle(cam.transform.position, detectionRadiusWorld, uiLayerMask);
			if (!(collider2D != null))
			{
				return null;
			}
			return collider2D.gameObject;
		}

		private PointerEventData BuildCentrePointerEvent()
		{
			return new PointerEventData(EventSystem.current)
			{
				position = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f)
			};
		}
	}
}
