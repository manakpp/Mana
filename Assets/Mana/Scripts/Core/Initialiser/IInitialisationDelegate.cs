using UnityEngine;
using System.Collections;

namespace Mana
{
	public interface IInitialisationDelegate
	{
		/// <summary>
		/// Used to create data structures that is not dependent on anything else
		/// Should/Will be called at the earliest time possible (Awake time)
		/// </summary>
		void Initialise();

		/// <summary>
		/// Used to intialise self once all other dependencies have loaded
		/// Call into the App class to retrieve any references needed
		/// </summary>
		void InitialisePostLoad();

		/// <summary>
		/// Called via OnDestroy to control the shutdown order of the app
		/// </summary>
		void Shutdown();
	}
}