using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Accessor to Manager category classes
	/// Handles app initialisation, shutdown, updating, data loading and save data saving/loading.
	/// ENSURE THAT THIS EXECUTES BEFORE ANYTHING ELSE!!!!!!!!!
	/// </summary>
	public class App : MonoBehaviour
	{
		[SerializeField]
		private GlobalContext m_globalContextPrefab;

		private GlobalContext m_globalContext;

		private static App s_instance;

		private void Awake()
		{
			if (s_instance != null)
			{
				Logger.Warning("You tried to initialise a second instance of App. Getting rid of the second one now.");
				Destroy(gameObject);
				return;
			}

			s_instance = this;
			DontDestroyOnLoad(gameObject);

			if (m_globalContextPrefab == null)
			{
				Logger.Error("No GlobalContext was set on App. One is needed to start.");
				return;
			}

			StartCoroutine(BootLoad());
		}

		private IEnumerator BootLoad()
		{
			GameObject contextParent = new GameObject("Contexts");
			contextParent.transform.SetParent(transform);

			m_globalContext = m_globalContextPrefab.Clone<GlobalContext>();
			m_globalContext.Initialise(contextParent);

			yield return StartCoroutine(m_globalContext.LoadRoutine());
			
			// Ready start loading the first scene
			SceneManager sceneManager = m_globalContext.GetContextObject<SceneManager>();
			if (sceneManager == null)
			{
				Logger.Error("GlobalContext did not have a SceneManager. You need one in there with scenes set up.");
				yield return null;
			}

			AsyncOperation loadOperation = sceneManager.LoadSceneAsync(SceneType.SceneB, SceneManager.LoadMode.Single);
			yield return loadOperation;
		}

		private void OnApplicationQuit()
		{
			m_globalContext.Shutdown();
		}

		private void OnDestroy()
		{
			// Not used
		}
	}
}