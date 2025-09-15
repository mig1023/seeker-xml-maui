using System;

namespace SeekerMAUI.Gamebook.OctopusIsland
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public static Character Serge { get; set; }
        public static Character Xolotl { get; set; }
        public static Character Thibaut { get; set; }
        public static Character Souhi { get; set; }

        public static List<Character> Team { get; set; }

        public static Character CurrentWarrior { get; set; }

        private int _food;
        public int Food
        {
            get => (StolenStuffs == 0 ? _food : 0);
            set => _food = Game.Param.Setter(value, _food, this);
        }

        private int _lifeGivingOintment;
        public int LifeGivingOintment
        {
            get => (StolenStuffs == 0 ? _lifeGivingOintment : 0);
            set => _lifeGivingOintment = Game.Param.Setter(value, _lifeGivingOintment, this);
        }
        public int StolenStuffs { get; set; }

        private int _hitpoint;
        public int Hitpoint
        {
            get => _hitpoint;
            set => _hitpoint = Game.Param.Setter(value, max: 20, _hitpoint, this);
        }

        public int Skill { get; set; }

        public bool GameOver { get; set; }

        public override void Init()
        {
            base.Init();

            Thibaut = new Character
            {
                Name = "Тибо",
                Skill = Game.Dice.Roll() + 6,
                Hitpoint = 20,
            };

            Serge = new Character
            {
                Name = "Серж",
                Skill = Thibaut.Skill - 1,
                Hitpoint = 20,
            };

            Xolotl = new Character
            {
                Name = "Ксолотл",
                Skill = Serge.Skill,
                Hitpoint = 20,
            };

            Souhi = new Character
            {
                Name = "Суи",
                Skill = Xolotl.Skill,
                Hitpoint = 20,
            };

            Team = new List<Character> { Serge, Xolotl, Thibaut, Souhi };

            Food = 4;
            LifeGivingOintment = 40;
            StolenStuffs = 0;

            GameOver = false;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,

            Name = this.Name,
            Hitpoint = this.Hitpoint,
            Skill = this.Skill,
            Food = this.Food,
            LifeGivingOintment = this.LifeGivingOintment,
            StolenStuffs = this.StolenStuffs,
            GameOver = this.GameOver,
        };

        public override string Save() => String.Join("|",
            Thibaut.Hitpoint, Thibaut.Skill, Serge.Hitpoint, Serge.Skill, Xolotl.Hitpoint,
            Xolotl.Skill, Souhi.Hitpoint, Souhi.Skill, Food, LifeGivingOintment, StolenStuffs,
            GameOver ? "1" : "0");

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Thibaut = new Character
            {
                Name = "Тибо",
                Skill = int.Parse(save[0]),
                Hitpoint = int.Parse(save[1]),
            };

            Serge = new Character
            {
                Name = "Серж",
                Skill = int.Parse(save[2]),
                Hitpoint = int.Parse(save[3]),
            };

            Xolotl = new Character
            {
                Name = "Ксолотл",
                Skill = int.Parse(save[4]),
                Hitpoint = int.Parse(save[5]),
            };

            Souhi = new Character
            {
                Name = "Суи",
                Skill = int.Parse(save[6]),
                Hitpoint = int.Parse(save[7]),
            };

            Team = new List<Character> { Serge, Xolotl, Thibaut, Souhi };

            Food = int.Parse(save[8]);
            LifeGivingOintment = int.Parse(save[9]);
            StolenStuffs = int.Parse(save[10]);
            GameOver = save[11] == "1";

            IsProtagonist = true;
        }
    }
}
