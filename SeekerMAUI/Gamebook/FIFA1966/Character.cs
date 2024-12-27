using System;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public Dictionary<string, int> Vars { get; set; }

        public override void Init()
        {
            base.Init();

            Vars = new Dictionary<string, int>();

            foreach (var team in Constants.Teams)
                Vars[$"силы соперников/{team.Key}"] = team.Value;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
        };

        public override string Save()
        {
            string vars = String.Empty;

            foreach (var pair in Vars)
            {
                vars += $"{pair.Key}:{pair.Value};";
            }

            return vars.TrimEnd(';');
        }

        public override void Load(string saveLine)
        {
            Vars = new Dictionary<string, int>();

            foreach (var vars in saveLine.Split(';'))
            {
                var pair = vars.Split(':');
                Vars.Add(pair[0], int.Parse(pair[1]));
            }

            IsProtagonist = true;
        }

        public override string Debug()
        {
            string propertiesList = String.Empty;

            foreach (var pair in Vars)
            {
                propertiesList += $"{pair.Key} = {pair.Value}\n";
            }

            return propertiesList;
        }
    }
}
