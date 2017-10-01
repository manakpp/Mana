using UnityEngine;
using System.Collections;

namespace Mana
{
	/// <summary>
	/// Called in Unity update loop
	/// Allows better control of execution order and deltaTime
	/// </summary>
	/// <param name="deltaTime"></param>
	public class Updater : ACoreList<IUpdateDelegate>
	{
		private bool m_isUpdating;

		public bool IsUpdating { get { return m_isUpdating; } set { m_isUpdating = value; } }

		private void Update()
		{
			// Will only update if you want you tell it to
			if (!m_isUpdating)
			{
				return;
			}

			for (int i = 0; i < m_list.Count; ++i)
			{
				// TODO: Explore other options for updating time so that I can pause the game without using this.
				float deltaTime = Time.deltaTime; 
				m_list[i].Update(deltaTime);
			}
		}
	}
}