namespace Monoblok.StateMachine
{
	public interface IState
	{
		void Enter();

		void Update(float deltaTime);

		void Exit();
	}
}
