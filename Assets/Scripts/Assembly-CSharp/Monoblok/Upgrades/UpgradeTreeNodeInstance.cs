using System;
using System.Collections.Generic;
using Monoblok.UpgradeTree.Runtime;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class UpgradeTreeNodeInstance
	{
		public enum NodeState
		{
			Hidden,
			Unavailable,
			Available,
			Completed
		}

		private readonly UpgradeTreeRuntimeNode nodeData;

		private NodeState state;

		private RuntimePrice price;

		public int ID => nodeData.ID;

		public UpgradeTreeRuntimeNode.RuntimeNodeType NodeType => nodeData.NodeType;

		public UpgradeData UpgradeData => nodeData.UpgradeData;

		public RuntimePrice Price => price;

		public List<int> NextNodeIndices => nodeData.NextNodeIndices;

		public Vector2 Position => nodeData.Position;

		public NodeState State => state;

		public event Action<NodeState, NodeState> OnStateChanged;

		public UpgradeTreeNodeInstance(UpgradeTreeRuntimeNode nodeData)
		{
			this.nodeData = nodeData;
			price = new RuntimePrice(nodeData.Price);
		}

		public void SetState(NodeState newState)
		{
			if (newState != state)
			{
				NodeState arg = state;
				state = newState;
				this.OnStateChanged?.Invoke(arg, state);
			}
		}
	}
}
