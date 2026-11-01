using Monoblok.Upgrades;

namespace Monoblok.Train
{
	public class ModuleData
	{
		public readonly ModuleDefinition Definition;

		public readonly RuntimePrice Worth;

		public ModuleData(ModuleDefinition definition, Price worth)
		{
			Definition = definition;
			Worth = new RuntimePrice(worth);
		}
	}
}
