using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;
using UnityEditorInternal;

namespace Mana
{
	/// <summary>
	/// An interface for loading in different scenes.
	/// TODO: Decide on a convention for scene loading and scene management
	/// </summary>
	[RequireComponent(typeof(SceneInfoList))]
	public class SceneManager : MonoBehaviour
	{
		public enum LoadMode
		{
			Single,
			Additive,
		}

		private SceneInfoList m_sceneInfoCollection;

		public void Awake()
		{
			m_sceneInfoCollection = GetComponent<SceneInfoList>();
		}

		public AsyncOperation LoadSceneAsync(SceneType sceneType, LoadMode loadMode)
		{
			string sceneName = m_sceneInfoCollection.GetSceneNameFromType(sceneType, false);
			if (string.IsNullOrEmpty(sceneName))
			{
				Logger.Error("Could not find Scene associated with type: {0}. Hook it up on SceneInfoCollection.", sceneType);
				return null; 
			}

			AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, (UnityEngine.SceneManagement.LoadSceneMode)loadMode);
			return operation;
		}
	}
}