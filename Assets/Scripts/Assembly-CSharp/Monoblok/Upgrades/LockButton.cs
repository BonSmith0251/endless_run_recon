using System;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	public class LockButton : MonoBehaviour
	{
		[SerializeField]
		private Button button;

		[SerializeField]
		private Image lockImage;

		[SerializeField]
		private Sprite locked;

		[SerializeField]
		private Sprite unlocked;

		public event Action OnClicked;

		private void OnEnable()
		{
			button.onClick.AddListener(delegate
			{
				this.OnClicked?.Invoke();
			});
		}

		private void OnDisable()
		{
			button.onClick.RemoveAllListeners();
		}

		public void SetState(bool locked)
		{
			lockImage.sprite = (locked ? this.locked : unlocked);
		}
	}
}
