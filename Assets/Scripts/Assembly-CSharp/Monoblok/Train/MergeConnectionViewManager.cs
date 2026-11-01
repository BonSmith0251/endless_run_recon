using System.Collections.Generic;
using Monoblok.Progression;
using Monoblok.SA;
using UnityEngine;
using ZLinq;

namespace Monoblok.Train
{
	public class MergeConnectionViewManager : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private MergeConnectionRuntimeCollection activeMergeCollection;

		[SerializeField]
		private MergeConnectionView viewPrefab;

		[SerializeField]
		private RectTransform container;

		private Dictionary<CartMergeConnection, MergeConnectionView> viewMap = new Dictionary<CartMergeConnection, MergeConnectionView>();

		private Camera mainCam;

		private void Awake()
		{
			mainCam = Camera.main;
			activeMergeCollection.OnAdded += OnConnectionAdded;
			activeMergeCollection.OnRemoved += OnConnectionRemoved;
			foreach (CartMergeConnection item in activeMergeCollection.Collection.AsValueEnumerable().ToList())
			{
				if (item.Parent == null || item.Child == null)
				{
					activeMergeCollection.Unregister(item);
				}
				else
				{
					OnConnectionAdded(item);
				}
			}
		}

		private void OnDestroy()
		{
			activeMergeCollection.OnAdded -= OnConnectionAdded;
			activeMergeCollection.OnRemoved -= OnConnectionRemoved;
		}

		private void Update()
		{
			if (progression.IsRunning)
			{
				return;
			}
			foreach (MergeConnectionView value in viewMap.Values)
			{
				value.UpdatePosition(mainCam);
			}
		}

		private void OnConnectionAdded(CartMergeConnection connection)
		{
			MergeConnectionView mergeConnectionView = Object.Instantiate(viewPrefab, container);
			mergeConnectionView.Initialize(connection);
			viewMap.Add(connection, mergeConnectionView);
		}

		private void OnConnectionRemoved(CartMergeConnection connection)
		{
			if (viewMap.TryGetValue(connection, out var value))
			{
				viewMap.Remove(connection);
				Object.Destroy(value.gameObject);
			}
		}
	}
}
