﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.ThreePaths
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public int? Time { get; set; }
        public List<string> Spells { get; set; }
        public int SpellSlots { get; set; }

        public override void Init()
        {
            base.Init();

            Time = null;
            SpellSlots = 9;
            Spells = new List<string>();
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Time = this.Time,
            Spells = new List<string>(),
        };

        public override string Save() => String.Join("|",
            Time, SpellSlots, String.Join(",", Spells).TrimEnd(','));

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Time = History.Continue.IntNullableParse(save[0]);
            SpellSlots = int.Parse(save[1]);
            Spells = save[2].Split(',').ToList();

            IsProtagonist = true;
        }
    }
}
