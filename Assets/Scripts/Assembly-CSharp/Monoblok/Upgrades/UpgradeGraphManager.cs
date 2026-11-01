using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Monoblok.Input;
using Monoblok.Inventory;
using Monoblok.SA;
using Monoblok.UpgradeTree.Runtime;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class UpgradeGraphManager : MonoBehaviour
	{
		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private UpgradeTreeRuntimeGraph runtimeGraph;

		[SerializeField]
		private CurrencyDefinition metaCurrency;

		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private UpgradeGraphManagerView managerView;

		[Header("SFX")]
		[SerializeField]
		private EventReference purchaseRef;

		[SerializeField]
		private EventReference cannotPurchaseRef;

		[Header("Game Events")]
		[SerializeField]
		private GameEvent purchaseGameEvent;

		private UpgradeTreeInstance treeInstance;

		private Dictionary<int, List<UpgradeTreeNodeInstance>> parentsMap;

		private Dictionary<int, List<UpgradeTreeNodeInstance>> childrenMap;

		private List<IUpgrade> activeUpgrades = new List<IUpgrade>();

		public event Action OnExitRequested;

		private void OnEnable()
		{
			managerView.OnExitButtonClicked += HandleExitClicked;
			managerView.OnRefundClicked += RefundAll;
			managerView.OnNotificationAccepted += HandleNotificationAccepted;
			managerView.OnNodeClicked += OnNodeClicked;
		}

		private void OnDisable()
		{
			managerView.OnExitButtonClicked -= HandleExitClicked;
			managerView.OnRefundClicked -= RefundAll;
			managerView.OnNotificationAccepted -= HandleNotificationAccepted;
			managerView.OnNodeClicked -= OnNodeClicked;
		}

		public bool AllUnlocked()
		{
			foreach (UpgradeTreeNodeInstance node in treeInstance.Nodes)
			{
				if (node.State != UpgradeTreeNodeInstance.NodeState.Completed)
				{
					return false;
				}
			}
			return true;
		}

		private void HandleExitClicked()
		{
			this.OnExitRequested?.Invoke();
		}

		private void HandleNotificationAccepted()
		{
			this.OnExitRequested?.Invoke();
		}

		public void Initialize()
		{
			treeInstance = new UpgradeTreeInstance(runtimeGraph);
			BuildGraphCaches();
			SubscribeToNodeEvents();
			InitializeStates();
			managerView.Initialize(treeInstance);
		}

		public void OpenGraph()
		{
			EvaluateNode(treeInstance.StartingNode);
			ReevaluateAllNodes();
			managerView.gameObject.SetActive(value: true);
		}

		public void CloseGraph()
		{
			managerView.gameObject.SetActive(value: false);
		}

		private void RefundAll()
		{
			foreach (UpgradeTreeNodeInstance node in treeInstance.Nodes)
			{
				if (node.State == UpgradeTreeNodeInstance.NodeState.Completed)
				{
					wallet.AddCurrency(node.Price.Currency, node.Price.Amount.Value);
				}
			}
			foreach (IUpgrade activeUpgrade in activeUpgrades)
			{
				activeUpgrade.Remove();
			}
			activeUpgrades.Clear();
			InitializeStates();
			EvaluateNode(treeInstance.StartingNode);
			ReevaluateAllNodes();
		}

		private void BuildGraphCaches()
		{
			parentsMap = new Dictionary<int, List<UpgradeTreeNodeInstance>>();
			childrenMap = new Dictionary<int, List<UpgradeTreeNodeInstance>>();
			foreach (UpgradeTreeNodeInstance node in treeInstance.Nodes)
			{
				childrenMap[node.ID] = new List<UpgradeTreeNodeInstance>();
				foreach (int nextNodeIndex in node.NextNodeIndices)
				{
					UpgradeTreeNodeInstance item = treeInstance.Nodes[nextNodeIndex];
					childrenMap[node.ID].Add(item);
					if (!parentsMap.TryGetValue(nextNodeIndex, out var value))
					{
						value = new List<UpgradeTreeNodeInstance>();
						parentsMap[nextNodeIndex] = value;
					}
					value.Add(node);
				}
			}
		}

		private void SubscribeToNodeEvents()
		{
			foreach (UpgradeTreeNodeInstance node in treeInstance.Nodes)
			{
				node.OnStateChanged += delegate(UpgradeTreeNodeInstance.NodeState prev, UpgradeTreeNodeInstance.NodeState current)
				{
					if (current == UpgradeTreeNodeInstance.NodeState.Completed)
					{
						OnNodeCompleted(node);
					}
				};
			}
		}

		private void InitializeStates()
		{
			foreach (UpgradeTreeNodeInstance node in treeInstance.Nodes)
			{
				node.SetState(UpgradeTreeNodeInstance.NodeState.Hidden);
			}
			EvaluateNode(treeInstance.StartingNode);
		}

		private void OnNodeClicked(UpgradeNodeView view)
		{
			UpgradeTreeNodeInstance node = view.Node;
			if (node.State != UpgradeTreeNodeInstance.NodeState.Available)
			{
				view.Shake();
				RuntimeManager.PlayOneShot(cannotPurchaseRef);
			}
			else if (wallet.CanAfford(node.Price))
			{
				UnlockNode(node);
				wallet.Spend(node.Price);
				RuntimeManager.PlayOneShot(purchaseRef);
				purchaseGameEvent.TriggerGameEvent();
				view.PlayUnlockAnimation();
				ReevaluateAllNodes();
			}
		}

		private void UnlockNode(UpgradeTreeNodeInstance node)
		{
			Debug.Log("Unlocking Node");
			if (node.State != UpgradeTreeNodeInstance.NodeState.Completed)
			{
				node.SetState(UpgradeTreeNodeInstance.NodeState.Completed);
				IUpgrade upgrade = node.UpgradeData.GetUpgrade();
				if (upgrade != null)
				{
					upgrade.Apply();
					activeUpgrades.Add(upgrade);
					Debug.Log(node.UpgradeData.name + ": Node effect applied");
				}
			}
		}

		private void OnNodeCompleted(UpgradeTreeNodeInstance node)
		{
			if (!childrenMap.TryGetValue(node.ID, out var value))
			{
				return;
			}
			foreach (UpgradeTreeNodeInstance item in value)
			{
				EvaluateNode(item);
			}
		}

		private void EvaluateNode(UpgradeTreeNodeInstance node)
		{
			if (node.State != UpgradeTreeNodeInstance.NodeState.Completed)
			{
				if (!ParentsCompleted(node))
				{
					node.SetState(UpgradeTreeNodeInstance.NodeState.Hidden);
				}
				else if (!RequirementsMet(node))
				{
					node.SetState(UpgradeTreeNodeInstance.NodeState.Unavailable);
				}
				else
				{
					node.SetState(UpgradeTreeNodeInstance.NodeState.Available);
				}
			}
		}

		private bool ParentsCompleted(UpgradeTreeNodeInstance node)
		{
			if (!parentsMap.TryGetValue(node.ID, out var value))
			{
				return true;
			}
			foreach (UpgradeTreeNodeInstance item in value)
			{
				if (item.State != UpgradeTreeNodeInstance.NodeState.Completed)
				{
					return false;
				}
			}
			return true;
		}

		private bool RequirementsMet(UpgradeTreeNodeInstance node)
		{
			return wallet.CanAfford(node.Price);
		}

		private void ReevaluateAllNodes()
		{
			foreach (UpgradeTreeNodeInstance node in treeInstance.Nodes)
			{
				if (node.State != UpgradeTreeNodeInstance.NodeState.Completed)
				{
					EvaluateNode(node);
				}
			}
		}

		public UpgradeGraphSaveData GetSaveData()
		{
			return new UpgradeGraphSaveData
			{
				unlockedNodes = (from n in treeInstance.Nodes
					where n.State == UpgradeTreeNodeInstance.NodeState.Completed
					select n.ID).ToList()
			};
		}

		public void LoadSaveData(UpgradeGraphSaveData data)
		{
			if (treeInstance == null)
			{
				Debug.LogError("Tree instance is still null when trying to load");
				return;
			}
			foreach (IUpgrade activeUpgrade in activeUpgrades)
			{
				activeUpgrade.Remove();
			}
			activeUpgrades.Clear();
			InitializeStates();
			foreach (int unlockedNode in data.unlockedNodes)
			{
				bool flag = false;
				foreach (UpgradeTreeNodeInstance node in treeInstance.Nodes)
				{
					if (node.ID == unlockedNode)
					{
						UnlockNode(node);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Debug.LogError($"No node found with id {unlockedNode} when trying to load");
				}
			}
			EvaluateNode(treeInstance.StartingNode);
			ReevaluateAllNodes();
		}
	}
}
