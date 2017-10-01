using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Simply wraps a list. 
	/// This is done in an effort to reduce boilerplate editor code.
	/// </summary>
	public class GameObjectList : ACoreList<GameObject>
	{
		private void Awake()
		{
			Validate();
		}

		private void Validate()
		{
#if UNITY_EDITOR
			for (int i = 0; i < m_list.Count; ++i)
			{
				GameObject initObject = m_list[i];
				if (initObject == null)
				{
					Logger.Error("GameObject in slot [{0}] was empty.", i);
					continue;
				}
			}
#endif
		}
	}
}