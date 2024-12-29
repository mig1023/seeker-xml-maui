using System;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public Vars Vars { get; set; }

        public string Enemy { get; set; }

        public override void Init()
        {
            base.Init();

            Vars = new Vars();

            foreach (var team in Constants.Teams)
                Vars[$"силы соперников/{team.Key}"] = team.Value;

            Enemy = String.Empty;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Enemy = this.Enemy,
        };

        public override string Save()
        {
            string vars = String.Empty;

            foreach (var key in Vars.Keys())
            {
                vars += $"{key}:{Vars[key]};";
            }

            return Enemy + "|" + vars.TrimEnd(';');
        }

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Enemy = save[0];
            Vars = new Vars();

            foreach (var vars in save[1].Split(';'))
            {
                var pair = vars.Split(':');
                Vars[pair[0]] = int.Parse(pair[1]);
            }

            IsProtagonist = true;
        }

        public override string Debug()
        {
            string propertiesList = String.Empty;

            foreach (var key in Vars.Keys())
            {
                propertiesList += $"{key} = {Vars[key]}\n";
            }

            return propertiesList;
        }
    }
}
