using System;

namespace SeekerMAUI.Gamebook.LandOfTallGrasses
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

        private int _luck;
        public int Luck
        {
            get => _luck;
            set => _luck = Game.Param.Setter(value, _luck, this);
        }

        public override void Init()
        {
            base.Init();

            Skill = Game.Dice.Roll() + 10;
            Strength = Game.Dice.Roll() + 10;
            Luck = Game.Dice.Roll();
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Skill = this.Skill,
            Strength = this.Strength,
            Luck = this.Luck,
        };

        public override string Save() => String.Join("|",
            Skill, Strength, Luck);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Skill = int.Parse(save[0]);
            Strength = int.Parse(save[1]);
            Luck = int.Parse(save[2]);

            IsProtagonist = true;
        }
    }
}
