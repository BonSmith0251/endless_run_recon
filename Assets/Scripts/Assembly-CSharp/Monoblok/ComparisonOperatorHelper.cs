using System;

namespace Monoblok
{
	public static class ComparisonOperatorHelper
	{
		public static bool Evaluate(this ComparisonOperator comparisonOperator, int current, int target)
		{
			return comparisonOperator switch
			{
				ComparisonOperator.LessThan => current < target, 
				ComparisonOperator.LessThanOrEqual => current <= target, 
				ComparisonOperator.Equal => current == target, 
				ComparisonOperator.MoreThan => current > target, 
				ComparisonOperator.MoreThanOrEqual => current >= target, 
				_ => throw new NotImplementedException(), 
			};
		}
	}
}
