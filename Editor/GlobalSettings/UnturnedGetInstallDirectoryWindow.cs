using System.IO;
using UnityEngine;
using UnityEditor;

namespace LB3D.PuggosWorld.Unturned
{
    public class GetInstallDirectoryWindow : EditorWindow
    {
        public static string GetUnturnedInstallationPath()
        {
            string path = EditorUtility.OpenFolderPanel("Overwrite with png", "", "png");
            if (path.Length != 0)
            {
                return path;
            }
            return null;
        }
    }
}