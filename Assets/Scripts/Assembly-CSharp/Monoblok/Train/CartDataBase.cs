using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Cart DataBase")]
	public class CartDataBase : ScriptableObject
	{
		[SerializeField]
		private List<CartData> cartDatas = new List<CartData>();

		public IReadOnlyList<CartData> CartDatas => cartDatas;
	}
}
