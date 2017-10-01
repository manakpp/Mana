using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;

namespace Mana
{
	/// <summary>
	/// Base class of list types.
	/// Inherit for each type because Unity doesn't support generic editors.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ACoreListEditor<T> : TypedEditor<T> where T : MonoBehaviour
	{
		protected SerializedProperty m_list;
		protected string m_listTitle; // Set this to change the title

		const string DEFAULT_TITLE = "Objects";

		protected override void Initialise()
		{
			m_list = serializedObject.FindProperty("m_list");
		}

		protected override void Shutdown()
		{
			// Not used
		}

		protected override void Render()
		{
			ReorderableListGUI.Title(string.IsNullOrEmpty(m_listTitle) ? DEFAULT_TITLE : m_listTitle);
			ReorderableListGUI.ListField(m_list, ReorderableListFlags.ShowIndices);
		}
	}
}