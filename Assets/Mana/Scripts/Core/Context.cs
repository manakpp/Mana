using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Handles context initialisation, shutdown, updating, data loading and save data saving/loading.
	/// Allows access to Context objects by type.
	/// </summary>
	[RequireComponent(typeof(GameObjectList))]
	public class Context : MonoBehaviour
	{
		protected GameObjectList m_initObjectList;
		protected Initialiser m_initialiser;
		protected Updater m_updater;
		protected DataLoader m_dataLoader;
		protected SaveDataLoader m_saveDataLoader;
		protected GameObject m_parentContainer;

		private Dictionary<Type, MonoBehaviour> m_objects;

		public virtual void Initialise(GameObject parent)
		{
			m_parentContainer = parent;
			Load();
		}

		public void Shutdown()
		{
			m_initialiser.Shutdown();
		}

		public T GetContextObject<T>() where T : MonoBehaviour
		{
			Type type = typeof(T);
			if (m_objects.ContainsKey(type)) // TODO: Compile this line out
			{
				return (T)m_objects[type];
			}

			return null;
		}
		
		/// <summary>
		/// Sequences boot up in an organised manner (no weird execution order business here!)
		/// Base context loads everything in one frame 
		/// TODO: Not sure if I want everything to load in one frame (maybe allow it to be loaded over multiple
		/// </summary>
		/// <returns></returns>
		private void Load()
		{
			AddCoreReferences();
			CreateObjectsAndAssignDelegates();

			// Let initialiation objects create whatever they need to (things you would normally do in awake such as finding internal references, allocation, etc...)
			m_initialiser.Initialise();

			// Load local data assets/resources
			m_dataLoader.LoadData();

			// TODO: optionally allow remote data (but only on the global context)
			//yield return StartCoroutine(m_dataLoader.LoadData());

			// Populate data structures using loaded data
			m_initialiser.InitialisePostLoad();

			// Load in save data
			m_saveDataLoader.Load();
		}

		protected void AddCoreReferences()
		{
			// Done differently to others as we need to extract the objects in the list
			m_initObjectList = gameObject.GetComponent<GameObjectList>();
		
			// TODO: Not sure if this dict actually needs to be a fixed size (maybe I want to add stuff later??)
			const int CORE_REFERENCES_COUNT = 4;
			m_objects = new Dictionary<Type, MonoBehaviour>(CORE_REFERENCES_COUNT + m_initObjectList.Count);
			AddContextObjectReferenceByBehaviour(m_initObjectList);

			m_initialiser = AddCoreContextObjectReference<Initialiser>();
			m_updater = AddCoreContextObjectReference<Updater>();
			m_saveDataLoader = AddCoreContextObjectReference<SaveDataLoader>();
			m_dataLoader = AddCoreContextObjectReference<DataLoader>();
		}

		protected void CreateObjectsAndAssignDelegates()
		{
			for (int i = 0; i < m_initObjectList.Count; ++i)
			{
				GameObject initObject = m_initObjectList[i];
				if (initObject == null)
				{
					continue;
				}
				
				GameObject contextObject = initObject.Clone(m_parentContainer.transform, true);
				AssignDelegates(contextObject);

				AddContextObjectReference(contextObject);
			}
		}

		private void AssignDelegates(GameObject contextObject)
		{
			IInitialisationDelegate initialisationDelegate = contextObject.GetComponent<IInitialisationDelegate>();
			if (initialisationDelegate != null)
			{
				m_initialiser.Add(initialisationDelegate);
			}

			IDataLoadDelegate dataloadDelegate = contextObject.GetComponent<IDataLoadDelegate>();
			if (dataloadDelegate != null)
			{
				m_dataLoader.Add(dataloadDelegate);
			}

			ISaveDataDelegate saveDataDelegate = contextObject.GetComponent<ISaveDataDelegate>();
			if (saveDataDelegate != null)
			{
				m_saveDataLoader.Add(saveDataDelegate);
			}

			IUpdateDelegate updateDelegate = contextObject.GetComponent<IUpdateDelegate>();
			if (updateDelegate != null)
			{
				m_updater.Add(updateDelegate);
			}
		}

		private T AddCoreContextObjectReference<T>() where T : MonoBehaviour
		{
			T behaviour = gameObject.AddComponent<T>();
			AddContextObjectReferenceByBehaviour(behaviour);
			return (T)behaviour; // Assume not null :)
		}

		private T GetCoreContextObjectReference<T>() where T : MonoBehaviour
		{
			T behaviour = gameObject.GetComponent<T>();
			AddContextObjectReferenceByBehaviour(behaviour);
			return (T)behaviour; // Assume not null :)
		}

		/// <summary>
		/// This is retrieving the first MonoBehaviour on this GameObject and using it as the key.
		/// This assumption may not always be true so just need to be really careful about this.
		/// TODO: Perhaps find a solution that overcomes this assumption
		/// </summary>
		/// <param name="contextObject"></param>
		private void AddContextObjectReference(GameObject contextObject)
		{
			MonoBehaviour behaviour = contextObject.GetComponent<MonoBehaviour>();
			AddContextObjectReferenceByBehaviour(behaviour);
		}

		private void AddContextObjectReferenceByBehaviour(MonoBehaviour behaviour)
		{
			Type type = behaviour.GetType();
			if (m_objects.ContainsKey(type))
			{
				Logger.Warning("There is a duplicate of object type [{0}] in this context. It will replace the other one.", type); // TODO: Not sure if duplicates is something I'd want.
			}

			m_objects.Add(type, behaviour);
		}
	}
}