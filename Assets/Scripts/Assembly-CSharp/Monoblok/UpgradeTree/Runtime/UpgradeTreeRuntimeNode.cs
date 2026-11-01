using System;
using System.Collections.Generic;
using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok.UpgradeTree.Runtime
{
	[Serializable]
	public class UpgradeTreeRuntimeNode
	{
		public enum RuntimeNodeType
		{
			Starting,
			Regular
		}

		public int ID;

		public RuntimeNodeType NodeType;

		public UpgradeData UpgradeData;

		public Price Price;

		public List<int> NextNodeIndices = new List<int>();

		public List<int> PreviousNodeIndices = new List<int>();

		public Vector2 Position;

		public UpgradeTreeRuntimeNode(int id, RuntimeNodeType type, UpgradeData upgradeData, Price price)
		{
			ID = id;
			NodeType = type;
			UpgradeData = upgradeData;
			Price = price;
		}
	}
}
