using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Loads data assets 
	/// TODO: Optionally load from payloads / remote data / external data
	/// </summary>
	public class DataLoader : ACoreList<IDataLoadDelegate>
	{
		public IEnumerator LoadData()
		{
			yield break;
		}
	}
}