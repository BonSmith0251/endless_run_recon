using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Monoblok.DamageSystem;
using Monoblok.Enemy;
using Monoblok.Environment;
using Monoblok.Inventory;
using Monoblok.Progression;
using Monoblok.SA;
using Monoblok.StatSystem;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;
using ZLinq;

namespace Monoblok.Train
{
	[RequireComponent(typeof(Health))]
	public class Train : MonoBehaviour, IStatManagerInjectable
	{
		[SerializeField]
		private TrainCart cartPrefab;

		[SerializeField]
		private Dredger dredger;

		[SerializeField]
		private TrainController controller;

		[SerializeField]
		private Health health;

		[SerializeField]
		private TrainExplosion explosion;

		[SerializeField]
		private TrainSounds trainSounds;

		[SerializeField]
		private TrainExperience experience;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private EnemyGameEvent enemyDiedEvent;

		[SerializeField]
		private AbilityManager abilityManager;

		[SerializeField]
		private ParticleSystem addCartPs;

		[SerializeField]
		private float cartGap;

		[SerializeField]
		private StatOrValue maxCarts;

		[SerializeField]
		private GlobalStatManager globalStatManager;

		[Header("Game Events")]
		[SerializeField]
		private GameEvent cartAddedEvent;

		private TrainUnitSequence unitSequence;

		private TrainCart locomotive;

		private TrainConfig config;

		[Header("---TEST---")]
		[SerializeField]
		private CartData cartData;

		private List<Func<UniTask>> asyncActions = new List<Func<UniTask>>();

		[Space]
		[Header("Save System")]
		[SerializeField]
		private CartDataBase cartDataBase;

		[SerializeField]
		private ModuleDatabase moduleDataBase;

		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private CartMergeManager mergeManager;

		public TrainConfig Config => config;

		public TrainCart Locomotive => locomotive;

		public Health Health => health;

		public IReadOnlyList<TrainUnit> Units => unitSequence?.Units;

		public IEnumerable<TrainCart> Carts => unitSequence?.Carts;

		public bool IsFull
		{
			get
			{
				if ((float)maxCarts != -1f)
				{
					return (float)CartCount >= (float)maxCarts;
				}
				return false;
			}
		}

		public int CartCount => unitSequence.CartCount;

		public int MaxCartCount => (int)maxCarts.Value;

		public int TrainLevel => experience.CurrentLevel;

		public Bounds Bounds
		{
			get
			{
				Bounds localBounds = unitSequence.LocalBounds;
				localBounds.center += base.transform.position;
				return localBounds;
			}
		}

		public event Action<TrainCart> OnCartAdded;

		public event Action<TrainCart> OnCartRemoved;

		public event Action OnCartsChanged;

		public event Action<int> OnMaxCartsChanged;

		[Button(null, EButtonEnableMode.Always)]
		private void Test()
		{
			TryAddCart(cartData, out var _);
		}

