using System;
using Monoblok.Train;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monoblok.Inventory
{
	public class ModuleUIView : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		[SerializeField]
		private RectTransform rendererWrapper;

		[SerializeField]
		private Image moduleImage;

		private MaskableGraphic[] maskables;

		public bool draggable = true;

		public ModuleData Module { get; private set; }

		public event Action<ModuleUIView, Vector2> OnDropped;

		public event Action<ModuleUIView> OnPointerEntered;

		public event Action<ModuleUIView> OnPointerExited;

		private void Awake()
		{
			maskables = GetComponentsInChildren<MaskableGraphic>();
		}

		public void Initialize(ModuleData moduleData)
		{
			Module = moduleData;
			moduleImage.sprite = moduleData.Definition.Sprite;
			moduleImage.preserveAspect = true;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (draggable)
			{
				MaskableGraphic[] array = maskables;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].maskable = false;
				}
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (draggable)
			{
				rendererWrapper.transform.position = eventData.position;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (draggable)
			{
				MaskableGraphic[] array = maskables;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].maskable = true;
				}
				if (this.OnDropped == null)
				{
					ResetPosition();
				}
				else
				{
					this.OnDropped(this, eventData.position);
				}
			}
		}

		public void ResetPosition()
		{
			rendererWrapper.transform.localPosition = Vector2.zero;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			this.OnPointerEntered?.Invoke(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			this.OnPointerExited?.Invoke(this);
		}
	}
}
