namespace Monoblok
{
	public class CustomSizeKnockback : RadialKnockback
	{
		public float radius = 1f;

		protected override float GetRadius()
		{
			return radius;
		}
	}
}
