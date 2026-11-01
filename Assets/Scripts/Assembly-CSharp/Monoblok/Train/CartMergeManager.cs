using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Monoblok.Core;
using Monoblok.Input;
using Monoblok.SA;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using ZLinq;

namespace Monoblok.Train
{
	public class CartMergeManager : MonoBehaviour
	{
		public enum SelectionState
		{
			Idle,
			Selecting,
			Complete,
			Canceled,
			Forbidden
		}

		[SerializeField]
		private Train train;

		[SerializeField]
		private CartSelector cartSelector;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private MergeConnectionRuntimeCollection activeMergeCollection;

		[SerializeField]
		private CanvasGroup overlayCanvasGroup;

		[SerializeField]
		private GameObject tooltip;

		[SerializeField]
		private EventReference startMergeRef;

		[SerializeField]
		private EventReference executeMergeRef;

		[Space]
		[SerializeField]
		private GameEvent mergeEvent;

		private List<CartMergeConnection> mergeConnections = new List<CartMergeConnection>();

		private Observable<SelectionState> state = new Observable<SelectionState>(SelectionState.Idle);

		private UniTaskCompletionSource<TrainCart> selectionTcs;

		private GameObject lastSelected;

		private TrainCart _pendingSource;

		private Tween yoyoSeq;

		public IReadOnlyList<CartMergeConnection> MergeConnections => mergeConnections;

		public Observable<SelectionState> State => state;

		public event Action OnMergeSequenceComplete;

		private void Awake()
		{
			PruneDeadConnections();
			train.OnCartRemoved += HandleCartRemoved;
		}

		private void OnDestroy()
		{
			train.OnCartRemoved -= HandleCartRemoved;
			ClearConnections();
		}

		private void PruneDeadConnections()
		{
			foreach (CartMergeConnection item in (from c in activeMergeCollection.Collection.AsValueEnumerable()
				where c.Parent == null || c.Child == null
				select c).ToList())
			{
				activeMergeCollection.Unregister(item);
			}
		}

		private void HandleCartRemoved(TrainCart cart)
		{
			CartMergeConnection mergeConnection = mergeConnections.AsValueEnumerable().FirstOrDefault((CartMergeConnection c) => c.Parent == cart || c.Child == cart);
			Dispose(mergeConnection);
		}

		public void CancelSelection()
		{
			if (selectionTcs != null)
			{
				selectionTcs.TrySetCanceled();
			}
		}

		public async UniTaskVoid StartSelection(TrainCart source)
		{
			if (state.Value != 0)
			{
				return;
			}
			tooltip.gameObject.SetActive(value: true);
			Tween.PunchScale(tooltip.transform, Vector3.one * 0.5f, 0.15f, 2f);
			source.SpriteRenderer.color = Unity6Compat.CadetBlue;
			yoyoSeq = Tween.Scale(source.transform, 1.05f, 0.2f, default(Easing), -1, CycleMode.Yoyo);
			lastSelected = EventSystem.current.currentSelectedGameObject;
			overlayCanvasGroup.interactable = false;
			_pendingSource = source;
			await UniTask.Yield();
			try
			{
				state.SetValue(SelectionState.Selecting);
				selectionTcs = new UniTaskCompletionSource<TrainCart>();
				inputReader.OnCanceled += HandleCanceled;
				cartSelector.OnSelectionChanged += HandleSelectionChanged;
				while (!selectionTcs.Task.Status.IsCompleted())
				{
					if (InputManager.CurrentDevice != InputManager.InputDeviceType.KBM && inputReader.SubmitPressed)
					{
						TryConfirm(source, cartSelector.SelectedCart);
					}
					await UniTask.Yield();
				}
				TrainCart target = await selectionTcs.Task;
				state.SetValue(SelectionState.Complete);
				StartMerge(source, target);
			}
			catch (OperationCanceledException)
			{
				state.SetValue(SelectionState.Canceled);
			}
			finally
			{
				cartSelector.OnSelectionChanged -= HandleSelectionChanged;
				inputReader.OnCanceled -= HandleCanceled;
				selectionTcs = null;
				overlayCanvasGroup.interactable = true;
				EventSystem.current.SetSelectedGameObject(lastSelected);
				lastSelected = null;
				if (source != null)
				{
					source.SpriteRenderer.color = Color.white;
					source.transform.localScale = Vector3.one;
				}
				yoyoSeq.Complete();
				yoyoSeq.Stop();
				state.SetValue(SelectionState.Idle);
				tooltip.gameObject.SetActive(value: false);
			}
		}

		private void HandleSelectionChanged(TrainCart cart)
		{
			if (InputManager.CurrentDevice == InputManager.InputDeviceType.KBM)
			{
				TryConfirm(_pendingSource, cart);
			}
		}

		private void TryConfirm(TrainCart source, TrainCart target)
		{
			if (!CanMerge(source, target))
			{
				Debug.Log("Not eligible");
				state.SetValue(SelectionState.Forbidden);
			}
			else
			{
				selectionTcs.TrySetResult(target);
			}
		}

		private void HandleCanceled()
		{
			selectionTcs?.TrySetCanceled();
		}

