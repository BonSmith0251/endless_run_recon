using System;

namespace Monoblok.Upgrades
{
	public class EligibilityState : IEquatable<EligibilityState>
	{
		public static readonly EligibilityState Eligible = new EligibilityState(isEligible: true, IneligibilityReason.None, string.Empty);

		public bool IsEligible { get; }

		public IneligibilityReason Reason { get; }

		public string Message { get; }

		public EligibilityState(bool isEligible, IneligibilityReason reason, string message)
		{
			IsEligible = isEligible;
			Reason = reason;
			Message = message;
		}

		public bool Equals(EligibilityState other)
		{
			if (other == null)
			{
				return false;
			}
			if (IsEligible == other.IsEligible)
			{
				return Reason == other.Reason;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as EligibilityState);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(IsEligible, Reason);
		}
	}
}
