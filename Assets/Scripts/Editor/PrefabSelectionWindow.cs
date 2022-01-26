using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VictorElselam.Scripts.Editor
{
    public class PrefabSelectionWindow : EditorWindow
    {
        private List<GameObjectSelection> gameObjectSelectionList = new List<GameObjectSelection>();
        private string folderToSearch; //todo: make a folder dragger
        private string oldFolderToSearch; //todo: make a folder dragger

        private Vector2 scrollPos;
        private Object dataFile;

        [MenuItem("ProductMadness/PrefabSelection")]
        public static void ShowWindow()
        {
            var window = GetWindow<PrefabSelectionWindow>();
            window.titleContent = new GUIContent("PrefabSelection");
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Folder to search:", EditorStyles.whiteLargeLabel);
            GUILayout.Space(5);
            folderToSearch = EditorGUILayout.TextField(folderToSearch); //this could be easily extended to multiple folders.

            EditorGUILayout.EndHorizontal();
            dataFile = EditorGUILayout.ObjectField("DataFile", dataFile, typeof(Object));

            if (folderToSearch != oldFolderToSearch)
            {
                if (string.IsNullOrEmpty(folderToSearch))
                    RefreshList(folderToSearch);
                else
                {
                    if (!Directory.Exists(folderToSearch))
                    {
                        EditorGUILayout.LabelField("Invalid Folder");
                        return;
                    }
                    else
                        RefreshList(folderToSearch);
                }

                oldFolderToSearch = folderToSearch;
            }

            GUILayout.Space(15);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Refresh List"))
                RefreshList(folderToSearch);

            if (gameObjectSelectionList.Any(gosl => gosl.IsToggled))
            {
                if (GUILayout.Button("Apply Data"))
                    ApplyData();
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);
            RenderObjects();
            
            EditorGUILayout.EndVertical();
        }

        private void RenderObjects()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (var obj in gameObjectSelectionList)
                obj.OnGUI();

            EditorGUILayout.EndScrollView();
        }

        private void RefreshList(string folderToSearch)
        {
            gameObjectSelectionList.Clear();

            var hasFolderValue = !string.IsNullOrEmpty(folderToSearch);
            var foldersToSearch = hasFolderValue ? new[] { folderToSearch } : null;
            var foundGameObjects = AssetDatabase.FindAssets("t:GameObject", foldersToSearch)
                   .Select(g => AssetDatabase.GUIDToAssetPath(g))
                   .Select(p => AssetDatabase.LoadAssetAtPath<GameObject>(p))
                   .Where(go => go.GetComponent<Text>() || go.GetComponentInChildren<Text>(true)) //search in the component and in inactive childs
                   .ToList();

            if (foundGameObjects.IsNullOrEmpty())
                return;

            foundGameObjects.ForEach(fgo => gameObjectSelectionList.Add(new GameObjectSelection(fgo)));
        }

        private void ApplyData()
        {
            var raw = File.ReadAllText(AssetDatabase.GetAssetPath(dataFile)).Replace("\n", "").Replace("\t", "").Replace("\\\"", "");
            var desserialized = JsonUtility.FromJson<ViewData[]>(raw);
            var selectedPrefabs = gameObjectSelectionList.Where(gosl => gosl.IsToggled).ToList();

            //it was not clear in the test what to do in this situation, so I just repeated the configuration.
            var currentItem = 0;
            while (currentItem < selectedPrefabs.Count) 
            {
                for (int i = 0; i < desserialized.Length; i++, currentItem++)
                    selectedPrefabs[currentItem].DataSetter.ApplyData(desserialized[i]);
            }
        }
    }
}