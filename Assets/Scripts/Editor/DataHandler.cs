using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VictorElselam.Scripts.Editor
{
    public class DataHandler
    {
        private History history;

        public DataHandler(History history) => this.history = history;

        public void StartApplyProcess(Object dataFile, IEnumerable<GameObjectSelection> gameObjectSelectionList)
        {
            var viewData = dataFile.ReadFileAs<List<ViewData>>();
            var dataSetters = GetDataSetters(gameObjectSelectionList);

            //each time we apply, it generates a history of the current state
            history.AddState(dataSetters);
            ApplyData(dataSetters.ToList(), viewData);
        }

        public IEnumerable<GameObject> StartRevertProcess()
        {
            //every time we revert, it gives us back the last state and removes it from the list
            var state = history.GetLastState();
            var viewData = state.Select(s => s.Value).ToList();
            var dataSetters = state.Select(s => s.Key).ToList();

            ApplyData(dataSetters, viewData);

            return dataSetters.Select(ds => ds.gameObject);
        }

        private void ApplyData(List<DataSetter> dataSetters, List<ViewData> viewData)
        {
            //it was not clear in the test description what to do in this situation, so I just repeated the configuration.
            var currentItem = 0;
            while (currentItem < dataSetters.Count)
            {
                for (int i = 0; i < viewData.Count; i++, currentItem++)
                {
                    if (currentItem >= dataSetters.Count)
                        break;
                    dataSetters[currentItem].ApplyData(viewData[i]);
                    EditorUtility.SetDirty(dataSetters[currentItem].gameObject);
                }
            }

            AssetDatabase.SaveAssets();
        }

        private IEnumerable<DataSetter> GetDataSetters(IEnumerable<GameObjectSelection> gameObjectSelection) =>
            gameObjectSelection.Where(gosl => gosl.IsToggled).Select(gosl => gosl.DataSetter);
    }
}