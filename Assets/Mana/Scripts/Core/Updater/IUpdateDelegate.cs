using UnityEngine;
using System.Collections;

namespace Mana
{
	public interface IUpdateDelegate
	{
		/// <summary>
		/// Used to update objects in a controlled order.
		/// Allows non-MonoBehaviours to get into the update loop.
		/// Just use however you would use the Unity update method.
		/// </summary>
		/// <param name="deltaTime"></param>
		void Update(float deltaTime);
	}
}