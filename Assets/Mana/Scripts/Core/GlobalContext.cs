using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// GlobalContext is used to...
	/// - create all essential objects
	/// - load all initial data 
	/// - load all save data
	/// Also has additional info for logging
	/// Any critical systems should be attached to the GlobalContext (this is probably everything in a small game)
	/// </summary>
	public class GlobalContext : Context
	{
		public enum LoadStep
		{
			NONE,

			AddCoreReferences,
			CreateObjectsAndAssignDelegates,
			Initialiser_Initialise,
			DataLoader_LoadData,
			Initialiser_InitialisePostData,
			SaveDataLoader_Load,
			Ready,

			MAX
		}

		private LoadStep m_loadStep;

		/// <summary>
		/// Can use this to log out to screen
		/// </summary>
		public LoadStep Step { get { return m_loadStep; } }

		public override void Initialise(GameObject parent)
		{
			m_parentContainer = parent;
		}

		/// <summary>
		/// Sequences boot up in an organised manner (no weird execution order business here!)
		/// To be called from App
		/// Note that execution will leave Bootload at the first yield 
		/// This means that other MonoBehaviours will invoke Awake....
		/// because of this, it is recommended that you do not use Awake to find references to other external objects
		/// TODO: Make this more easily configurable for different load behaviours
		/// TODO: Add options/steps for connecting to a remote server (or going online)
		/// </summary>
		/// <returns></returns>
		public IEnumerator LoadRoutine()
		{
			SetLoadStep(LoadStep.AddCoreReferences);
			AddCoreReferences();

			SetLoadStep(LoadStep.CreateObjectsAndAssignDelegates);
			CreateObjectsAndAssignDelegates();

			// Let initialiation objects create whatever they need to (things you would normally do in awake such as finding internal references, allocation, etc...)
			SetLoadStep(LoadStep.Initialiser_Initialise);
			m_initialiser.Initialise();

			// Load local data assets/resources
			SetLoadStep(LoadStep.DataLoader_LoadData);
			m_dataLoader.LoadData();

			// TODO: optionally allow remote data (but only on the global context)
			//yield return StartCoroutine(m_dataLoader.LoadData());

			// Populate data structures using loaded data
			SetLoadStep(LoadStep.Initialiser_InitialisePostData);
			m_initialiser.InitialisePostLoad();

			// Load in save data
			SetLoadStep(LoadStep.SaveDataLoader_Load);
			m_saveDataLoader.Load();

			yield return null; // Necessary until other steps are added
		}

		private void SetLoadStep(LoadStep step)
		{
			m_loadStep = step;
			Logger.Log(step.ToString());
		}
	}
}