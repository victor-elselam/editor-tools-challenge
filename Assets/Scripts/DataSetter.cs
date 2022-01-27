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

            if (TextField)
            {
                TextField.text = viewData.Text;
                TextField.color = viewData.Color;
            }
            else
            {
                //this will warn user if the code couldn't resolve the components
                Debug.LogError($"{nameof(TextField)} not found in {gameObject.name}");
            }

            if (ImageField)
            {
                ImageField.sprite = viewData.Image;
                ImageField.color = Color.white;
            }
            else
            {
                //this will warn user if the code couldn't resolve the components
                Debug.LogError($"{nameof(ImageField)} not found in {gameObject.name}");
            }
        }

        //this will deal with misconfigured prefabs in some situations.
        private void TryToResolveComponents()
        {
            if (!TextField)
                TextField = gameObject.GetComponentOrInChildren<Text>();

            if (!ImageField)
                ImageField = gameObject.GetComponentOrInChildren<Image>();
        }

        public ViewData GetCurrentViewData()
        {
            var text = TextField ? TextField.text : "";
            var color = TextField ? TextField.color : Color.white;
            var image = ImageField ? ImageField.sprite : null;

            return new ViewData(text, color, image);
        }
    }
}