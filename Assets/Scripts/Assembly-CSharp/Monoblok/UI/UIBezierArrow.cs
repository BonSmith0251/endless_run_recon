using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.UI
{
	[RequireComponent(typeof(CanvasRenderer))]
	public class UIBezierArrow : Graphic
	{
		[Header("Curve")]
		[SerializeField]
		private Vector2 _startPoint = new Vector2(-100f, 0f);

		[SerializeField]
		private Vector2 _endPoint = new Vector2(100f, 0f);

		[Tooltip("Absolute canvas-space offset from the start point that defines the first control point.")]
		[SerializeField]
		private Vector2 _startTangentOffset = new Vector2(0f, 150f);

		[Tooltip("Absolute canvas-space offset from the end point that defines the second control point.")]
		[SerializeField]
		private Vector2 _endTangentOffset = new Vector2(0f, 150f);

		[Range(4f, 128f)]
		[SerializeField]
		private int _segments = 32;

		[Header("Width")]
		[Tooltip("Base width of the curve in canvas units.")]
		[SerializeField]
		private float _width = 20f;

		[Tooltip("Width multiplier along the curve. X axis = 0 (tail) to 1 (tip). Y axis = width multiplier.")]
		[SerializeField]
		private AnimationCurve _widthCurve = AnimationCurve.Linear(0f, 0.3f, 1f, 1f);

		[Tooltip("Width of the transparent feather region on each edge, in canvas units. 0 = hard aliased edge.")]
		[SerializeField]
		[Min(0f)]
		private float _edgeSoftness = 4f;

		[Header("Arrowhead")]
		[SerializeField]
		private bool _showArrowhead = true;

		[SerializeField]
		private float _arrowheadLength = 30f;

		[SerializeField]
		private float _arrowheadWidth = 30f;

		[Tooltip("Optional sprite for the arrowhead. Leave null to use a plain filled triangle.")]
		[SerializeField]
		private Sprite _arrowheadSprite;

		private readonly List<Vector2> _sampledPoints = new List<Vector2>(129);

		private readonly List<float> _sampledT = new List<float>(129);

		public override Texture mainTexture
		{
			get
			{
				if (_arrowheadSprite != null)
				{
					return _arrowheadSprite.texture;
				}
				return base.mainTexture;
			}
		}

		public void SetPoints(Vector2 start, Vector2 end)
		{
			_startPoint = start;
			_endPoint = end;
			SetVerticesDirty();
		}

		public void SetEndPoint(Vector2 end)
		{
			_endPoint = end;
			SetVerticesDirty();
		}

		public void SetStartPoint(Vector2 start)
		{
			_startPoint = start;
			SetVerticesDirty();
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			SampleCurve();
			if (_sampledPoints.Count >= 2)
			{
				int num = _sampledPoints.Count;
				if (_showArrowhead && num >= 2)
				{
					num = FindBodyEndIndex();
				}
				AddCurveBody(vh, num);
				if (_showArrowhead)
				{
					AddArrowhead(vh);
				}
			}
		}

		private void SampleCurve()
		{
			_sampledPoints.Clear();
			_sampledT.Clear();
			Vector2 c = _startPoint + _startTangentOffset;
			Vector2 c2 = _endPoint + _endTangentOffset;
			for (int i = 0; i <= _segments; i++)
			{
				float num = (float)i / (float)_segments;
				_sampledPoints.Add(EvaluateCubic(_startPoint, c, c2, _endPoint, num));
				_sampledT.Add(num);
			}
		}

		private static Vector2 EvaluateCubic(Vector2 p0, Vector2 c0, Vector2 c1, Vector2 p1, float t)
		{
			float num = 1f - t;
			float num2 = num * num;
			float num3 = num2 * num;
			float num4 = t * t;
			float num5 = num4 * t;
			return num3 * p0 + 3f * num2 * t * c0 + 3f * num * num4 * c1 + num5 * p1;
		}

		private void AddCurveBody(VertexHelper vh, int pointCount)
		{
			if (pointCount >= 2)
			{
				int currentVertCount = vh.currentVertCount;
				for (int i = 0; i < pointCount; i++)
				{
					Vector2 vector = _sampledPoints[i];
					float num = _sampledT[i];
					float num2 = _width * _widthCurve.Evaluate(num) * 0.5f;
					float num3 = Mathf.Min(_edgeSoftness, num2);
					Vector2 vector2 = ComputeMiter(i, pointCount);
					Color c = color;
					Color c2 = new Color(color.r, color.g, color.b, 0f);
					AddVertColor(vh, vector + vector2 * (num2 + num3), num, c2);
					AddVertColor(vh, vector + vector2 * (num2 - num3), num, c);
					AddVertColor(vh, vector - vector2 * (num2 - num3), num, c);
					AddVertColor(vh, vector - vector2 * (num2 + num3), num, c2);
				}
				for (int j = 0; j < pointCount - 1; j++)
				{
					int num4 = currentVertCount + j * 4;
					vh.AddTriangle(num4 + 1, num4 + 5, num4 + 2);
					vh.AddTriangle(num4 + 2, num4 + 5, num4 + 6);
					vh.AddTriangle(num4, num4 + 4, num4 + 1);
					vh.AddTriangle(num4 + 1, num4 + 4, num4 + 5);
					vh.AddTriangle(num4 + 2, num4 + 6, num4 + 3);
					vh.AddTriangle(num4 + 3, num4 + 6, num4 + 7);
				}
			}
		}

		private Vector2 ComputeMiter(int i, int pointCount)
		{
			Vector2 vector = Vector2.zero;
			Vector2 vector2 = Vector2.zero;
			if (i > 0)
			{
				vector = SegmentNormal(_sampledPoints[i - 1], _sampledPoints[i]);
			}
			if (i < pointCount - 1)
			{
				vector2 = SegmentNormal(_sampledPoints[i], _sampledPoints[i + 1]);
			}
			if (i == 0)
			{
				return vector2;
			}
			if (i == pointCount - 1)
			{
				return vector;
			}
			Vector2 normalized = (vector + vector2).normalized;
			float num = Vector2.Dot(normalized, vector);
			float value = ((Mathf.Abs(num) > 0.1f) ? (1f / num) : 10f);
			value = Mathf.Clamp(value, 0f, 4f);
			return normalized * value;
		}

		private static Vector2 SegmentNormal(Vector2 a, Vector2 b)
		{
			Vector2 vector = b - a;
			float magnitude = vector.magnitude;
			if (magnitude < 0.0001f)
			{
				return Vector2.up;
			}
			return new Vector2(0f - vector.y, vector.x) / magnitude;
		}

		private void AddArrowhead(VertexHelper vh)
		{
			Vector2 vector = _sampledPoints[_sampledPoints.Count - 1];
			Vector2 vector2 = _endPoint + _endTangentOffset;
			Vector2 vector3 = 3f * (_endPoint - vector2);
			float magnitude = vector3.magnitude;
			if (magnitude < 0.0001f)
			{
				vector3 = vector - _sampledPoints[_sampledPoints.Count - 2];
				magnitude = vector3.magnitude;
			}
			if (!(magnitude < 0.0001f))
			{
				vector3 /= magnitude;
				Vector2 vector4 = new Vector2(0f - vector3.y, vector3.x);
				Vector2 vector5 = vector - vector3 * _arrowheadLength;
				Vector2 position = vector5 + vector4 * (_arrowheadWidth * 0.5f);
				Vector2 position2 = vector5 - vector4 * (_arrowheadWidth * 0.5f);
				int currentVertCount = vh.currentVertCount;
				if (_arrowheadSprite != null)
				{
					Rect rect = _arrowheadSprite.rect;
					Rect rect2 = new Rect(rect.x / (float)_arrowheadSprite.texture.width, rect.y / (float)_arrowheadSprite.texture.height, rect.width / (float)_arrowheadSprite.texture.width, rect.height / (float)_arrowheadSprite.texture.height);
					Vector2 uv = new Vector2(rect2.x + rect2.width * 0.5f, rect2.y + rect2.height);
					Vector2 uv2 = new Vector2(rect2.x, rect2.y);
					Vector2 uv3 = new Vector2(rect2.x + rect2.width, rect2.y);
					AddVertUV(vh, vector, uv);
					AddVertUV(vh, position, uv2);
					AddVertUV(vh, position2, uv3);
				}
				else
				{
					AddVert(vh, vector, 1f);
					AddVert(vh, position, 1f);
					AddVert(vh, position2, 1f);
				}
				vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			}
		}

		private void AddVert(VertexHelper vh, Vector2 position, float t)
		{
			AddVertColor(vh, position, t, color);
		}

		private void AddVertColor(VertexHelper vh, Vector2 position, float t, Color c)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = c;
			simpleVert.position = position;
			simpleVert.uv0 = new Vector2(t, 0.5f);
			vh.AddVert(simpleVert);
		}

		private void AddVertUV(VertexHelper vh, Vector2 position, Vector2 uv)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = color;
			simpleVert.position = position;
			simpleVert.uv0 = uv;
			vh.AddVert(simpleVert);
		}

		private int FindBodyEndIndex()
		{
			float num = 0f;
			int count = _sampledPoints.Count;
			for (int num2 = count - 1; num2 > 0; num2--)
			{
				num += Vector2.Distance(_sampledPoints[num2], _sampledPoints[num2 - 1]);
				if (num >= _arrowheadLength)
				{
					return Mathf.Clamp(num2 + 1, 2, count);
				}
			}
			return 2;
		}
	}
}
