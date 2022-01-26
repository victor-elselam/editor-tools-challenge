using UnityEngine.UI;

namespace VictorElselam.Scripts
{
    public class CardView : DataSetter
    {
        public Image CardImage => ImageField;
        public Text CardName => TextField;
    }
}