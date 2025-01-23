using System;

namespace SeekerMAUI.Gamebook.LoneWolf
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
        public int MaxStrength { get; set; }
        public int Strength
        {
            get => _strength;
            set => _strength = Game.Param.Setter(value, max: MaxStrength, _strength, this);
        }

        private int _gold;
        public int Gold
        {
            get => _gold;
            set => _gold = Game.Param.Setter(value, _gold, this);
        }

        public override void Init()
        {
            base.Init();

            Name = "Главный герой";
            Skill = Game.Dice.Roll(size: 10) + 9;
            MaxStrength = Game.Dice.Roll(size: 10) + 19;
            Strength = MaxStrength;
            Gold = 0;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Skill = this.Skill,
            MaxStrength = this.MaxStrength,
            Strength = this.Strength,
            Gold = this.Gold,
        };

        public override string Save() => String.Join("|",
            Skill, MaxStrength, Strength, Gold);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Skill = int.Parse(save[0]);
            MaxStrength = int.Parse(save[1]);
            Strength = int.Parse(save[2]);
            Gold = int.Parse(save[3]);

            IsProtagonist = true;
        }
    }
}
