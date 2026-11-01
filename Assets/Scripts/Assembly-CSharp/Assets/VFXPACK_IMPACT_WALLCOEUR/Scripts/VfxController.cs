using System.Collections.Generic;
using UnityEngine;

namespace Assets.VFXPACK_IMPACT_WALLCOEUR.Scripts
{
	public class VfxController : MonoBehaviour
	{
		[Header("Place VFX prefabs here")]
		[SerializeField]
		private List<GameObject> _vfxList;

		private GameObject _currentVfx;

		public List<GameObject> VfxList => _vfxList;

		public void Play(int index)
		{
			if (_currentVfx != null)
			{
				Object.Destroy(_currentVfx);
			}
			_currentVfx = Object.Instantiate(_vfxList[index], Vector3.zero, Quaternion.identity, base.transform);
		}

		public void Stop()
		{
			if (_currentVfx != null)
			{
				Object.Destroy(_currentVfx);
			}
		}
	}
}
