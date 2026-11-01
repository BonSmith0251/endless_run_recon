using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.VFXPACK_IMPACT_WALLCOEUR.Scripts
{
	public class DropdownController : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private VfxController _vfxController;

		[SerializeField]
		private TMP_Dropdown _dropdown;

		private void Start()
		{
			GenerateDropdown();
		}

		private void GenerateDropdown()
		{
			List<string> list = new List<string>();
			_dropdown.ClearOptions();
			foreach (GameObject vfx in _vfxController.VfxList)
			{
				list.Add(vfx.name);
			}
			_dropdown.AddOptions(list);
			_dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
		}

		private void OnDropdownValueChanged(int index)
		{
			PlayVfx();
		}

		public void PlayVfx()
		{
			_vfxController.Play(_dropdown.value);
		}

		public void StopVfx()
		{
			_vfxController.Stop();
		}

		public void PreviousVfx()
		{
			if (_dropdown.value != 0)
			{
				_dropdown.value--;
				_dropdown.RefreshShownValue();
			}
		}

		public void NextVfx()
		{
			if (_dropdown.value != _dropdown.options.Count)
			{
				_dropdown.value++;
				_dropdown.RefreshShownValue();
			}
		}
	}
}
