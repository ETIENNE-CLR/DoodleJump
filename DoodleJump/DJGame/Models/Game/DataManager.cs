using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DJGame.Models.Game
{
    static class DataManager
    {
        // Champs de la classe...
        private const string SAVE_DATA_PATH = "../../../data.json";

        // Méthode de la classe...
        public static Dictionary<string, int> GetSaves()
        {
            string json = File.ReadAllText(SAVE_DATA_PATH);
            Dictionary<string, int> saves = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
            return saves ?? new Dictionary<string, int>();
        }

        public static void SaveData(Dictionary<string, int> saves)
        {
            string json = JsonConvert.SerializeObject(saves, Formatting.Indented);
            File.WriteAllText(SAVE_DATA_PATH, json);
        }
    }
}
