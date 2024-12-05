using SeekerMAUI.Game;
using System;

namespace SeekerMAUI.Gamebook.Trap
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _strength;
        public int Strength
        {
            get => _strength;
            set => _strength = Game.Param.Setter(value, _strength, this);
        }

        private int _skill;
        public int Skill
        {
            get => _skill;
            set => _skill = Game.Param.Setter(value, _skill, this);
        }

        private int _charm;
        public int Charm
        {
            get => _charm;
            set => _charm = Game.Param.Setter(value, _charm, this);
        }

        private int _gold;
        public int Gold
        {
            get => _gold;
            set => _gold = Game.Param.Setter(value, _gold, this);
        }

        private int _hitpoints;
        public int Hitpoints
        {
            get => _hitpoints;
            set => _hitpoints = Game.Param.Setter(value, max: 100, _hitpoints, this);
        }

        public int Karma { get; set; }

        public List<string> Equipment { get; set; }

        public override void Init()
        {
            base.Init();

            Strength = 8;
            Skill = 8;
            Charm = 8;
            Hitpoints = 100;
            Karma = 0;
            Gold = 0;

            Equipment = new List<string>();
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Strength = this.Strength,
            Skill = this.Skill,
            Charm = this.Charm,
            Hitpoints = this.Hitpoints,
            Karma = this.Karma,
            Gold = this.Gold,
        };

        public override string Save() => String.Join("|",
            Strength, Skill, Charm, Hitpoints, Karma, Gold, String.Join(";", Equipment));

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Strength = int.Parse(save[0]);
            Skill = int.Parse(save[1]);
            Charm = int.Parse(save[2]);
            Hitpoints = int.Parse(save[3]);
            Karma = int.Parse(save[4]);
            Gold = int.Parse(save[5]);

            Equipment = save[6].Split(';').ToList();

            IsProtagonist = true;
        }
    }
}
