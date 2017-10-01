using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Base class of editor scripts.
	/// An attempt to make it easier to work with editor code.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class TypedEditor<T> : UnityEditor.Editor where T : MonoBehaviour
	{
		protected T m_target;
		protected MonoScript m_script;
		protected MonoScript m_editorScript;
		protected bool m_isUsingDefaultInspector;

		private bool IsInitialiesd { get { return m_target != null && m_script != null; } }

		/// <summary>
		/// Called in place of OnEnable
		/// </summary>
		protected abstract void Initialise();

		/// <summary>
		/// Called in place of OnDisable
		/// You'll want to do clean up here (eg. un-register from events)
		/// </summary>
		protected abstract void Shutdown();

		/// <summary>
		/// Wraps OnInspectorGUI with Update and ApplyModified properties (I'm making the assumption at you always want this)
		/// </summary>
		protected abstract void Render();

		/// <summary>
		/// Converts the target to the actual type
		/// Finds the script so that it can render it
		/// Called by Unity
		/// </summary>
		private void OnEnable()
		{
			m_target = target as T;
			if (m_target == null)
			{
				Logger.Error("Could not cast type to type [{0}]", typeof(T).ToString());
				return;
			}

			m_script = MonoScript.FromMonoBehaviour(m_target);
			m_editorScript = MonoScript.FromScriptableObject(this);

			Initialise();
		}

		private void OnDisable()
		{
			// Not used
		}

		/// <summary>
		/// Updates and renders UI elements
		/// Called by Unity
		/// </summary>
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (m_isUsingDefaultInspector)
			{
				RenderEditorScriptField();
				DrawDefaultInspector();
			}
			else if (!IsInitialiesd)
			{
				EditorGUILayout.HelpBox("Custom editor was not initialiesd correctly. Rendering default.", MessageType.Warning);
				RenderEditorScriptField();
				DrawDefaultInspector();
			}
			else
			{
				RenderScriptFields();
				Render();

				// TODO: May be more efficient to only apply when changes are actually made
				serializedObject.ApplyModifiedProperties();
			}
		}

		/// <summary>
		/// Only renders editor script field
		/// </summary>
		private void RenderEditorScriptField()
		{
			GUI.enabled = false;
			m_editorScript = EditorGUILayout.ObjectField(m_isUsingDefaultInspector ? "Editor (Default)" : "Editor", m_editorScript, typeof(MonoScript), false) as MonoScript;
			GUI.enabled = true;
		}

		/// <summary>
		/// Renders the script field similar (but not the same) to how Unity does it.
		/// </summary>
		private void RenderScriptFields()
		{
			GUI.enabled = false;
			m_editorScript = EditorGUILayout.ObjectField("Editor", m_editorScript, typeof(MonoScript), false) as MonoScript;
			m_script = EditorGUILayout.ObjectField("Script", m_script, typeof(MonoScript), false) as MonoScript;
			GUI.enabled = true;
		}
	}
}