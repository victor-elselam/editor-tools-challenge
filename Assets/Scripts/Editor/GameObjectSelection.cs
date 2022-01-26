using UnityEditor;
using UnityEngine;

namespace VictorElselam.Scripts.Editor
{
    public class GameObjectSelection : PropertyDrawer
    {
        public bool IsToggled => isToggled;
        private bool isToggled;

        private GameObject gameObject;
        public DataSetter DataSetter => gameObject.GetComponent<DataSetter>();

        public GameObjectSelection(GameObject gameObject) => this.gameObject = gameObject;

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(200));
            
            isToggled = EditorGUILayout.Toggle(isToggled);
            GUILayout.Space(0);
            GUILayout.Box(PrefabUtility.GetIconForGameObject(gameObject), new GUIStyle() { fixedWidth = 20, fixedHeight = 20 });
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(gameObject.name);
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(getPathWithoutExtension(gameObject), GUILayout.Width(1000));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();


            string getPathWithoutExtension(GameObject gameObject) => AssetDatabase.GetAssetPath(gameObject).Replace(".prefab", "");
        }
    }
}