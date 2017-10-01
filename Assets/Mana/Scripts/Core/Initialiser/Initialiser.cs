using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	public class Initialiser : ACoreList<IInitialisationDelegate>
	{
		private bool m_hasInitialiesd = false;
		private bool m_hasInitialiesdPostLoad = false;
		private bool m_hasShutdown = false;

		/// <summary>
		/// Called first before any sort of data is loaded.
		/// Use this to set up internally and find references to other "Manager" category objects.
		/// </summary>
		public void Initialise()
		{
			if (m_hasInitialiesd)
			{
				Logger.Warning("You're calling Initialise more than once. If this is what you want then you can ignore or remove this warning.");
			}

			int count = m_list.Count;
			for (int i = 0; i < count; ++i)
			{
				m_list[i].Initialise();
			}

			m_hasInitialiesd = true;
		}

		/// <summary>
		/// Called after data and save data has been loaded.
		/// Use to restore the game to the correct state after reloading the game OR
		/// use to set the game up to a fresh state.
		/// </summary>
		public void InitialisePostLoad()
		{
			if (m_hasInitialiesdPostLoad)
			{
				Logger.Warning("You're calling InitialisePostLoad more than once. If this is what you want then you can ignore or remove this warning.");
			}

			int count = m_list.Count;
			for (int i = 0; i < count; ++i)
			{
				m_list[i].InitialisePostLoad();
			}
		}

		/// <summary>
		/// Called to shut down the game safely (probably call from Application.Quit or something similar).
		/// Use to finalise anything important such as saving the game or halting OS level operations.
		/// Very important that if you do save the game, it saves correctly.
		/// </summary>
		public void Shutdown()
		{
			if (!m_hasInitialiesd)
			{
				Logger.Warning("You're calling Shutdown when m_hasInitialiesd is not true.");
			}

			if (!m_hasInitialiesdPostLoad)
			{
				Logger.Warning("You're calling Shutdown when m_hasInitialiesdPostLoad is not true.");
			}

			if (m_hasShutdown)
			{
				Logger.Warning("You're calling Shutdown more than once. If this is what you want then you can ignore or remove this warning.");
			}

			int count = m_list.Count;
			for (int i = 0; i < count; ++i)
			{
				m_list[i].Shutdown();
			}
		}
	}
}