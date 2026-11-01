using System.Collections.Generic;
using UnityEngine;
using ZLinq;

namespace Monoblok.Environment
{
	public class TracksManager : MonoBehaviour
	{
		[SerializeField]
		private TrainTracks[] tracks;

		public IReadOnlyList<TrainTracks> Tracks => tracks;

		private void Awake()
		{
			tracks = (from t in tracks.AsValueEnumerable()
				orderby t.transform.position.y
				select t).ToArray();
		}

		public TrainTracks GetNextUp(int index, out int newIndex)
		{
			newIndex = 0;
			if (index >= tracks.Length - 1)
			{
				newIndex = tracks.Length - 1;
			}
			else
			{
				newIndex = index + 1;
			}
			return tracks[newIndex];
		}

		public TrainTracks GetNextDown(int index, out int newIndex)
		{
			newIndex = 0;
			if (index <= 0)
			{
				newIndex = 0;
			}
			else
			{
				newIndex = index - 1;
			}
			return tracks[newIndex];
		}

		public TrainTracks GetTracksAtIndex(int index)
		{
			index = Mathf.Clamp(index, 0, tracks.Length - 1);
			return tracks[index];
		}
	}
}
