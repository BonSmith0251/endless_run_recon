using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity.UI.Shaders.Sample
{
	[RequireComponent(typeof(Graphic))]
	public class BlurredHexagonController : UIBehaviour, IMaterialModifier
	{
		private const string _saturationRef = "_Background_Saturation";

		private const string _tintRef = "_Tint";

		private const string _hexagonDissolveRef = "_Hexagon_Dissolve";

		private const string _hexagonTilesRef = "_Hexagon_Tiles";

		private const string _blurRef = "_Blur";

		private const string _blurCyclesRef = "_Blur_Cycles";

		private const string _blurSamplesPerCycleRef = "_Blur_Samples_Per_Cycle";

		[SerializeField]
		private Slider _saturationSlider;

		[SerializeField]
		private Slider _tintRedSlider;

		[SerializeField]
		private Slider _tintGreenSlider;

		[SerializeField]
		private Slider _tintBlueSlider;

		[SerializeField]
		private Slider _hexagonDissolveSlider;

		[SerializeField]
		private Slider _hexagonTilesSlider;

		[SerializeField]
		private Slider _blurSlider;

		[SerializeField]
		private Slider _blurCyclesSlider;

		[SerializeField]
		private Slider _blurSamplesPerCycleSlider;

		private static int _saturationId;

		private static int _tintId;

		private static int _hexagonDissolveId;

		private static int _hexagonTilesId;

		private static int _blurId;

		private static int _blurCyclesId;

		private static int _blurSamplesPerCycleId;

		private float _saturation;

		private float _hexagonDissolve;

		private float _hexagonTiles;

		private float _blur;

		private float _blurCycles;

		private float _blurSamplesPerCycle;

		private Color _tint = Color.gray;

		private Graphic _graphic;

		public float Saturation
		{
			get
			{
				return _saturation;
			}
			set
			{
				_saturation = value;
				Graphic.SetMaterialDirty();
			}
		}

		public Color Tint
		{
			get
			{
				return _tint;
			}
			set
			{
				_tint = value;
				Graphic.SetMaterialDirty();
			}
		}

		public float TintRed
		{
			get
			{
				return Tint.r;
			}
			set
			{
				Tint = new Color(value, _tint.g, _tint.b);
			}
		}

		public float TintGreen
		{
			get
			{
				return Tint.g;
			}
			set
			{
				Tint = new Color(_tint.r, value, _tint.b);
			}
		}

		public float TintBlue
		{
			get
			{
				return Tint.b;
			}
			set
			{
				Tint = new Color(_tint.r, _tint.g, value);
			}
		}

		public float HexagonDissolve
		{
			get
			{
				return _hexagonDissolve;
			}
			set
			{
				_hexagonDissolve = value;
				Graphic.SetMaterialDirty();
			}
		}

		public float HexagonTiles
		{
			get
			{
				return _hexagonTiles;
			}
			set
			{
				_hexagonTiles = value;
				Graphic.SetMaterialDirty();
			}
		}

		public float Blur
		{
			get
			{
				return _blur;
			}
			set
			{
				_blur = value;
				Graphic.SetMaterialDirty();
			}
		}

		public float BlurCycles
		{
			get
			{
				return _blurCycles;
			}
			set
			{
				_blurCycles = value;
				Graphic.SetMaterialDirty();
			}
		}

		public float BlurSamplesPerCycle
		{
			get
			{
				return _blurSamplesPerCycle;
			}
			set
			{
				_blurSamplesPerCycle = value;
				Graphic.SetMaterialDirty();
			}
		}

		public Graphic Graphic
		{
			get
			{
				if (_graphic == null)
				{
					_graphic = GetComponent<Graphic>();
				}
				return _graphic;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_saturationId = Shader.PropertyToID("_Background_Saturation");
			_tintId = Shader.PropertyToID("_Tint");
			_hexagonDissolveId = Shader.PropertyToID("_Hexagon_Dissolve");
			_hexagonTilesId = Shader.PropertyToID("_Hexagon_Tiles");
			_blurId = Shader.PropertyToID("_Blur");
			_blurCyclesId = Shader.PropertyToID("_Blur_Cycles");
			_blurSamplesPerCycleId = Shader.PropertyToID("_Blur_Samples_Per_Cycle");
			Material material = Graphic.material;
			_saturation = material.GetFloat(_saturationId);
			_tint = material.GetColor(_tintId);
			_hexagonDissolve = material.GetFloat(_hexagonDissolveId);
			_hexagonTiles = material.GetFloat(_hexagonTilesId);
			_blur = material.GetFloat(_blurId);
			_blurCycles = material.GetFloat(_blurCyclesId);
			_blurSamplesPerCycle = material.GetFloat(_blurSamplesPerCycleId);
			_saturationSlider?.SetValueWithoutNotify(Saturation);
			_tintRedSlider?.SetValueWithoutNotify(TintRed);
			_tintGreenSlider?.SetValueWithoutNotify(TintGreen);
			_tintBlueSlider?.SetValueWithoutNotify(TintBlue);
			_hexagonDissolveSlider?.SetValueWithoutNotify(HexagonDissolve);
			_hexagonTilesSlider?.SetValueWithoutNotify(HexagonTiles);
			_blurSlider?.SetValueWithoutNotify(Blur);
			_blurCyclesSlider?.SetValueWithoutNotify(BlurCycles);
			_blurSamplesPerCycleSlider?.SetValueWithoutNotify(BlurSamplesPerCycle);
			_saturationSlider?.onValueChanged.AddListener(delegate(float x)
			{
				Saturation = x;
			});
			_tintRedSlider?.onValueChanged?.AddListener(delegate(float x)
			{
				TintRed = x;
			});
			_tintGreenSlider?.onValueChanged?.AddListener(delegate(float x)
			{
				TintGreen = x;
			});
			_tintBlueSlider?.onValueChanged?.AddListener(delegate(float x)
			{
				TintBlue = x;
			});
			_hexagonDissolveSlider?.onValueChanged.AddListener(delegate(float x)
			{
				HexagonDissolve = x;
			});
			_hexagonTilesSlider?.onValueChanged.AddListener(delegate(float x)
			{
				HexagonTiles = x;
			});
			_blurSlider?.onValueChanged.AddListener(delegate(float x)
			{
				Blur = x;
			});
			_blurCyclesSlider?.onValueChanged.AddListener(delegate(float x)
			{
				BlurCycles = x;
			});
			_blurSamplesPerCycleSlider?.onValueChanged.AddListener(delegate(float x)
			{
				BlurSamplesPerCycle = x;
			});
		}

		public Material GetModifiedMaterial(Material baseMaterial)
		{
			if (!Application.isPlaying || !base.enabled)
			{
				return baseMaterial;
			}
			baseMaterial.SetFloat(_saturationId, Saturation);
			baseMaterial.SetColor(_tintId, Tint);
			baseMaterial.SetFloat(_hexagonDissolveId, HexagonDissolve);
			baseMaterial.SetFloat(_hexagonTilesId, HexagonTiles);
			baseMaterial.SetFloat(_blurId, Blur);
			baseMaterial.SetFloat(_blurCyclesId, BlurCycles);
			baseMaterial.SetFloat(_blurSamplesPerCycleId, BlurSamplesPerCycle);
			return baseMaterial;
		}
	}
}
