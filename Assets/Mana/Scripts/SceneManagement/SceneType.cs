using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// Enums for mapping with scenes. Enforces typed scenes instead of working with strings.
	/// Customise this enum per App.
	/// TODO: Explore code generation solution instead.
	/// </summary>
	public enum SceneType
	{
		SceneA,
		SceneB,

		MAX,
	}
}