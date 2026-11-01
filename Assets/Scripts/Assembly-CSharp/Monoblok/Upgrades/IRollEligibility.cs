namespace Monoblok.Upgrades
{
	public interface IRollEligibility
	{
		bool IsElgible(ShopActionContext ctx);
	}
}
