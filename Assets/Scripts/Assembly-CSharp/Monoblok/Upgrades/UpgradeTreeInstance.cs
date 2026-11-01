using System.Collections.Generic;
using Monoblok.UpgradeTree.Runtime;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class UpgradeTreeInstance
	{
		private readonly List<UpgradeTreeNodeInstance> nodes = new List<UpgradeTreeNodeInstance>();

		private UpgradeTreeNodeInstance startingNode;

		public IReadOnlyList<UpgradeTreeNodeInstance> Nodes => nodes;

		public UpgradeTreeNodeInstance StartingNode => startingNode;

		public UpgradeTreeInstance(UpgradeTreeRuntimeGraph runtimeGraph)
		{
			BuildNodes(runtimeGraph);
			ValidateTree(runtimeGraph);
		}

		private void BuildNodes(UpgradeTreeRuntimeGraph runtimeGraph)
		{
			foreach (UpgradeTreeRuntimeNode node in runtimeGraph.Nodes)
			{
				UpgradeTreeNodeInstance item = new UpgradeTreeNodeInstance(node);
				nodes.Add(item);
				if (node.NodeType == UpgradeTreeRuntimeNode.RuntimeNodeType.Starting)
				{
					startingNode = item;
				}
			}
		}

		private void ValidateTree(UpgradeTreeRuntimeGraph runtimeGraph)
		{
			if (runtimeGraph.Nodes.Count != nodes.Count)
			{
				Debug.LogError($"Graph's node count {runtimeGraph.Nodes.Count} is not equal to tree's node count {nodes.Count}");
			}
			if (startingNode == null)
			{
				Debug.LogError("No Strating Node found");
			}
		}
	}
}
