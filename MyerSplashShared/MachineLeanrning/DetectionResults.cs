using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyerSplashShared.MachineLeanrning
{
    public static class DetectionResults
    {
        private static List<string> _labels = new List<string>();

        public static async Task InitAsync()
        {
            await LoadLabelsAsync();
        }

        private static async Task LoadLabelsAsync()
        {
            if (_labels.Count > 0) return;

            var labelFile = await StorageFile.GetFileFromApplicationUriAsync(
                new Uri($"ms-appx:///Assets/Json/Labels.json"));

            var fileString = await FileIO.ReadTextAsync(labelFile);
            var fileDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileString);
            foreach (var kvp in fileDict)
            {
                var value = kvp.Value.Split(',').FirstOrDefault().Trim();
                _labels.Add(value);
            }
        }

        public static string FindLabel(int index)
        {
            if (index < 0 || index >= _labels.Count)
            {
                return "";
            }
            else
            {
                return _labels[index];
            }
        }
    }
}
