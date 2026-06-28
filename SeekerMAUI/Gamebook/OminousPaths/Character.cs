using System;

namespace SeekerMAUI.Gamebook.OminousPaths
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _skill;
        public int Skill
        {
            get => _skill;
            set => _skill = Game.Param.Setter(value, _skill, this);
        }

        private int _strength;
        public int Strength
        {
            get => _strength;
            set => _strength = Game.Param.Setter(value, _strength, this);
        }

        private int _charm;
        public int Charm
        {
            get => _charm;
            set
            {
                if (value < 1)
                    _charm = 1;
                else
                    _charm = value;
            }
        }

        public List<bool> Luck { get; set; }

        public override void Init()
        {
            base.Init();

            Skill = Constants.Skills[Game.Dice.Roll() - 1];
            Strength = Constants.Strengths[Game.Dice.Roll() - 1];
            Charm = Constants.Charms[Game.Dice.Roll() - 1];

            Luck = new List<bool> { false, true, true, true, true, true, true };

            for (int i = 0; i < 2; i++)
                Luck[Game.Dice.Roll()] = false;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Skill = this.Skill,
            Strength = this.Strength,
            Charm = this.Charm,
        };

        public override string Save() => String.Join("|",
            Skill, Strength, Charm,
            String.Join(",", Luck.Select(x => x ? "1" : "0")));

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Skill = int.Parse(save[0]);
            Strength = int.Parse(save[1]);
            Charm = int.Parse(save[2]);

            Luck = save[3].Split(',').Select(x => x == "1").ToList();

            IsProtagonist = true;
        }
    }
}
