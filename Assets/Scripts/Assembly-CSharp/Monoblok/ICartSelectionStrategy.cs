using Monoblok.Core;
using Monoblok.Train;

namespace Monoblok
{
	public interface ICartSelectionStrategy
	{
		Observable<TrainCart> TargetedCart { get; }

		Observable<TrainCart> SelectedCart { get; }

		void Initialize(TrainCart initialSelection = null);

		void Update(float deltaTime);

		void Dispose();
	}
}