		public void RegisterMergeConnection(CartMergeConnection connection)
		{
			mergeConnections.Add(connection);
			activeMergeCollection.Register(connection);
		}

		public void ClearConnections()
		{
			for (int num = mergeConnections.Count - 1; num >= 0; num--)
			{
				CartMergeConnection connection = mergeConnections[num];
				RemoveConnection(connection);
			}
		}

		private void RemoveConnection(CartMergeConnection connection)
		{
			train.UngroupUnit(connection.MergedUnit);
			connection.Dispose();
			activeMergeCollection.Unregister(connection);
			mergeConnections.Remove(connection);
		}

		public bool EligibleForMerge(TrainCart target)
		{
			if (target == null)
			{
				return false;
			}
			if (!target.CartData.CanMerge)
			{
				return false;
			}
			if (target.CartData.NextTier == null)
			{
				return false;
			}
			if (IsInActiveMerge(target))
			{
				return false;
			}
			return true;
		}

		private bool CanMerge(TrainCart source, TrainCart target)
		{
			if (source == target)
			{
				return false;
			}
			if (!EligibleForMerge(source) || !EligibleForMerge(target))
			{
				return false;
			}
			if (target.CartData.Tier != source.CartData.Tier)
			{
				return false;
			}
			return true;
		}

		private void StartMerge(TrainCart source, TrainCart target)
		{
			TrainCart trainCart = ((source.Index < target.Index) ? source : target);
			TrainCart child = ((source.Index < target.Index) ? target : source);
			int wavesRequired = WavesRequiredForTier(trainCart.CartData.Tier);
			CartMergeConnection connection = new CartMergeConnection(trainCart, child, wavesRequired);
			RegisterMergeConnection(connection);
			RuntimeManager.PlayOneShot(startMergeRef);
		}

		private void Dispose(CartMergeConnection mergeConnection)
		{
			if (mergeConnection != null && mergeConnections.Contains(mergeConnection))
			{
				mergeConnections.Remove(mergeConnection);
				activeMergeCollection.Unregister(mergeConnection);
			}
		}

		public void EvaluateActiveMergeConnections()
		{
			foreach (CartMergeConnection mergeConnection in mergeConnections)
			{
				mergeConnection.OnWaveCompleted();
			}
			List<CartMergeConnection> list = (from c in mergeConnections.AsValueEnumerable()
				where c.IsComplete
				select c).ToList();
			list.Sort((CartMergeConnection a, CartMergeConnection b) => train.UnitIndexOf(b.Parent).CompareTo(train.UnitIndexOf(a.Parent)));
			PrimeTween.Sequence sequence = PrimeTween.Sequence.Create();
			foreach (CartMergeConnection connection in list)
			{
				sequence.Chain(PrimeTween.Sequence.Create().Chain(Tween.PositionY(connection.Child.transform, connection.Child.transform.position.y + 1f, 0.15f, Ease.OutBack)).ChainDelay(0.15f)
					.Chain(Tween.PositionX(connection.Child.transform, connection.Parent.transform.position.x, 0.15f, Ease.OutBack))
					.ChainDelay(0.15f)
					.ChainCallback(this, delegate(CartMergeManager t)
					{
						RuntimeManager.PlayOneShot(t.executeMergeRef);
					})
					.Chain(Tween.PositionY(connection.Child.transform, connection.Parent.transform.position.y, 0.15f, Ease.InBack))
					.ChainCallback(this, delegate(CartMergeManager t)
					{
						t.ExecuteMerge(connection);
					})).ChainDelay(0.15f);
			}
			sequence.OnComplete(this, delegate(CartMergeManager t)
			{
				t.OnMergeSequenceComplete?.Invoke();
			});
		}

		private void ExecuteMerge(CartMergeConnection connection)
		{
			CartData nextTier = connection.Parent.CartData.NextTier;
			if (nextTier == null)
			{
				return;
			}
			int index = train.UnitIndexOf(connection.Parent);
			connection.Parent.ModuleManager.DetachAll(out var modules);
			connection.Child.ModuleManager.DetachAll(out var modules2);
			List<ModuleData> list = new List<ModuleData>();
			list.AddRange(modules);
			list.AddRange(modules2);
			train.RemoveCart(connection.Parent);
			train.RemoveCart(connection.Child);
			train.TryAddCart(nextTier, out var cart, index, animate: false);
			foreach (ModuleData item in list)
			{
				if (!cart.ModuleManager.TryAddModule(item, silent: true, animate: false))
				{
					Debug.LogError(item.Definition.DisplayName + " couldn't be migrated to new cart");
				}
			}
			Tween.PunchScale(cart.transform, Vector3.one * 1.3f, 0.2f, 5f);
			Dispose(connection);
			mergeEvent.TriggerGameEvent();
			Debug.Log("Merge event triggered");
		}

		private bool IsInActiveMerge(TrainCart cart)
		{
			return mergeConnections.AsValueEnumerable().Any((CartMergeConnection c) => c.Parent == cart || c.Child == cart);
		}

		private int WavesRequiredForTier(CartTier tier)
		{
			return tier.MergeWaveCount;
		}
	}
}
