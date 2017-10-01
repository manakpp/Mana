using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	public class SceneInfoList : ACoreList<SceneInfo>
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
				SceneInfo initObject = m_list[i];
				if (initObject == null)
				{
					Logger.Error("Object in slot [{0}] was empty.", i);
					continue;
				}

				if (!SceneListCheck.Has(initObject.Name))
				{
					Logger.Error("Scene of type [{0}] has scene name [{1}] but that doesn't exist.", initObject.Type, initObject.Name);
					continue;
				}
			}
#endif
		}

		public string GetSceneNameFromType(SceneType type, bool showWarning = false)
		{
			for (int i = 0; i < m_list.Count; ++i)
			{
				if (m_list[i].Type == type)
				{
					return m_list[i].Name;
				}
			}

			if (showWarning)
			{
				Logger.Warning("Could not find Scene name with SceneType: {0}.", type);
			}

			return null;
		}
	}
}