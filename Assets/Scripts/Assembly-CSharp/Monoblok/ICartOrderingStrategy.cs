namespace Monoblok
{
	public interface ICartOrderingStrategy
	{
		void Initialize();

		void Dispose();

		void Update(float deltaTime);
	}
}
