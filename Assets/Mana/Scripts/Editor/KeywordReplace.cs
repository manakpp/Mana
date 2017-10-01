// Ref: http://forum.unity3d.com/threads/c-script-template-how-to-make-custom-changes.273191/
using UnityEngine;
using UnityEditor;

namespace Mana.Editor
{
	/// <summary>
	/// Utility for inserting my namespace on new files
	/// TODO: Remove this from project when not using it
	/// </summary>
    public class KeywordReplace : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
			if (index <= -1)
				return;

			string file = path.Substring(index);
            if (file != ".cs" && file != ".js" && file != ".boo")
                return;

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            file = System.IO.File.ReadAllText(path);

            file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);

            System.IO.File.WriteAllText(path, file);
            AssetDatabase.Refresh();
        }

        public static string GetProjectName()
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            Debug.Log("project = " + projectName);
            return projectName;
        }
    }
}