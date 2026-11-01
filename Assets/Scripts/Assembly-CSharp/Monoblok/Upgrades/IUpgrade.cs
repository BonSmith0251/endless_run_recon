namespace Monoblok.Upgrades
{
	public interface IUpgrade
	{
		void Apply();

		void Remove();
	}
}
