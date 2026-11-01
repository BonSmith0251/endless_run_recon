using System.Threading;
using Cysharp.Threading.Tasks;
using Monoblok.Input;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class ModuleDropHandler : MonoBehaviour
	{
		[SerializeField]
		private LayerMask cartLayer;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private SpriteRenderer ghostPrefab;

		private Camera mainCam;

		private SpriteRenderer ghost;

		private UniTaskCompletionSource<ModuleSlot> tcs;

		private ModuleSlot hoveredSlot;

		private bool isActive;

		private void Awake()
		{
			mainCam = Camera.main;
		}

		public void Initialize(ModuleDefinition definition)
		{
			ghost = Object.Instantiate(ghostPrefab, base.transform);
			ghost.sprite = definition.Sprite;
			ghost.color = Color.red;
			ghost.gameObject.SetActive(value: true);
			isActive = true;
		}

		private void Update()
		{
			if (!isActive)
			{
				return;
			}
			Vector3 vector = mainCam.ScreenToWorldPoint(inputReader.PointerPosition);
			vector.z = 0f;
			ModuleSlot moduleSlot = null;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector2.zero, float.PositiveInfinity, cartLayer);
			if (raycastHit2D.collider != null && raycastHit2D.collider.TryGetComponent<TrainCart>(out var component))
			{
				foreach (ModuleSlot slot in component.ModuleManager.Slots)
				{
					if (slot.IsFree)
					{
						moduleSlot = slot;
						break;
					}
				}
			}
			hoveredSlot = moduleSlot;
			if (hoveredSlot != null)
			{
				ghost.transform.position = hoveredSlot.Cart.transform.TransformPoint(hoveredSlot.LocalPosition);
				ghost.color = Color.green;
			}
			else
			{
				ghost.transform.position = vector;
				ghost.color = Color.red;
			}
			HandlePlacement();
		}

		private void HandlePlacement()
		{
			if (inputReader.PointerPressedThisFrame && hoveredSlot != null)
			{
				ModuleSlot result = hoveredSlot;
				Cleanup();
				tcs.TrySetResult(result);
			}
		}

		public async UniTask<ModuleSlot> SelectAsync(CancellationToken ct)
		{
			tcs = new UniTaskCompletionSource<ModuleSlot>();
			ct.RegisterWithoutCaptureExecutionContext(delegate
			{
				Cleanup();
				tcs.TrySetCanceled();
			});
			inputReader.OnCanceled += CancelSelection;
			try
			{
				return await tcs.Task;
			}
			finally
			{
				inputReader.OnCanceled -= CancelSelection;
			}
		}

		private void CancelSelection()
		{
			Cleanup();
			tcs.TrySetCanceled();
		}

		private void Cleanup()
		{
			isActive = false;
			hoveredSlot = null;
			if (ghost != null)
			{
				Object.Destroy(ghost.gameObject);
				ghost = null;
			}
		}

		private void OnDestroy()
		{
			if (isActive)
			{
				Cleanup();
				tcs?.TrySetCanceled();
			}
		}
	}
}
