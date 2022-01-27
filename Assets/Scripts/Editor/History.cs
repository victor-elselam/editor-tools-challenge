using System.Collections.Generic;

namespace VictorElselam.Scripts.Editor
{
    public class History
    {
        private Stack<Dictionary<DataSetter, ViewData>> historyList;

        public History() => historyList = new Stack<Dictionary<DataSetter, ViewData>>();

        //depending on the domain, maybe history should not be responsable for this conversion, but looks cleaner in this case.
        public void AddState(IEnumerable<DataSetter> dataSetters)
        {
            var dictionary = new Dictionary<DataSetter, ViewData>();
            foreach (var dataSetter in dataSetters)
                dictionary[dataSetter] = dataSetter.GetCurrentViewData();

            AddState(dictionary);
        }

        public void AddState(Dictionary<DataSetter, ViewData> state) => historyList.Push(state);
        public Dictionary<DataSetter, ViewData> GetLastState() => historyList.Pop();
        public bool HasHistory => !historyList.IsNullOrEmpty();
        public void Clear() => historyList.Clear();
    }
}