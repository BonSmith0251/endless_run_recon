using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace DigitalRuby.LightningBolt
{
	[RequireComponent(typeof(LineRenderer))]
	[ExecuteInEditMode]
	public class LightningBoltScript : MonoBehaviour
	{
		[Tooltip("The game object where the lightning will emit from. If null, StartPosition is used.")]
		public GameObject StartObject;

		[Tooltip("The start position where the lightning will emit from. This is in world space if StartObject is null, otherwise this is offset from StartObject position.")]
		public Vector3 StartPosition;

		[Tooltip("The game object where the lightning will end at. If null, EndPosition is used.")]
		public GameObject EndObject;

		[Tooltip("The end position where the lightning will end at. This is in world space if EndObject is null, otherwise this is offset from EndObject position.")]
		public Vector3 EndPosition;

		[Range(0f, 8f)]
		[Tooltip("How manu generations? Higher numbers create more line segments.")]
		public int Generations = 6;

		[Range(0.01f, 1f)]
		[Tooltip("How long each bolt should last before creating a new bolt. In ManualMode, the bolt will simply disappear after this amount of seconds.")]
		public float Duration = 0.05f;

		private float timer;

		[Range(0f, 1f)]
		[Tooltip("How chaotic should the lightning be? (0-1)")]
		public float ChaosFactor = 0.15f;

		[Tooltip("In manual mode, the trigger method must be called to create a bolt")]
		public bool ManualMode;

		[Range(1f, 64f)]
		[Tooltip("The number of rows in the texture. Used for animation.")]
		public int Rows = 1;

		[Range(1f, 64f)]
		[Tooltip("The number of columns in the texture. Used for animation.")]
		public int Columns = 1;

		[NonSerialized]
		[HideInInspector]
		public System.Random RandomGenerator = new System.Random();

		[SerializeField]
		private LineRenderer lineRenderer;

		private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();

		private int startIndex;

		private void OnValidate()
		{
			if (!(lineRenderer != null))
			{
				lineRenderer = GetComponent<LineRenderer>();
			}
		}

		private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount)
		{
			if (generation < 0 || generation > 8)
			{
				return;
			}
			start.z = (end.z = Mathf.Min(start.z, end.z));
			segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));
			if (generation == 0)
			{
				return;
			}
			if (offsetAmount <= 0f)
			{
				offsetAmount = (end - start).magnitude * ChaosFactor;
			}
			while (generation-- > 0 && segments.Count < 1000)
			{
				int num = startIndex;
				startIndex = segments.Count;
				for (int i = num; i < startIndex; i++)
				{
					if (segments.Count >= 1000)
					{
						break;
					}
					start = segments[i].Key;
					end = segments[i].Value;
					Vector3 vector = (start + end) * 0.5f;
					RandomVector(ref start, ref end, offsetAmount, out var result);
					vector += result;
					segments.Add(new KeyValuePair<Vector3, Vector3>(start, vector));
					segments.Add(new KeyValuePair<Vector3, Vector3>(vector, end));
				}
				offsetAmount *= 0.5f;
			}
		}

		public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
		{
			Vector3 normalized = (end - start).normalized;
			Vector3 vector = new Vector3(0f - normalized.y, normalized.x, normalized.z);
			float num = (float)RandomGenerator.NextDouble() * offsetAmount * 2f - offsetAmount;
			result = vector * num;
		}

		private void UpdateLineRenderer()
		{
			int num = segments.Count - startIndex + 1;
			lineRenderer.positionCount = num;
			if (num >= 1)
			{
				int num2 = 0;
				lineRenderer.SetPosition(num2++, segments[startIndex].Key);
				for (int i = startIndex; i < segments.Count; i++)
				{
					lineRenderer.SetPosition(num2++, segments[i].Value);
				}
				segments.Clear();
			}
		}

		private void Start()
		{
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.positionCount = 0;
		}

		private void Update()
		{
			if (timer <= 0f)
			{
				if (ManualMode)
				{
					timer = Duration;
					lineRenderer.positionCount = 0;
				}
				else
				{
					Trigger();
				}
			}
			timer -= Time.deltaTime;
		}

		[Button(null, EButtonEnableMode.Always)]
		public void Trigger()
		{
			timer = Duration + Mathf.Min(0f, timer);
			Vector3 start = ((!(StartObject == null)) ? (StartObject.transform.position + StartPosition) : StartPosition);
			Vector3 end = ((!(EndObject == null)) ? (EndObject.transform.position + EndPosition) : EndPosition);
			startIndex = 0;
			GenerateLightningBolt(start, end, Generations, Generations, 0f);
			UpdateLineRenderer();
		}

		public void Clear()
		{
			lineRenderer.positionCount = 0;
		}
	}
}
