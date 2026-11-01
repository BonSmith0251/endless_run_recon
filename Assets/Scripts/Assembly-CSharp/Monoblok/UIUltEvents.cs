using UltEvents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Monoblok
{
	public class UIUltEvents : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		[SerializeField]
		private UltEvent Click;

		[SerializeField]
		private UltEvent PointerEnter;

		[SerializeField]
		private UltEvent PointerExit;

		[SerializeField]
		private UltEvent PointerDown;

		[SerializeField]
		private UltEvent PointerUp;

		public void OnPointerClick(PointerEventData eventData)
		{
			Click?.Invoke();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			PointerDown?.Invoke();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			PointerEnter?.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			PointerExit?.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			PointerUp?.Invoke();
		}
	}
}
