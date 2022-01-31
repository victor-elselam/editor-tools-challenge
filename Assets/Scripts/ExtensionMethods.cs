using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VictorElselam.Scripts
{
    public static class ExtensionMethods
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();

        public static Color HexToColor(this string hexadecimal, float alpha = 1f)
        {
            var intHex = Convert.ToInt32(hexadecimal.Replace("#", "0x"), 16);
            var rgbColor = new Color
            {
                r = ((intHex >> 16) & 0xFF) / 255.0f,
                g = ((intHex >> 8) & 0xFF) / 255.0f,
                b = (intHex & 0xFF) / 255.0f,
                a = alpha
            };

            return rgbColor;
        }

        public static Texture2D GetTextureFromPath(this string path)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/{path}");
        }

        public static T GetComponentOrInChildren<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (!component) //normally I would use '??', but Unity has the false null (or Missing), so is safer to check in Unity way.
                component = gameObject.GetComponentInChildren<T>(true);

            return component;
        }

        public static string GetGameObjectNames(this IEnumerable<GameObject> gameObjects)
        {
            string names = "";
            foreach (var prefab in gameObjects)
                names += $"{prefab.name} \n";

            return names;
        }

#if UNITY_EDITOR
        public static T ReadFileAs<T>(this UnityEngine.Object obj)
        {
            var raw = File.ReadAllText(AssetDatabase.GetAssetPath(obj));
            return JsonConvert.DeserializeObject<T>(raw); //newtonsoft can desserialize lists, Unity json can't. Also, tried with UPM, but looks like it's not available in 2018.4
        }

        public static List<GameObject> FindComponentInProjectGameObjects<T>(string[] foldersToSearch = null) where T : Component
        {
            //finds all assets with type of GameObject in specified folders and return the GUID,
            //then convert the GUID to path and load the gameObject from path. After that we search for the ones that have a Text or a child have a Text.
            return AssetDatabase.FindAssets("t:GameObject", foldersToSearch)
                   .Select(g => AssetDatabase.GUIDToAssetPath(g))
                   .Select(p => AssetDatabase.LoadAssetAtPath<GameObject>(p))
                   .Where(go => go.GetComponent<T>() || go.GetComponentInChildren<T>(true)) //search in the component and in inactive childs
                   .ToList();
        }
#endif
    }
}