		private void OnValidate()
		{
			if (health == null)
			{
				health = GetComponent<Health>();
			}
		}

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				progression.OnWaveStarted += OnWaveStarted;
				progression.OnWaveEnded += OnWaveEnded;
				enemyDiedEvent.OnTriggered += OnEnemyDied;
				health.OnHealthZero += HandleHealthZero;
			}
		}

		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				progression.OnWaveStarted -= OnWaveStarted;
				progression.OnWaveEnded -= OnWaveEnded;
				enemyDiedEvent.OnTriggered -= OnEnemyDied;
				health.OnHealthZero -= HandleHealthZero;
			}
		}

		private void OnWaveStarted()
		{
			controller.EnableControls(enabled: true);
		}

		private void OnWaveEnded()
		{
			controller.EnableControls(enabled: false);
			controller.ResetPosition();
		}

		private void OnEnemyDied(Monoblok.Enemy.Enemy enemy)
		{
			experience.GainXP(enemy.XP);
		}

		private void ClearTrain()
		{
			if (Carts != null)
			{
				List<TrainCart> list = Carts.AsValueEnumerable().ToList();
				for (int num = list.Count - 1; num >= 0; num--)
				{
					TrainCart trainCart = list[num];
					trainCart.ModuleManager.DetachAll(out var _);
					RemoveCart(trainCart);
				}
			}
		}

		private void HandleHealthZero()
		{
			explosion.Explode();
		}

		public void Initialize(TrainConfig config)
		{
			ClearTrain();
			ApplyConfig(config);
			base.transform.position = Vector3.zero;
			dredger.ResetConstraintAndLocalPosition();
			trainSounds.StopAll();
			experience.Initialize();
			unitSequence = new TrainUnitSequence(this);
			TryAddCart(config.LocoData, out locomotive, -1, animate: false);
			dredger.ResetConstraintAndLocalPosition();
			dredger.SetParentConstraintSource(locomotive.SpriteRenderer.transform);
			CartData[] initialCarts = config.InitialCarts;
			foreach (CartData data in initialCarts)
			{
				TryAddCart(data, out var _, -1, animate: false);
			}
			locomotive.ModuleManager.TryAddModule(config.InitialModule.GetRuntimeData(), silent: true);
			health.ForceHealth(health.MaxHealth);
			Refresh(instant: false);
		}

		private void ApplyConfig(TrainConfig config)
		{
			this.config = config;
			RegisterConfigStats(config);
			abilityManager.UnregisterAll();
			if (config.AbiltiyConfig != null)
			{
				abilityManager.RegisterAbility(config.AbiltiyConfig.GetAbility());
			}
		}

		private void RegisterConfigStats(TrainConfig config)
		{
			if (config.Stats == null)
			{
				return;
			}
			StatManager statManager = globalStatManager.StatManager;
			if (statManager == null)
			{
				Debug.LogError("No global stat manager assigned, cannot register train config stats", this);
				return;
			}
			StatInitializer[] stats = config.Stats;
			foreach (StatInitializer statInitializer in stats)
			{
				statManager.RegisterOrUpdateStat(statInitializer);
			}
			globalStatManager.RefreshView();
		}

		public bool TryAddCart(CartData data, out TrainCart cart, int index = -1, bool animate = true)
		{
			cart = null;
			if (IsFull)
			{
				return false;
			}
			cart = UnityEngine.Object.Instantiate(cartPrefab, base.transform);
			cart.Initialize(this, data);
			TrainUnit unit = new TrainUnit(this, cartGap, cart);
			unitSequence.Add(unit, index);
			this.OnCartAdded?.Invoke(cart);
			Refresh();
			if (!animate)
			{
				return true;
			}
			Vector3 vector = new Vector3(0f, 2f, 0f);
			Vector3 psPos = cart.Center.position - new Vector3(0f, 0.5f, 0f);
			cart.transform.localScale = Vector3.zero;
			cart.transform.position += vector;
			Sequence.Create().Chain(Tween.Scale(cart.transform, Vector3.one, 0.15f, Ease.OutBack)).ChainDelay(0.2f)
				.Chain(Tween.Position(cart.transform, cart.transform.position - vector, 0.1f, Ease.InCirc))
				.ChainCallback(this, delegate(Train t)
				{
					t.addCartPs.transform.position = psPos;
					t.addCartPs.Play();
				})
				.ChainDelay(0.6f)
				.ChainCallback(this, delegate(Train t)
				{
					t.addCartPs.Stop();
				});
			cartAddedEvent.TriggerGameEvent();
			return true;
		}

		public void RemoveCart(TrainCart cart)
		{
			cart.ModuleManager.DetachAll(out var _);
			unitSequence.Remove(cart);
			Refresh();
			this.OnCartRemoved?.Invoke(cart);
			UnityEngine.Object.Destroy(cart.gameObject);
		}

		public TrainUnit GroupUnits(TrainUnit a, TrainUnit b)
		{
			int num = unitSequence.IndexOf(a);
			int num2 = unitSequence.IndexOf(b);
			TrainUnit trainUnit = TrainUnit.Merge(a, b);
			unitSequence.Remove(a);
			unitSequence.Remove(b);
			int index = ((num < num2) ? num : (num - 1));
			unitSequence.Add(trainUnit, index);
			Refresh(instant: false);
			return trainUnit;
		}

		public void UngroupUnit(TrainUnit unit)
		{
			int num = unitSequence.IndexOf(unit);
			if (num == -1)
			{
				Debug.LogError("Unit is not part of this sequence");
				return;
			}
			List<TrainCart> list = unit.GetCarts().AsValueEnumerable().ToList();
			unitSequence.Remove(unit);
			for (int i = 0; i < list.Count; i++)
			{
				TrainUnit unit2 = new TrainUnit(this, cartGap, list[i]);
				unitSequence.Add(unit2, num + i);
			}
			Refresh();
		}

		public void SwapUnits(TrainUnit a, TrainUnit b)
		{
			if (a.Orderable && b.Orderable)
			{
				unitSequence.Swap(a, b);
				Refresh(instant: false);
			}
		}

		public int IndexOf(TrainUnit unit)
		{
			return unitSequence.IndexOf(unit);
		}

		public int UnitIndexOf(TrainCart cart)
		{
			return unitSequence.UnitIndexOf(cart);
		}

		public TrainUnit GetUnitAtIndex(int index)
		{
			if (index < 0 || index >= Units.Count)
			{
				return null;
			}
			return Units[index];
		}

		public async UniTask OnArrivedToStation()
		{
			Func<UniTask>[] array = asyncActions.ToArray();
			Func<UniTask>[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				await array2[i]();
			}
		}

		public void RegisterWaitable(Func<UniTask> action)
		{
			asyncActions.Add(action);
		}

		public void RemoveWaitable(Func<UniTask> action)
		{
			asyncActions.Remove(action);
		}

		[Button(null, EButtonEnableMode.Always)]
		private void RefreshInstantly()
		{
			Refresh();
		}

		public void Refresh(bool instant = true)
		{
			unitSequence.Refresh(instant);
			this.OnCartsChanged?.Invoke();
		}

		public void LoadSaveData(TrainSaveData data, TrainConfig config)
		{
			if (data == null)
			{
				return;
			}
			ClearTrain();
			if (config != null)
			{
				ApplyConfig(config);
			}
			unitSequence = new TrainUnitSequence(this);
			locomotive = LoadCart(data.loco);
			dredger.ResetConstraintAndLocalPosition();
			dredger.SetParentConstraintSource(locomotive.SpriteRenderer.transform);
			unitSequence.Add(new TrainUnit(this, cartGap, locomotive));
			foreach (TrainUnitSaveData unit in data.units)
			{
				TrainCart[] array = new TrainCart[unit.carts.Count];
				for (int i = 0; i < unit.carts.Count; i++)
				{
					array[i] = LoadCart(unit.carts[i]);
				}
				TrainUnit trainUnit = new TrainUnit(this, cartGap, array);
				unitSequence.Add(trainUnit);
				if (unit.mergeCounter != -1 && array.Length >= 2)
				{
					CartMergeConnection connection = new CartMergeConnection(trainUnit, array[0], array[1], unit.mergeCounter);
					mergeManager.RegisterMergeConnection(connection);
				}
			}
			health.ForceHealth(data.hp);
			Refresh();
		}

		private TrainCart LoadCart(TrainCartSaveData data)
		{
			string cartId = data.id;
			List<ModuleSaveData> modules = data.modules;
			CartData cartData = cartDataBase.CartDatas.AsValueEnumerable().FirstOrDefault((CartData d) => d.ID == cartId);
			if (cartData == null)
			{
				Debug.LogError("No cart found in database with id " + cartId);
				return null;
			}
			TrainCart trainCart = UnityEngine.Object.Instantiate(cartPrefab, base.transform);
			trainCart.Initialize(this, cartData);
			this.OnCartAdded?.Invoke(trainCart);
			LoadModules(trainCart, modules);
			return trainCart;
		}

		private void LoadModules(TrainCart cart, List<ModuleSaveData> datas)
		{
			foreach (ModuleSaveData data in datas)
			{
				string moduleId = data.id;
				ModuleDefinition moduleDefinition = moduleDataBase.Modules.AsValueEnumerable().FirstOrDefault((ModuleDefinition d) => d.ID == moduleId);
				if (moduleDefinition == null)
				{
					Debug.LogError("No module definition found in databse with id " + moduleId);
					continue;
				}
				ModuleData runtimeData = moduleDefinition.GetRuntimeData();
				runtimeData.Worth.Amount.SetValue(data.worth.amount);
				cart.ModuleManager.TryAddModule(runtimeData, silent: true);
			}
		}

		public TrainSaveData GetSaveData()
		{
			TrainCartSaveData loco = new TrainCartSaveData
			{
				id = locomotive.CartData.ID,
				modules = GetModuleSaveDatas(locomotive)
			};
			List<TrainUnitSaveData> unitDatas = GetUnitDatas();
			return new TrainSaveData
			{
				loco = loco,
				units = unitDatas,
				hp = health.CurrentHealth,
				configId = ((config != null) ? config.ID : null)
			};
		}

		private List<TrainUnitSaveData> GetUnitDatas()
		{
			List<TrainUnitSaveData> list = new List<TrainUnitSaveData>();
			bool flag = false;
			foreach (TrainUnit unit in unitSequence.Units)
			{
				if (!flag && unit.GetCarts().AsValueEnumerable().Contains(locomotive))
				{
					flag = true;
					continue;
				}
				List<TrainCartSaveData> cartDatas = GetCartDatas(unit);
				TrainUnitSaveData item = new TrainUnitSaveData
				{
					carts = cartDatas,
					mergeCounter = ((unit.mergeConnection == null) ? (-1) : unit.mergeConnection.WavesRemaining.Value)
				};
				list.Add(item);
			}
			return list;
		}

		private List<TrainCartSaveData> GetCartDatas(TrainUnit unit)
		{
			List<TrainCartSaveData> list = new List<TrainCartSaveData>();
			foreach (TrainCart cart in unit.GetCarts())
			{
				List<ModuleSaveData> moduleSaveDatas = GetModuleSaveDatas(cart);
				TrainCartSaveData item = new TrainCartSaveData
				{
					id = cart.CartData.ID,
					modules = moduleSaveDatas
				};
				list.Add(item);
			}
			return list;
		}

		private List<ModuleSaveData> GetModuleSaveDatas(TrainCart cart)
		{
			List<ModuleSaveData> list = new List<ModuleSaveData>();
			foreach (ModuleData module in cart.ModuleManager.Modules)
			{
				ModuleSaveData item = new ModuleSaveData
				{
					id = module.Definition.ID,
					worth = new CurrencySaveData
					{
						id = module.Worth.Currency.ID,
						amount = module.Worth.Amount.Value
					}
				};
				list.Add(item);
			}
			return list;
		}

		public void Inject(StatManager statManager)
		{
			maxCarts.Bind(statManager, delegate(float v)
			{
				this.OnMaxCartsChanged?.Invoke((int)v);
			});
			health.BindStatManager(statManager);
		}
	}
}
