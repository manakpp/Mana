using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using UnityEditorInternal;
using System.Collections.Generic;

namespace Mana
{
	[CustomEditor(typeof(GameObjectList))]
	public class InitialisationObjectListEditor : ACoreListEditor<GameObjectList>
	{
		protected override void Initialise()
		{
			base.Initialise();

			m_listTitle = "Initialisation Objects";
		}
	}
}