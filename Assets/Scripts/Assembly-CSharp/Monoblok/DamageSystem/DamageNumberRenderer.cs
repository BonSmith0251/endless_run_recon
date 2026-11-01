using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Pool;

namespace Monoblok.DamageSystem
{
	public class DamageNumberRenderer : MonoBehaviour
	{
		[SerializeField]
		private DamageNumber textPrefab;

		[SerializeField]
		private RectTransform textContainer;

		[SerializeField]
		private LocalizedString localizedDodge;

		[Header("Config")]
		[SerializeField]
		private Color defaultColor = Color.white;

		[SerializeField]
		private Color critColor = Unity6Compat.OrangeRed;

		[SerializeField]
		private Color dodgeColor = Unity6Compat.AliceBlue;

		[SerializeField]
		private Color shieldColor = Color.cyan;

		[Space]
		[SerializeField]
		private float yPositionGain = 25f;

		private ObjectPool<DamageNumber> textPool;

		private List<DamageNumber> activeNumbers = new List<DamageNumber>();

		private Camera mainCam;

		private void Awake()
		{
			textPool = new ObjectPool<DamageNumber>(Create, OnGet, OnRelease, OnDestroyText);
			mainCam = Camera.main;
		}

		private void OnDisable()
		{
			DamageNumber[] array = activeNumbers.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Release();
			}
			activeNumbers.Clear();
		}

		private DamageNumber Create()
		{
			DamageNumber damageNumber = Object.Instantiate(textPrefab, textContainer);
			damageNumber.Initialize(textPool);
			damageNumber.Released += HandleReleased;
			return damageNumber;
		}

		private void HandleReleased(DamageNumber instance)
		{
			activeNumbers.Remove(instance);
		}

		private void OnGet(DamageNumber text)
		{
			text.gameObject.SetActive(value: true);
		}

		private void OnRelease(DamageNumber text)
		{
			text.gameObject.SetActive(value: false);
		}

		private void OnDestroyText(DamageNumber text)
		{
			if (text != null) Object.Destroy(text.gameObject);
		}

		public void Register(DamageNumberProvider provider)
		{
			provider.OnDamaged += DisplayDamageNumber;
			provider.OnShieldHit += DisplayShieldDamageNumber;
			provider.OnDodged += DisplayDodge;
		}

		public void UnRegister(DamageNumberProvider provider)
		{
			provider.OnDamaged -= DisplayDamageNumber;
			provider.OnShieldHit -= DisplayShieldDamageNumber;
			provider.OnDodged -= DisplayDodge;
		}

		private void DisplayDodge(Damage damage, Transform tr)
		{
			string msg = ((localizedDodge == null || localizedDodge.IsEmpty) ? "DODGED!" : localizedDodge.GetLocalizedString());
			DisplayText(tr.position, msg, dodgeColor);
		}

		private void DisplayDamageNumber(Damage damage, Transform tr)
		{
			if (damage.DamageValue != 0)
			{
				DisplayText(tr.position, damage.ToString(), damage.IsCrit ? critColor : defaultColor);
			}
		}

		private void DisplayShieldDamageNumber(Damage damage, Transform tr)
		{
			if (damage.DamageValue != 0)
			{
				DisplayText(tr.position, damage.ToString(), shieldColor);
			}
		}

		private void DisplayText(Vector3 worldPos, string msg, Color color)
		{
			if (base.gameObject.activeInHierarchy)
			{
				Vector3 position = mainCam.WorldToScreenPoint(worldPos + (Vector3)Random.insideUnitCircle);
				DamageNumber damageNumber = textPool.Get();
				activeNumbers.Add(damageNumber);
				damageNumber.SetColor(color);
				damageNumber.SetText(msg);
				damageNumber.transform.localScale = Vector3.one;
				damageNumber.transform.position = position;
				damageNumber.Animate(yPositionGain);
			}
		}
	}
}
