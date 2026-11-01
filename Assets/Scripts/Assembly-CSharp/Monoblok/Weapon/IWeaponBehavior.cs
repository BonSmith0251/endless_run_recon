using Monoblok.Movement;

namespace Monoblok.Weapon
{
	public interface IWeaponBehavior
	{
		void OnTargetLost();

		void Update(float deltaTime, ITarget target);

		void OnDisable();

		void OnEnable();
	}
}
