using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Game
{
    class Settings
    {
        private static Dictionary<string, int> Values { get; set; }

        public static bool IsEnabled(string name) =>
            GetValue(name) > 0;

        public static int GetValue(string name)
        {
            if (Values == null)
                Load();

            return Values.ContainsKey(name) ? Values[name] : 0;
        } 

        public static void SetValue(string name, int value)
        {
            Values[name] = value;
            Save();
        }

        private static void Load()
        {
            Values = new Dictionary<string, int>();

            if (!IsSettingsSaved())
                return;

            foreach (string setting in (Preferences.Default.Get("Settings", String.Empty) as string).Split(','))
            {
                if (String.IsNullOrEmpty(setting))
                    return;

                string[] value = setting.Split('=');
                Values.Add(value[0], int.Parse(value[1]));
            }
        }

        public static void Clean()
        {
            Preferences.Default.Remove("Settings");
            Load();
        }

        private static void Save()
        {
            string setting = string
                .Join(",", Values.Select(x => x.Key + "=" + x.Value)
                .ToArray());

            Preferences.Default.Get("Settings", setting);
        }

        private static bool IsSettingsSaved()
        {
            string value = Preferences.Default.Get("Settings", String.Empty);
            return String.IsNullOrEmpty(value);

        }
    }
}
