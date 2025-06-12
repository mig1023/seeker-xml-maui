using System;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public static Dictionary<string, Character> Team { get; set; }

        private int _weapons;
        public int Weapons
        {
            get => _weapons;
            set => _weapons = Game.Param.Setter(value, _weapons, this);
        }

        private int _shields;
        public int MaxShields { get; set; }
        public int Shields
        {
            get => _shields;
            set => _shields = Game.Param.Setter(value, max: MaxShields, _shields, this);
        }

        private int _luck;
        public int Luck
        {
            get => _luck;
            set => _luck = Game.Param.Setter(value, _luck, this);
        }

        private int _skill;
        public int Skill
        {
            get => _skill;
            set => _skill = Game.Param.Setter(value, _skill, this);
        }

        private int _stamina;
        public int MaxStamina { get; set; }
        public int Stamina
        {
            get => _stamina;
            set => _stamina = Game.Param.Setter(value, max: MaxStamina, _stamina, this);
        }

        public bool Selected { get; set; }

        private Character Crew()
        {
            Skill = Game.Dice.Roll() + 6;
            MaxStamina = Game.Dice.Roll() + 12;
            Stamina = MaxStamina;
            Selected = false;

            return this;
        }

        public override void Init()
        {
            base.Init();

            Weapons = Game.Dice.Roll() + 6;
            MaxShields = Game.Dice.Roll() + 12;
            Shields = MaxShields;
            Luck = Game.Dice.Roll() + 6;

            Team = new Dictionary<string, Character>();

            foreach (var name in Constants.Team)
            {
                Team.Add(name, new Character().Crew());
            }
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Weapons = this.Weapons,
            MaxShields = this.MaxShields,
            Shields = this.Shields,
            Luck = this.Luck,
        };

        private string SaveTeam()
        {
            var line = string.Empty;

            foreach (var team in Constants.Team)
            {
                var crew = Team[team];
                var selected = crew.Selected ? "1" : "0";
                line += $"{crew.Skill}:{crew.MaxStamina}:{crew.Stamina}:{selected};";
            }

            return line.Remove(line.Length - 1);
        }
        public override string Save() => String.Join("|",
            Weapons, MaxShields, Shields, Luck, SaveTeam()
        );

        private Character LoadCrew(string loadLine)
        {
            var load = loadLine.Split(':');

            return new Character
            {
                Skill = int.Parse(load[0]),
                MaxStamina = int.Parse(load[1]),
                Stamina = int.Parse(load[2]),
                Selected = load[2] == "1"
            };
        }

        public override void Load(string saveLine)
        {
            var save = saveLine.Split('|');

            Weapons = int.Parse(save[0]);
            MaxShields = int.Parse(save[1]);
            Shields = int.Parse(save[2]);
            Luck = int.Parse(save[3]);

            Team = new Dictionary<string, Character>();

            var index = 0;

            foreach (var team in save[4].Split(";"))
            {
                var name = Constants.Team[index];
                Team.Add(name, LoadCrew(team));
                index += 1;
            }

            IsProtagonist = true;
        }
    }
}
