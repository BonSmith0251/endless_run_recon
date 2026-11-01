using System.Collections.Generic;
using Monoblok.StatSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Cart Data")]
	public class CartData : IDScriptableObject
	{
		[SerializeField]
		private LocalizedString localizedName;

		[SerializeField]
		private LocalizedString localizedDescription;

		[SerializeField]
		[HideInInspector]
		private Vector2[] slotLocalPositions;

		private const float FootprintAlphaThreshold = 0.1f;

		[field: SerializeField]
		public CartTier Tier { get; private set; }

		[field: SerializeField]
		public float FootPrint { get; private set; }

		public string CartName => localizedName.GetLocalizedString();

		public string Description => localizedDescription.GetLocalizedString();

		public LocalizedString LocalizedDescription => localizedDescription;

		[field: SerializeField]
		public Sprite CartSprite { get; private set; }

		[field: SerializeField]
		public StatModifierFactory ModuleStatModifierFactory { get; private set; }

		[field: SerializeField]
		public CartData NextTier { get; private set; }

		[field: SerializeField]
		public bool Orderable { get; private set; } = true;


		[field: SerializeField]
		public bool CanMerge { get; private set; } = true;


		[field: SerializeField]
		public bool ShowModuleVisuals { get; private set; } = true;


		public IReadOnlyList<Vector2> SlotLocalPositions => slotLocalPositions;

		[ContextMenu("Calculate Footprint")]
		private void CalculateFootprint()
		{
			Texture2D texture = CartSprite.texture;
			Vector2Int vector2Int = Vector2Int.RoundToInt(CartSprite.textureRect.position) + new Vector2Int((int)CartSprite.pivot.x, (int)CartSprite.pivot.y);
			vector2Int.x = Mathf.Clamp(vector2Int.x, 0, texture.width - 1);
			vector2Int.y = Mathf.Clamp(vector2Int.y, 0, texture.height - 1);
			Vector2Int vector2Int2 = new Vector2Int(-2, 1);
			int num = texture.width + texture.height;
			int x = vector2Int.x;
			int y = vector2Int.y;
			int dx = Mathf.Abs(vector2Int2.x);
			int dy = Mathf.Abs(vector2Int2.y);
			int sx = ((vector2Int2.x >= 0) ? 1 : (-1));
			int sy = ((vector2Int2.y >= 0) ? 1 : (-1));
			int err = dx - dy;
			int num2 = 0;
			Color pixel2;
			while (true)
			{
				if (!TryGetPixel(out pixel2))
				{
					Debug.LogWarning("CartData '" + base.name + "': ray left the texture bounds before finding a non-transparent pixel.", this);
					return;
				}
				if (pixel2.a > 0.1f)
				{
					break;
				}
				if (++num2 > num)
				{
					Debug.LogWarning("CartData '" + base.name + "': no non-transparent pixel found along the footprint ray.", this);
					return;
				}
				Step();
			}
			Vector2Int vector2Int3 = new Vector2Int(x, y);
			num2 = 0;
			while (true)
			{
				Step();
				if (!TryGetPixel(out pixel2))
				{
					Debug.LogWarning("CartData '" + base.name + "': ray left the texture bounds while still inside the opaque region.", this);
					return;
				}
				if (pixel2.a <= 0.1f)
				{
					break;
				}
				if (++num2 > num)
				{
					Debug.LogWarning("CartData '" + base.name + "': ray never exited the opaque region.", this);
					return;
				}
			}
			float footPrint = (new Vector2Int(x, y) - vector2Int3).magnitude / CartSprite.pixelsPerUnit;
			FootPrint = footPrint;
			void Step()
			{
				int num3 = 2 * err;
				if (num3 > -dy)
				{
					err -= dy;
					x += sx;
				}
				if (num3 < dx)
				{
					err += dx;
					y += sy;
				}
			}
			bool TryGetPixel(out Color pixel)
			{
				if (x < 0 || x >= texture.width || y < 0 || y >= texture.height)
				{
					pixel = default(Color);
					return false;
				}
				pixel = texture.GetPixel(x, y);
				return true;
			}
		}
	}
}
