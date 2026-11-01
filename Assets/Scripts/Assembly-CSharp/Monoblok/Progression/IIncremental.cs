namespace Monoblok.Progression
{
	public interface IIncremental<T>
	{
		T Get(int waveIndex);
	}
}
