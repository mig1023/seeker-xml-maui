﻿using System;

namespace SeekerMAUI.Gamebook.Tremble
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

        private int _endurance;
        public int MaxEndurance { get; set; }
        public int Endurance
        {
            get => _endurance;
            set => _endurance = Game.Param.Setter(value, max: MaxEndurance, _endurance, this);
        }

        private int _luck;
        public int MaxLuck { get; set; }
        public int Luck
        {
            get => _luck;
            set => _luck = Game.Param.Setter(value, max: MaxLuck, _luck, this);
        }

        public string Keys { get; set; }

        public override void Init()
        {
            base.Init();

            MaxSkill = Game.Dice.Roll() + 6;
            Skill = MaxSkill;
            MaxEndurance = Game.Dice.Roll(dices: 2) + 12;
            Endurance = MaxEndurance;
            MaxLuck = Game.Dice.Roll() + 6;
            Luck = MaxLuck;
            Keys = String.Empty;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            MaxSkill = this.MaxSkill,
            Skill = this.Skill,
            MaxEndurance = this.MaxEndurance,
            Endurance = this.Endurance,
            MaxLuck = this.MaxLuck,
            Luck = this.Luck,
            Keys = this.Keys,
        };

        public override string Save() => String.Join("|",
            MaxSkill, Skill, MaxEndurance, Endurance, MaxLuck, Luck, Keys);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            MaxSkill = int.Parse(save[0]);
            Skill = int.Parse(save[1]);
            MaxEndurance = int.Parse(save[2]);
            Endurance = int.Parse(save[3]);
            MaxLuck = int.Parse(save[4]);
            Luck = int.Parse(save[5]);
            Keys = save[6];

            IsProtagonist = true;
        }
    }
}
