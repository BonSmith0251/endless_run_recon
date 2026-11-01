namespace Monoblok.Movement
{
	public interface IMove
	{
		void SetDestination(ITarget target);

		void Stop();
	}
}
