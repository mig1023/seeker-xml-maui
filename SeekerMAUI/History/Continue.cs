using SeekerMAUI.Game;

namespace SeekerMAUI.History
{
    class Continue
    {
        public static void CurrentGame(string name)
        {
            Preferences.Default.Set("LastGame", name);
            Data.CurrentGamebook = name;
        }

        public static int? IntNullableParse(string line)
        {
            if (int.TryParse(line, out int value))
            {
                return value == -1 ? null : value;
            }
            else
            {
                return null;
            }
        }

        public static bool IsGameSaved()
        {
            string value = Preferences.Default.Get(Data.CurrentGamebook, string.Empty);
            return !string.IsNullOrEmpty(value);
        }

        public static void SaveCurrentGame() =>
            Save(Data.CurrentGamebook);

        public static void Save(string gameName)
        {
            string triggers = string.Join(",", Data.Triggers);
            string healing = Healing.Save();
            int paragraph = Data.CurrentParagraphID;
            string path = string.Join(",", Data.Path);
            string character = Data.Character.Save();

            Preferences.Default.Set(gameName,
                $"{paragraph}@{triggers}@{healing}@{character}@{path}");
        }

        public static int Load(string gameName)
        {
            if (string.IsNullOrEmpty(gameName))
                gameName = Data.CurrentGamebook;

            string saveLine = Preferences.Default.Get(gameName, string.Empty);

            string[] save = saveLine.Split('@');

            Data.CurrentParagraphID = int.Parse(save[0]);
            Data.Triggers = save[1].Split(',').ToList();

            Healing.Load(save[2]);
            Data.Character.Load(save[3]);
            Data.Path = save[4].Split(',').ToList();

            return Data.CurrentParagraphID;
        }

        public static void Remove() =>
            Preferences.Default.Remove(Data.CurrentGamebook);

        public static void Clean()
        {
            foreach (string gamebook in Gamebook.List.GetBooks())
                Preferences.Default.Remove(gamebook);

            foreach (string variable in Data.OuterGameVariable)
            {
                if (Preferences.Default.ContainsKey(variable))
                    Preferences.Default.Remove(variable);
            }
        }
    }
}
