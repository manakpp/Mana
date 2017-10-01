using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using UnityEditorInternal;
using System.Collections.Generic;

namespace Mana
{
	[CustomEditor(typeof(SceneInfoList))]
	public class SceneInfoListEditor : ACoreListEditor<SceneInfoList>
	{
		protected override void Initialise()
		{
			base.Initialise();

			m_listTitle = "Scenes";
		}
	}
}