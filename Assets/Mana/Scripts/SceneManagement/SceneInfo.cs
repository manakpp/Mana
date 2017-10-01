using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mana
{
	/// <summary>
	/// TODO: Add more values that could be useful for scene loading such as how a scene might load (might be info that belongs elsewhere)
	/// </summary>
	[System.Serializable]
	public class SceneInfo
	{
		[SerializeField]
		private SceneType m_type;

		[SerializeField]
		private string m_name;

		public SceneType Type { get { return m_type; } }
		public string Name { get { return m_name; } }

		public SceneInfo(SceneType type, string name)
		{
			m_type = type;
			m_name = name;
		}
	}
}