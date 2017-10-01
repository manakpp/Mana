using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Saves and loads save data.
	/// Expected convention is every class handles saving and load themselves.
	/// </summary>
	public class SaveDataLoader : ACoreList<ISaveDataDelegate>
	{
		public void Load()
		{

		}

		public void Save()
		{

		}
	}
}