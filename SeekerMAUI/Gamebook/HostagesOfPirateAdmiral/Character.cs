﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.HostagesOfPirateAdmiral
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

        private int _charm;
        public int Charm
        {
            get => _charm;
            set
            {
                if (value < 2)
                    _charm = 2;
                else
                    _charm = value;
            }
        }

        private int _blaster;
        public int Blaster
        {
            get => _blaster;
            set => _blaster = Game.Param.Setter(value, _blaster, this);
        }

        public List<bool> Luck { get; set; }
        public int Coins { get; set; }

        public override void Init()
        {
            base.Init();

            int dice = Game.Dice.Roll(dices: 2);

            MaxSkill = Constants.Skills[dice];
            Skill = MaxSkill;
            MaxStrength = Constants.Strengths[dice];
            Strength = MaxStrength;
            Charm = Constants.Charms[dice];

            Blaster = 1;
            Coins = 0;

            Luck = new List<bool> { false, true, true, true, true, true, true };

            for (int i = 0; i < 2; i++)
                Luck[Game.Dice.Roll()] = false;

            Game.Healing.Add(name: "Попить", healing: 2, portions: 2);
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            MaxSkill = this.MaxSkill,
            Skill = this.Skill,
            MaxStrength = this.MaxStrength,
            Strength = this.Strength,
            Charm = this.Charm,
            Blaster = this.Blaster,
            Coins = this.Coins,
        };

        public override string Save() => String.Join("|",
            MaxSkill, Skill, MaxStrength, Strength, Charm, Blaster, Coins,
            String.Join(",", Luck.Select(x => x ? "1" : "0")));

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            MaxSkill = int.Parse(save[0]);
            Skill = int.Parse(save[1]);
            MaxStrength = int.Parse(save[2]);
            Strength = int.Parse(save[3]);
            Charm = int.Parse(save[4]);
            Blaster = int.Parse(save[5]);
            Coins = int.Parse(save[6]);

            Luck = save[7].Split(',').Select(x => x == "1").ToList();

            IsProtagonist = true;
        }
    }
}
