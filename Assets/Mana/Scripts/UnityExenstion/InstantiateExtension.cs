using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Syntactic sugar for instantiating GameObjects
	/// </summary>
	public static class InstantiateExtension
	{
		public static GameObject Clone(this GameObject gameObject, Transform parent = null, bool instantiateInWorldSpace = false)
		{
			if (gameObject == null)
			{
				Logger.Warning("You tried to instantiate something but it was null. I can't log anything for you. Check the stack.");
				return null;
			}

			GameObject createdObject = GameObject.Instantiate(gameObject, parent, instantiateInWorldSpace);
			return createdObject;
		}

		public static T Clone<T>(this GameObject gameObject, Transform parent = null, bool instantiateInWorldSpace = false,
			FindComponentExtension.SearchType searchType = FindComponentExtension.SearchType.Self, bool showSearchWarnings = true) where T : MonoBehaviour
		{
			GameObject createdObject = gameObject.Clone(parent, instantiateInWorldSpace);
			if (createdObject == null)
			{
				return null;
			}

			T type = createdObject.FindComponent<T>(searchType, showSearchWarnings);
			if (createdObject == null)
			{
				Logger.Warning("You tried to find component of type [{0}] on [{1}] but it doesn't exist.", typeof(T), createdObject.name);
				return null;
			}

			return type;
		}

		public static Component Clone(this Component behaviour, Transform parent = null, bool instantiateInWorldSpace = false)
		{
			if (behaviour == null || behaviour.gameObject == null)
			{
				Logger.Warning("You tried to instantiate something but it was null. I can't log anything for you. Check the stack.");
				return null;
			}
			
			GameObject createdObject = GameObject.Instantiate(behaviour.gameObject, parent, instantiateInWorldSpace);
			return createdObject.GetComponent(behaviour.GetType());
		}

		public static T Clone<T>(this MonoBehaviour behaviour, Transform parent = null, bool instantiateInWorldSpace = false,
			FindComponentExtension.SearchType searchType = FindComponentExtension.SearchType.Self, bool showSearchWarnings = true) where T : MonoBehaviour
		{
			if (behaviour == null)
			{
				return null;
			}

			GameObject createdObject = behaviour.gameObject.Clone(parent, instantiateInWorldSpace);
			if (createdObject == null)
			{
				return null;
			}

			T type = createdObject.FindComponent<T>(searchType, showSearchWarnings);
			if (createdObject == null)
			{
				Logger.Warning("You tried to find component of type [{0}] on [{1}] but it doesn't exist.", typeof(T), createdObject.name);
				return null;
			}

			return type;
		}
	}
}