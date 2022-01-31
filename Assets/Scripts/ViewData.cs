using System.Runtime.Serialization;
using UnityEngine;

namespace VictorElselam.Scripts
{
    public class ViewData
    {
        //empty constructor for deserializer
        public ViewData()
        {  }

        public ViewData(string text, Color color, Texture image)
        {
            Text = text;
            Color = color;
            Image = image;
        }

        //this will run post deserialize, so we already have all data and is safe to make the conversions
        [OnDeserialized]
        public void OnDeserialized(StreamingContext streamingContext)
        {
            Text = text;
            Color = color.HexToColor();
            Image = image.GetTextureFromPath();
        }

        public string text;
        public string color;
        public string image;

        public string Text { get; private set; }
        public Color Color { get; private set; }
        public Texture Image { get; private set; }
    }
}