namespace Monoblok
{
	public interface ITradeRewardEligibility
	{
		bool IsEligible(TradeContext ctx);
	}
}
