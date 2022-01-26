using UnityEngine;
using UnityEngine.UI;

namespace VictorElselam.Scripts
{
    public class DataSetter : MonoBehaviour
    {
        [SerializeField] protected Text TextField;
        [SerializeField] protected Image ImageField;

        public void ApplyData(ViewData viewData)
        {
            TryToResolveComponents();

            //if (TextField)
            //{
            //    TextField.text = viewData.Text;
            //    TextField.color = viewData.Color;
            //}

            //if (ImageField)
            //{
            //    ImageField.sprite = viewData.Image;
            //}
        }

        private void TryToResolveComponents()
        {
            if (!TextField)
                TextField = gameObject.GetComponentOrInChildren<Text>();

            if (!ImageField)
                ImageField = gameObject.GetComponentOrInChildren<Image>();
        }
    }

    public class ViewData
    {
        public string text;
        public string color;
        public string image;

        
    }
}