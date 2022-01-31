using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private History history;
        private DataHandler dataHandler;

        [MenuItem("ProductMadness/PrefabSelection")]
        public static void ShowWindow()
        {
            var window = GetWindow<PrefabSelectionWindow>();
            window.titleContent = new GUIContent("PrefabSelection");
            window.Initialize();
        }

        private void Initialize()
        {
            //it was causing an annoying nullref when refreshing the editor
            history = history == null ? new History() : history;
            dataHandler = dataHandler == null ? new DataHandler(history) : dataHandler;
        }

        public void OnGUI()
        {
            Initialize();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Folder to search:", EditorStyles.whiteLargeLabel);
            GUILayout.Space(5);
            folderToSearch = EditorGUILayout.TextField(folderToSearch);
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
                    {
                        RefreshList(folderToSearch);
                    }
                }

                oldFolderToSearch = folderToSearch;
            }

            GUILayout.Space(15);

            #region Apply/Revert
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh List"))
                RefreshList(folderToSearch);
            if (gameObjectSelectionList.Any(gosl => gosl.IsToggled))
            {
                if (GUILayout.Button("Apply Data"))
                    ApplyData();
            }

            if (history.HasHistory)
            {
                if (GUILayout.Button("Revert"))
                    RevertData();
            }
            EditorGUILayout.EndHorizontal();
            #endregion

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
            history.Clear(); //this is an intended business logic, when you refresh the list your history is lost to avoid confusions when reverting

            var foldersToSearch = !string.IsNullOrEmpty(folderToSearch) ? new[] { folderToSearch } : null;
            var foundGameObjects = ExtensionMethods.FindComponentInProjectGameObjects<Text>(foldersToSearch);

            if (foundGameObjects.IsNullOrEmpty())
                return;

            foundGameObjects.ForEach(fgo => gameObjectSelectionList.Add(new GameObjectSelection(fgo)));
        }

        public void ApplyData()
        {
            if (dataFile == null)
            {
                EditorUtility.DisplayDialog("Error - DataFile null", "DataFile cannot be null!", "OK");
                return;
            }

            var selectedPrefabs = gameObjectSelectionList.Where(gosl => gosl.IsToggled);
            dataHandler.StartApplyProcess(dataFile, selectedPrefabs);

            var names = selectedPrefabs.Select(ds => ds.DataSetter.gameObject).GetGameObjectNames();
            EditorUtility.DisplayDialog("Data applied successfuly", $"Prefabs list: \n{names}", "OK");
        }

        public void RevertData()
        {
            var affectedObjects = dataHandler.StartRevertProcess();

            var names = affectedObjects.GetGameObjectNames();
            EditorUtility.DisplayDialog("Prefabs reverted successly", $"Prefabs list: \n{names}", "OK");
        }
    }
}