using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Syntactic sugar for finding components
	/// </summary>
	public static class FindComponentExtension
	{
		[System.Flags]
		public enum SearchType : int
		{
			None = 0,
			Self = 1,
			Parent = 2,
			Children = 4,
			Parents = 8,

			All = Self | Children | Parents, // Only supports this order of searching
		}
		
		public static class SearchTypeUtility
		{
			public static bool HasFlag(SearchType a, SearchType b)
			{
				return (a & b) == b;
			}

			public static SearchType AddFlag(SearchType a, SearchType b)
			{
				return a | b;
			}

			public static SearchType RemoveFlag(SearchType a, SearchType b)
			{
				return a & (~b);
			}

			public static SearchType ToggleFlag(SearchType a, SearchType b)
			{
				return a ^ b;
			}
		}

		public static T FindComponent<T>(this GameObject gameObject, SearchType searchType = SearchType.Self, bool showWarnings = false) where T : MonoBehaviour
		{
			if (gameObject == null)
			{
				if (showWarnings)
				{
					Logger.Warning("Looking for type [{0}] but passed GameObject was null", typeof(T));
				}

				return null;
			}

			T behaviour = null;

			if (SearchTypeUtility.HasFlag(searchType, SearchType.Self))
			{
				behaviour = gameObject.GetComponent<T>();
			}

			if (behaviour == null &&
				SearchTypeUtility.HasFlag(searchType, SearchType.Children))
			{
				behaviour = gameObject.GetComponentInChildren<T>();
			}

			if (behaviour == null &&
				SearchTypeUtility.HasFlag(searchType, SearchType.Parent) &&
				!SearchTypeUtility.HasFlag(searchType, SearchType.Parents))
			{
				Transform parent = gameObject.transform.parent;
				if (parent != null)
				{
					behaviour = gameObject.GetComponentInParent<T>();
				}
			}

			if (behaviour == null &&
			SearchTypeUtility.HasFlag(searchType, SearchType.Parents))
			{
				Transform parent = gameObject.transform.parent;
				while (parent != null)
				{
					behaviour = gameObject.GetComponent<T>();
					if (behaviour != null)
					{
						break;
					}

					parent = parent.parent;
				}
			}

			if (behaviour == null)
			{
				if (showWarnings)
				{
					Logger.Warning("Could not find component of type [{0}] in [{1}] using SearchType [{2}]",
						typeof(T), gameObject, searchType);
				}
			}

			return behaviour;
		}

		/// <summary>
		/// List is so conserve memory (don't want to reallocate if it isn't necessary).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gameObject"></param>
		/// <param name="results"></param>
		/// <param name="searchType"></param>
		/// <param name="includeInactive"></param>
		/// <returns></returns>
		public static List<T> FindComponents<T>(this GameObject gameObject, List<T> results, SearchType searchType = SearchType.Self, bool includeInactive = true, bool showWarnings = false) where T : MonoBehaviour
		{
			if (gameObject == null)
			{
				if (showWarnings)
				{
					Logger.Warning("Looking for type [{0}] but passed GameObject was null", typeof(T));
				}
				return results;
			}

			if (results == null)
			{
				if (showWarnings)
				{
					Logger.Warning("List specified for [{0}] was null", typeof(T));
				}
				return results;
			}

			int originalCount = results.Count;

			if (SearchTypeUtility.HasFlag(searchType, SearchType.Self))
			{
				T behaviour = gameObject.GetComponent<T>();
				if (behaviour != null && !results.Contains(behaviour))
				{
					results.Add(behaviour);
				}
			}

			if (SearchTypeUtility.HasFlag(searchType, SearchType.Children))
			{
				gameObject.GetComponentsInChildren<T>(includeInactive, results);
			}

			if (SearchTypeUtility.HasFlag(searchType, SearchType.Parent) &&
				!SearchTypeUtility.HasFlag(searchType, SearchType.Parents))
			{
				Transform parent = gameObject.transform.parent;
				if (parent != null)
				{
					T behaviour = gameObject.GetComponentInParent<T>();
					if (behaviour != null && !results.Contains(behaviour))
					{
						results.Add(behaviour);
					}
				}
			}

			if (SearchTypeUtility.HasFlag(searchType, SearchType.Parents))
			{
				Transform parent = gameObject.transform.parent;
				while (parent != null)
				{
					gameObject.GetComponents<T>(results);
					parent = parent.parent;
				}
			}

			if (results.Count == originalCount)
			{
				if (showWarnings)
				{
					Logger.Warning("Didn't find any new components of type [{0}] in [{1}] using SearchType [{2}]. StartCount [{3}].",
						typeof(T), gameObject, searchType, originalCount);
				}
			}

			return results;
		}
	}
}