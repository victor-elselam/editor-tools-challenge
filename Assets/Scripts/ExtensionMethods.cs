using System.Collections.Generic;
using UnityEngine;

namespace VictorElselam.Scripts
{
    public static class ExtensionMethods
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;
        public static bool IsNullOrEmpty(this string str) => str == null || str.Length == 0;
        public static bool IsNullOrEmpty<T, T2>(this IDictionary<T, T2> collection) => collection == null || collection.Count == 0;

        public static Color HexToColor(this string hexadecimal, float alpha = 1f)
        {
            var intHex = int.Parse(hexadecimal.Replace("#", "0x"));
            var rgbColor = new Color
            {
                r = ((intHex >> 16) & 0xFF) / 255.0f,
                g = ((intHex >> 8) & 0xFF) / 255.0f,
                b = (intHex & 0xFF) / 255.0f,
                a = alpha
            };

            return rgbColor;
        }

        public static Sprite GetSpriteFromResources(this string path)
        {
            var texture = Resources.Load<Texture2D>(path);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }

        public static T GetComponentOrInChildren<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (!component) //normally I would use '??', but Unity has the annoying false null (or Missing), so is safer to check in Unity way.
                component = gameObject.GetComponentInChildren<T>(true);

            return component;
        }
    }
}
