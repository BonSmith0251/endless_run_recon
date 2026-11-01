using System;
using System.Collections.Generic;
using Monoblok.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	public class UpgradeTreeView : MonoBehaviour
	{
		[SerializeField]
		private Camera treeCam;

		[SerializeField]
		private UpgradeNodeView nodeViewPrefab;

		[SerializeField]
		private UpgradeConnectionView connectionPrefab;

		[SerializeField]
		private UpgradeNodeTooltip tooltip;

		private List<UpgradeNodeView> nodeViews = new List<UpgradeNodeView>();

		private List<UpgradeConnectionView> connections = new List<UpgradeConnectionView>();

		[SerializeField]
		private RectTransform nodeContainer;

		public event Action<UpgradeNodeView> OnNodeClicked;

		public event Action<UpgradeNodeView> OnNodeSelected;

		private void OnEnable()
		{
			InputManager.OnInputDeviceChanged += HandleInputDeviceChanged;
			HandleInputDeviceChanged(InputManager.CurrentDevice);
		}

		private void OnDisable()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
		}

		private void HandleInputDeviceChanged(InputManager.InputDeviceType device)
		{
			Debug.Log("Tree view device");
			GameObject gameObject = nodeViews[0].gameObject;
			EventSystem.current.SetSelectedGameObject(gameObject.gameObject);
			if (gameObject.TryGetComponent<ISelectHandler>(out var component))
			{
				component.OnSelect(new BaseEventData(EventSystem.current));
			}
		}

		public Bounds Initialize(UpgradeTreeInstance tree)
		{
			Clear();
			Bounds result = default(Bounds);
			bool flag = false;
			foreach (UpgradeTreeNodeInstance node in tree.Nodes)
			{
				UpgradeNodeView upgradeNodeView = UnityEngine.Object.Instantiate(nodeViewPrefab, nodeContainer);
				upgradeNodeView.Initialize(node);
				upgradeNodeView.OnNodeClicked += HandleNodeClicked;
				upgradeNodeView.OnNodeSelected += HandleNodeSelected;
				upgradeNodeView.OnNodeDeselected += HandleNodeDeselected;
				upgradeNodeView.OnNodePointerEnter += HandleNodePointerEnter;
				upgradeNodeView.OnNodePointerExit += HandleNodePointerExit;
				nodeViews.Add(upgradeNodeView);
				if (!flag)
				{
					result = new Bounds((Vector3)node.Position, (Vector3)Vector2.zero);
					flag = true;
				}
				else
				{
					result.Encapsulate((Vector3)node.Position);
				}
				for (int i = 0; i < node.NextNodeIndices.Count; i++)
				{
					UpgradeTreeNodeInstance to = tree.Nodes[node.NextNodeIndices[i]];
					UpgradeConnectionView upgradeConnectionView = UnityEngine.Object.Instantiate(connectionPrefab, nodeContainer);
					upgradeConnectionView.Initialize(node, to);
					connections.Add(upgradeConnectionView);
				}
			}
			return result;
		}

		private Button FindNearest(UpgradeNodeView self, Vector2 origin, Vector2 direction, List<UpgradeNodeView> all)
		{
			Button result = null;
			float num = float.MaxValue;
			foreach (UpgradeNodeView item in all)
			{
				if (item == self)
				{
					continue;
				}
				Vector2 vector = item.Node.Position - origin;
				float num2 = Vector2.Dot(vector.normalized, direction);
				if (!(num2 <= 0.5f))
				{
					float num3 = Vector2.Dot(vector, direction);
					float magnitude = (vector - direction * num3).magnitude;
					float num4 = (num3 + magnitude * magnitude) / num2;
					if (num4 < num)
					{
						num = num4;
						result = item.GetComponent<Button>();
					}
				}
			}
			return result;
		}

		public void Clear()
		{
			for (int num = nodeViews.Count - 1; num >= 0; num--)
			{
				UpgradeNodeView upgradeNodeView = nodeViews[num];
				if (!(upgradeNodeView == null))
				{
					upgradeNodeView.OnNodeClicked -= HandleNodeClicked;
					upgradeNodeView.OnNodeSelected -= HandleNodeSelected;
					upgradeNodeView.OnNodeDeselected -= HandleNodeDeselected;
					upgradeNodeView.OnNodePointerEnter -= HandleNodePointerEnter;
					upgradeNodeView.OnNodePointerExit -= HandleNodePointerExit;
					UnityEngine.Object.Destroy(upgradeNodeView.gameObject);
				}
			}
			nodeViews.Clear();
			for (int num2 = connections.Count - 1; num2 >= 0; num2--)
			{
				if (connections[num2] != null)
				{
					UnityEngine.Object.Destroy(connections[num2].gameObject);
				}
			}
			connections.Clear();
		}

		private void HandleNodePointerEnter(UpgradeNodeView view)
		{
			ShowTooltip(view);
		}

		private void HandleNodePointerExit(UpgradeNodeView view)
		{
			if (!(tooltip == null))
			{
				tooltip.Hide();
			}
		}

		private void HandleNodeClicked(UpgradeNodeView view)
		{
			this.OnNodeClicked?.Invoke(view);
		}

		private void HandleNodeSelected(UpgradeNodeView view)
		{
			this.OnNodeSelected?.Invoke(view);
			ShowTooltip(view);
		}

		private void HandleNodeDeselected(UpgradeNodeView view)
		{
			if (!(tooltip == null))
			{
				tooltip.Hide();
			}
		}

		private void ShowTooltip(UpgradeNodeView view)
		{
			if (!(tooltip == null))
			{
				tooltip.Initialize(view.Node);
				tooltip.Show(view);
			}
		}
	}
}
