using System;
using System.Text.Json;

namespace SystemGlobalServicesTask
{
    public static class JsonHelper
    {
        public static T DownloadSerializedJsonData<T>(string url) where T : new()
        {
            using (var w = new System.Net.WebClient())
            {
                var jsonData = string.Empty;
                try
                {
                    jsonData = w.DownloadString(url);
                }
                catch (Exception)
                {
                }

                return !string.IsNullOrEmpty(jsonData) ? JsonSerializer.Deserialize<T>(jsonData) : new T();
            }
        }
    }
}