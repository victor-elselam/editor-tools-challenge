using UnityEngine.UI;

namespace VictorElselam.Scripts
{
    public class CardView : DataSetter
    {
        public RawImage CardImage => ImageField;
        public Text CardName => TextField;
    }
}