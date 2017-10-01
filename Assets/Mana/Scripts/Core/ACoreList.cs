using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Verifies that there are no duplicates added to the list
	/// Not using a hash set because Unity doesn't render it hand I don't want to make a renderer for it.
	/// Base class of a class that will just be used as list (might be overkill but looks nice in editor)
	/// Abstract because Unity doesn't support generic editors
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ACoreList<T> : MonoBehaviour
	{
		[SerializeField]
		protected List<T> m_list = new List<T>();

		public T this[int index]
		{
			get { return m_list[index]; }
			private set { m_list[index] = value; }
		}

		public int Count { get { return m_list.Count; } }
		public List<T> List { get { return m_list; } }

		public void Add(T element)
		{
			if (element == null)
			{
				Logger.Warning("Index ([{0}]) | You've attempted to add a element but it is null.", GetType());
				return;
			}

			if (!m_list.Contains(element))
			{
				m_list.Add(element);
			}
			else
			{
				Logger.Warning("Index ([{0}]) | You've attempted to add [{1}] but it has already been added.", GetType(), element);
			}
		}

		public void Remove(T element)
		{
			if (m_list == null)
			{
				Logger.Warning("Index ([{0}]) | You've attempted to remove a element but it is null.");
				return;
			}

			if (m_list.Contains(element))
			{
				m_list.Remove(element);
			}
			else
			{
				Logger.Warning("Remove", "Index ([{0}]) | You've attempted to removed [{1}] but isn't in the list.", GetType(), element);
			}
		}
	}
}