using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	public interface IEditorDrawable
	{
		Rect Draw(SerializedProperty property, Rect rect);
	}
}