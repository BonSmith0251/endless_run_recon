using Monoblok.Core;
using Monoblok.Train;

namespace Monoblok
{
	public abstract class CartSelectionStrategy : ICartSelectionStrategy
	{
		protected readonly Observable<TrainCart> targetedCart = new Observable<TrainCart>(null);

		protected readonly Observable<TrainCart> selectedCart = new Observable<TrainCart>(null);

		public Observable<TrainCart> TargetedCart => targetedCart;

		public Observable<TrainCart> SelectedCart => selectedCart;

		public void Initialize(TrainCart initialSelection = null)
		{
			selectedCart.SetValue(initialSelection);
			OnInitialize();
		}

		public void Update(float deltaTime)
		{
			OnUpdate(deltaTime);
		}

		public void Dispose()
		{
			OnDispose();
		}

		protected virtual void OnInitialize()
		{
		}

		protected virtual void OnDispose()
		{
		}

		protected virtual void OnUpdate(float deltaTime)
		{
		}
	}
}
