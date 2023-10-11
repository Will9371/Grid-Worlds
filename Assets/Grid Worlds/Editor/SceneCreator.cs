// Revisit this later, ran into problems renaming files
// Interactive menu version relies on: https://github.com/gkngkc/UnityStandaloneFileBrowser

/*
using UnityEditor;
using UnityEngine;
using System.IO;
using SFB;

//[CreateAssetMenu(menuName = "Grid Worlds/Scene Creator")]
public class SceneCreator : MonoBehaviour
{
    [MenuItem("Custom/Rename Files in Folder")]
    static void RenameFilesWithPrefix()
    {
        // Open a file browser to select the folder interactively
        string[] folderPaths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false);
        if (folderPaths.Length == 0)
        {
            // User canceled the folder selection
            return;
        }

        string folderPath = folderPaths[0];

        // Prompt the user for the common prefix
        string commonPrefix = EditorUtility.DisplayDialog("Enter Prefix", "Enter the common prefix to add to file names:", "Rename", "Cancel")
            ? EditorUtility.OpenDialog("Rename", "NewPrefix ", "Add Prefix") // User input or default prefix
            : string.Empty; // Cancelled or empty prefix

        if (string.IsNullOrEmpty(commonPrefix))
        {
            Debug.Log("Prefix input is empty or user canceled.");
            return;
        }

        // Get an array of file paths in the folder
        string[] files = Directory.GetFiles(folderPath);

        foreach (string filePath in files)
        {
            if (AssetDatabase.Contains(filePath))
            {
                // Calculate the new file name with the common prefix
                string fileName = Path.GetFileName(filePath);
                string newFileName = commonPrefix + fileName;

                // Rename the asset
                AssetDatabase.RenameAsset(filePath, newFileName);
            }
        }

        // Refresh the asset database to reflect the changes
        AssetDatabase.Refresh();
    }
}
*/

/*[Header("References")]
   [SerializeField] SceneAsset scene;
   [SerializeField] GameObject prefab;
   [SerializeField] ScriptableObject layout;
   [SerializeField] ScriptableObject objective;
   
   [Header("Settings")]
   [SerializeField] new string name;
   //[SerializeField] bool apply;
   
   void OnValidate()
   {
       if (apply)
       {
           apply = false;
           ApplyChanges();
       }
   }
   
   [MenuItem("Grid Worlds/Rename Level Files")]
   static void Rename()
   {
       Rename(scene.name, name);
       Rename(prefab.name, $"{name} Environment");
       Rename(layout.name, $"{name} Layout");
       Rename(objective.name, $"{name} Objective");

       AssetDatabase.Refresh();       
   }
   
   static void Rename(string asset, string newName)
   {
       if (asset == null) return;
       var path = AssetDatabase.GUIDToAssetPath(asset);
       AssetDatabase.RenameAsset(path, newName);        
   }*/
