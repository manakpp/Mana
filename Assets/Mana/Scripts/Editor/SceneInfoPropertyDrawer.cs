using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace Mana
{
	/// <summary>
	/// TODO: I don't like boilerplate PropertyDrawer code. Perhaps explore abstracting some of this away or use code generation.
	/// </summary>
	[CustomPropertyDrawer(typeof(SceneInfo))]
	public class SceneInfoPropertyDrawer : PropertyDrawer, IEditorDrawable
	{
		// Draw the property inside the given rect
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//// Using BeginProperty / EndProperty on the parent property means that
			//// prefab override logic works on the entire property.
			EditorGUI.BeginProperty(position, label, property);

			//// Draw label
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			//// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			position = Draw(property, position);

			//// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		public Rect Draw(SerializedProperty property, Rect rect)
		{
			string sceneName = property.FindPropertyRelative("m_name").stringValue;
			Object scene = GetSceneFromName(sceneName);

			Color originalColor = GUI.backgroundColor;
			if (scene == null)
			{
				GUI.backgroundColor = Color.red;
			}

			rect.y += 2;
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("m_type"), GUIContent.none);
			EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("m_name"), GUIContent.none);

			if (scene == null)
			{
				GUI.backgroundColor = originalColor;
			}

			return rect;
		}

		public static Object GetSceneFromName(string sceneName)
		{
			if (!string.IsNullOrEmpty(sceneName))
			{
				var guids = AssetDatabase.FindAssets(sceneName + " t:scene", new[] { "Assets" });
				if (guids.Length > 0)
				{
					for (int i = 0; i < guids.Length; ++i)
					{
						string name = AssetDatabase.GUIDToAssetPath(guids[i]);
						if (System.IO.Path.GetFileNameWithoutExtension(name) == sceneName)
						{
							Object obj = AssetDatabase.LoadMainAssetAtPath(name);
							return obj;
						}
					}

				}
			}
			return null;
		}
	}
}