namespace Monoblok.Train
{
	public class CartConnectionSlot
	{
		public enum SlotType
		{
			Parent,
			Child
		}

		private TrainCart connection;

		public bool IsConnected => connection != null;

		public TrainCart Connection => connection;

		public void Connect(TrainCart other)
		{
			connection = other;
		}

		public void Disconnect()
		{
			connection = null;
		}
	}
}
