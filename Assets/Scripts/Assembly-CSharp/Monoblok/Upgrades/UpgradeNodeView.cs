using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	[RequireComponent(typeof(RectTransform))]
	public class UpgradeNodeView : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		[Serializable]
		private struct ColorBlock
		{
			public Color outerCirlceColor;

			public Color innerCircleColor;

			public Color iconColor;
		}

		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image outerCircle;

		[SerializeField]
		private Image innerCircle;

		[SerializeField]
		private Image icon;

		[SerializeField]
		private Image outline;

		[Header("Config")]
		[SerializeField]
		private ColorBlock disabledColors;

		[SerializeField]
		private ColorBlock enabledColors;

		[SerializeField]
		private ColorBlock unlockedColors;

		[SerializeField]
		private ShakeSettings shakeSettings;

		[SerializeField]
		private ShakeSettings punchScaleSettings;

		[SerializeField]
		private ParticleSystem unlockParticles;

		private bool initialized;

		private Tween yoyo;

		public UpgradeTreeNodeInstance Node { get; private set; }

		public event Action<UpgradeNodeView> OnNodeClicked;

		public event Action<UpgradeNodeView> OnNodePointerEnter;

		public event Action<UpgradeNodeView> OnNodePointerExit;

		public event Action<UpgradeNodeView> OnNodeSelected;

		public event Action<UpgradeNodeView> OnNodeDeselected;

		private void OnValidate()
		{
			if (rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();
			}
		}

		public void Initialize(UpgradeTreeNodeInstance node)
		{
			Node = node;
			rectTransform.position = node.Position;
			if (node.UpgradeData != null)
			{
				icon.sprite = node.UpgradeData.UIIcon;
			}
			button.onClick.AddListener(HandleButtonClicked);
			node.OnStateChanged += OnNodeStateChanged;
			OnNodeStateChanged(node.State, node.State);
			Debug.Log("Node initialized");
		}

		public void Shake()
		{
			Debug.Log("Shake");
			Tween.ShakeLocalPosition(base.transform, shakeSettings);
		}

		public void PlayUnlockAnimation()
		{
			Tween.PunchScale(base.transform, punchScaleSettings);
			unlockParticles.Play();
		}

		private void OnNodeStateChanged(UpgradeTreeNodeInstance.NodeState prevState, UpgradeTreeNodeInstance.NodeState newState)
		{
			bool flag = !initialized;
			initialized = true;
			switch (newState)
			{
			case UpgradeTreeNodeInstance.NodeState.Hidden:
				base.transform.localScale = Vector2.zero;
				button.gameObject.SetActive(value: false);
				break;
			case UpgradeTreeNodeInstance.NodeState.Unavailable:
				if (prevState == UpgradeTreeNodeInstance.NodeState.Hidden || flag)
				{
					Tween.Scale(base.transform, 1f, 0.15f, Ease.OutBack);
				}
				button.gameObject.SetActive(value: true);
				SetColorBlock(disabledColors);
				break;
			case UpgradeTreeNodeInstance.NodeState.Available:
				if (prevState == UpgradeTreeNodeInstance.NodeState.Hidden || flag)
				{
					Tween.Scale(base.transform, 1f, 0.15f, Ease.OutBack);
				}
				button.gameObject.SetActive(value: true);
				SetColorBlock(enabledColors);
				break;
			case UpgradeTreeNodeInstance.NodeState.Completed:
				button.gameObject.SetActive(value: true);
				SetColorBlock(unlockedColors);
				break;
			default:
				throw new NotImplementedException($"{newState} is not supported");
			}
		}

		private void SetColorBlock(ColorBlock colorBlock)
		{
			innerCircle.color = colorBlock.innerCircleColor;
			outerCircle.color = colorBlock.outerCirlceColor;
			icon.color = colorBlock.iconColor;
		}

		private void HandleButtonClicked()
		{
			Debug.Log("Node clicked");
			this.OnNodeClicked?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (Node != null && Node.State != 0)
			{
				Tween.Scale(base.transform, 1.2f, 0.15f);
				this.OnNodePointerEnter?.Invoke(this);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (Node != null && Node.State != 0)
			{
				Tween.Scale(base.transform, 1f, 0.15f);
				this.OnNodePointerExit?.Invoke(this);
			}
		}

		public void OnSelect(BaseEventData eventData)
		{
			Debug.Log("NODE SELECTED");
			if (Node != null && Node.State != 0)
			{
				this.OnNodeSelected?.Invoke(this);
				if (!(outline == null))
				{
					outline.gameObject.SetActive(value: true);
					yoyo.Complete();
					yoyo.Stop();
					yoyo = Tween.Scale(outline.transform, 1.1f, 0.5f, default(Easing), -1, CycleMode.Yoyo);
				}
			}
		}

		public void OnDeselect(BaseEventData eventData)
		{
			if (Node != null && Node.State != 0)
			{
				this.OnNodeDeselected?.Invoke(this);
				if (!(outline == null))
				{
					outline.gameObject.SetActive(value: false);
					yoyo.Complete();
					yoyo.Stop();
					Tween.StopAll(outline.transform);
				}
			}
		}
	}
}
