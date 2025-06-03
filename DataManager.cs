using System.Collections;
using System.IO;
using System.Text.Json;

namespace TM
{
    public static class DataManager
    {
        private static readonly string _path = "data.json";

        /// <summary>
        /// Reads data json and returns json
        /// </summary>
        /// <returns>TaskData list containing tasks from storage</returns>
        public static List<TaskData> FetchData()
        {
            if (!File.Exists(_path)) return new List<TaskData>(); // If we don't have a data file just return an empty list

            var json = File.ReadAllText(_path);

            return JsonSerializer.Deserialize<List<TaskData>>(json) ?? new List<TaskData>(); // Deserialize json in an object list, if it fails return an empty one
        }

        /// <summary>
        /// Writes a data list into storage
        /// </summary>
        /// <param name="tasks">A list to write into storage</param>
        public static void WriteData(List<TaskData> tasks)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var newJson = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_path, newJson);
        }
    }
}
