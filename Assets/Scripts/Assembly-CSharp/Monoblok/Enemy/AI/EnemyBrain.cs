using System.Collections.Generic;
using Animancer;
using Monoblok.EffectSystem;
using Monoblok.Movement;
using Monoblok.StatSystem;
using Monoblok.StateMachine;
using Monoblok.Weapon;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[RequireComponent(typeof(Enemy))]
	public class EnemyBrain : MonoBehaviour
	{
		[SerializeField]
		private Enemy enemy;

		[SerializeField]
		private GameObject signalPrefab;

		[SerializeField]
		private MovementBase movement;

		[SerializeField]
		private WeaponHandler weaponHandler;

		[SerializeField]
		private AnimancerComponent animancer;

		[SerializeField]
		private StatManager statManager;

		[SerializeField]
		private BoxCollider2D boxCollider;

		[SerializeField]
		private EffectHandler effectHandler;

		private Monoblok.StateMachine.StateMachine stateMachine;

		private EnemyBehaviorConfig config;

		private EnemyState[][] phaseStates;

		private float[] phaseThresholds;

		private int currentPhaseIndex;

		private int currentStateIndex;

		private int? pendingPhaseIndex;

		public Enemy Enemy => enemy;

		public GameObject SignalPrefab => signalPrefab;

		public MovementBase Movement => movement;

		public AnimancerComponent Animancer => animancer;

		public StatManager StatManager => statManager;

		public BoxCollider2D BoxCollider => boxCollider;

		public WeaponHandler WeaponHandler => weaponHandler;

		public EffectHandler EffectHandler => effectHandler;

		private void OnValidate()
		{
			if (!(enemy != null))
			{
				enemy = GetComponent<Enemy>();
			}
		}

		private void Start()
		{
			if (weaponHandler != null)
			{
				weaponHandler.DisableWeapon();
			}
		}

		public void Stop()
		{
			stateMachine?.Stop();
			if (weaponHandler != null)
			{
				weaponHandler.DisableWeapon();
			}
		}

		public void Restart()
		{
			currentPhaseIndex = 0;
			currentStateIndex = 0;
			pendingPhaseIndex = null;
			stateMachine?.Restart();
		}

		public void Initialize(EnemyBehaviorConfig config)
		{
			if (config == null)
			{
				Debug.LogError("No config provided", base.gameObject);
				return;
			}
			if (config.Phases == null || config.Phases.Length == 0)
			{
				Debug.LogError("[EnemyBrain] EnemyBehaviorConfig has no phases.", base.gameObject);
				return;
			}
			if (config.Phases[0].States == null || config.Phases[0].States.Length == 0)
			{
				Debug.LogError("[EnemyBrain] EnemyBehaviorConfig's first phase has no states.", base.gameObject);
				return;
			}
			if (this.config == config)
			{
				Restart();
				return;
			}
			stateMachine = new Monoblok.StateMachine.StateMachine();
			int num = config.Phases.Length;
			phaseStates = new EnemyState[num][];
			phaseThresholds = new float[num];
			for (int i = 0; i < num; i++)
			{
				List<EnemyState> list = new List<EnemyState>();
				EnemyStateConfig[] states = config.Phases[i].States;
				for (int j = 0; j < states.Length; j++)
				{
					EnemyState state = states[j].GetState(this);
					state.OnFinished += HandleStateFinished;
					list.Add(state);
				}
				phaseStates[i] = list.ToArray();
				phaseThresholds[i] = (float)(num - i) / (float)num;
			}
			currentPhaseIndex = 0;
			currentStateIndex = 0;
			pendingPhaseIndex = null;
			enemy.Health.OnHealthChanged -= HandlePhaseHealthChanged;
			enemy.Health.OnHealthChanged += HandlePhaseHealthChanged;
			stateMachine.SetInitialState(phaseStates[0][0]);
			this.config = config;
		}

		private void HandleStateFinished()
		{
			if (pendingPhaseIndex.HasValue)
			{
				currentPhaseIndex = pendingPhaseIndex.Value;
				pendingPhaseIndex = null;
				currentStateIndex = 0;
			}
			else
			{
				currentStateIndex = (currentStateIndex + 1) % phaseStates[currentPhaseIndex].Length;
			}
			stateMachine.ForceState(phaseStates[currentPhaseIndex][currentStateIndex]);
		}

		private void HandlePhaseHealthChanged(int _)
		{
			if (phaseStates != null)
			{
				float normalizedHealth = enemy.Health.NormalizedHealth;
				int i;
				for (i = pendingPhaseIndex ?? currentPhaseIndex; i + 1 < phaseStates.Length && normalizedHealth <= phaseThresholds[i + 1]; i++)
				{
				}
				if (i != currentPhaseIndex && phaseStates[i].Length != 0)
				{
					pendingPhaseIndex = i;
				}
			}
		}

		private void Update()
		{
			if (stateMachine != null)
			{
				stateMachine.Update(Time.deltaTime);
			}
		}
	}
}
