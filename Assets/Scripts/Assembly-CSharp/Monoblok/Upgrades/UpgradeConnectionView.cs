using PrimeTween;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class UpgradeConnectionView : MonoBehaviour
	{
		[SerializeField]
		private LineRenderer lineRenderer;

		[SerializeField]
		private Color inactiveColor;

		[SerializeField]
		private Color activeColor;

		private UpgradeTreeNodeInstance from;

		private UpgradeTreeNodeInstance to;

		public void Initialize(UpgradeTreeNodeInstance from, UpgradeTreeNodeInstance to)
		{
			this.from = from;
			this.to = to;
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, from.Position);
			lineRenderer.SetPosition(1, from.Position);
			from.OnStateChanged += OnFromStateChanged;
			to.OnStateChanged += OnToStateChanged;
			RefreshImmediate();
		}

		private void RefreshImmediate()
		{
			Vector3 position = from.Position;
			Vector3 position2 = to.Position;
			lineRenderer.SetPosition(0, position);
			if (from.State == UpgradeTreeNodeInstance.NodeState.Completed)
			{
				lineRenderer.SetPosition(1, position2);
			}
			else
			{
				lineRenderer.SetPosition(1, position);
			}
			RefreshVisibility();
			RefreshVisualState();
		}

		private void OnFromStateChanged(UpgradeTreeNodeInstance.NodeState prev, UpgradeTreeNodeInstance.NodeState current)
		{
			if (current == UpgradeTreeNodeInstance.NodeState.Completed)
			{
				AnimateReveal();
			}
			else if (prev == UpgradeTreeNodeInstance.NodeState.Completed)
			{
				CollapseImmediate();
			}
			RefreshVisualState();
		}

		private void OnToStateChanged(UpgradeTreeNodeInstance.NodeState prev, UpgradeTreeNodeInstance.NodeState current)
		{
			RefreshVisibility();
			RefreshVisualState();
		}

		private void RefreshVisibility()
		{
			lineRenderer.enabled = to.State != UpgradeTreeNodeInstance.NodeState.Hidden;
		}

		private void CollapseImmediate()
		{
			Vector3 position = from.Position;
			lineRenderer.SetPosition(0, position);
			lineRenderer.SetPosition(1, position);
			RefreshVisualState();
		}

		private void AnimateReveal()
		{
			Vector3 start = from.Position;
			Vector3 end = to.Position;
			float duration = 0.2f;
			Tween.Custom(0f, 1f, duration, delegate(float t)
			{
				Vector3 position = Vector3.Lerp(start, end, t);
				lineRenderer.SetPosition(0, start);
				lineRenderer.SetPosition(1, position);
			});
		}

		private void RefreshVisualState()
		{
			if (from.State == UpgradeTreeNodeInstance.NodeState.Completed)
			{
				if (to.State == UpgradeTreeNodeInstance.NodeState.Available)
				{
					SetColor(inactiveColor);
				}
				else if (to.State == UpgradeTreeNodeInstance.NodeState.Completed)
				{
					SetColor(activeColor);
				}
				else
				{
					SetColor(inactiveColor);
				}
			}
			else
			{
				SetColor(inactiveColor);
			}
		}

		private void SetColor(Color color)
		{
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
		}

		private void OnDestroy()
		{
			if (from != null)
			{
				from.OnStateChanged -= OnFromStateChanged;
			}
			if (to != null)
			{
				to.OnStateChanged -= OnToStateChanged;
			}
		}
	}
}
