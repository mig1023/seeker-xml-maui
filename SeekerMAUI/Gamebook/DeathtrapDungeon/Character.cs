using System;

namespace SeekerMAUI.Gamebook.DeathtrapDungeon
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _skill;
        public int MaxSkill { get; set; }
        public int Skill
        {
            get => _skill;
            set => _skill = Game.Param.Setter(value, max: MaxSkill, _skill, this);
        }

        private int _strength;
        public int MaxStrength { get; set; }
        public int Strength
        {
            get => _strength;
            set => _strength = Game.Param.Setter(value, max: MaxStrength, _strength, this);
        }

        private int _luck;
        public int MaxLuck { get; set; }
        public int Luck
        {
            get => _luck;
            set => _luck = Game.Param.Setter(value, max: MaxLuck, _luck, this);
        }

        public override void Init()
        {
            base.Init();

            int dice = Game.Dice.Roll(dices: 2);

            MaxSkill = Game.Dice.Roll() + 6;
            Skill = MaxSkill;
            MaxStrength = Game.Dice.Roll(dices: 2) + 12;
            Strength = MaxStrength;
            MaxLuck = Game.Dice.Roll() + 6;
            Luck = MaxLuck;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            MaxSkill = this.MaxSkill,
            Skill = this.Skill,
            MaxStrength = this.MaxStrength,
            Strength = this.Strength,
            MaxLuck = this.MaxLuck,
            Luck = this.Luck,
        };

        public override string Save() => String.Join("|",
            MaxSkill, Skill, MaxStrength, Strength, MaxLuck, Luck);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            MaxSkill = int.Parse(save[0]);
            Skill = int.Parse(save[1]);
            MaxStrength = int.Parse(save[2]);
            Strength = int.Parse(save[3]);
            MaxLuck = int.Parse(save[4]);
            Luck = int.Parse(save[5]);

            IsProtagonist = true;
        }
    }
}
