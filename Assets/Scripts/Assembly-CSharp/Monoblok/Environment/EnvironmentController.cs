using System.Collections.Generic;
using System.Linq;
using Monoblok.Progression;
using UnityEngine;

namespace Monoblok.Environment
{
	public class EnvironmentController : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		private List<EnvironmentMember> members;

		private void Awake()
		{
			members = GetComponentsInChildren<EnvironmentMember>().ToList();
			if (!(progression == null))
			{
				InitializeMembers();
			}
		}

		public void InitializeMembers()
		{
			if (members == null)
			{
				return;
			}
			foreach (EnvironmentMember member in members)
			{
				if (!(member == null))
				{
					member.Initialize(progression.Speed, progression.CurrentWaveIndex);
				}
			}
		}

		private void Update()
		{
			if (progression == null || members == null)
			{
				return;
			}
			float globalDistance = progression.GlobalDistance;
			float normalizedSpeed = progression.NormalizedSpeed;
			foreach (EnvironmentMember member in members)
			{
				if (!(member == null))
				{
					member.UpdateEnvironmentMember(globalDistance, normalizedSpeed);
				}
			}
		}

		public void Register(EnvironmentMember member)
		{
			members.Add(member);
		}

		public void ResetMembers()
		{
			if (members == null)
			{
				return;
			}
			foreach (EnvironmentMember member in members)
			{
				member.ResetMember();
			}
		}
	}
}
