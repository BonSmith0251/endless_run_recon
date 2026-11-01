namespace Monoblok.EffectSystem
{
	public readonly struct DamageOverTimePayload
	{
		public readonly float DamagePerTick;

		public readonly float TickInterval;

		public DamageOverTimePayload(float damagePerTick, float tickInterval)
		{
			DamagePerTick = damagePerTick;
			TickInterval = tickInterval;
		}
	}
}
