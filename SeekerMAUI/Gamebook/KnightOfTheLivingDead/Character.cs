using System;

namespace SeekerMAUI.Gamebook.KnightOfTheLivingDead
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public string Weapon { get; set; }

        public int Attack { get; set; }

        public int Damage { get; set; }

        private int _hitpoints;
        public int Hitpoints
        {
            get => _hitpoints;
            set => _hitpoints = Game.Param.Setter(value, _hitpoints, this);
        }

        public override void Init()
        {
            base.Init();

            Weapon = string.Empty;
            Attack = 0;
            Damage = 0;
            Hitpoints = 1;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Weapon = this.Weapon,
            Attack = this.Attack,
            Damage = this.Damage,
            Hitpoints = this.Hitpoints,
        };

        public override string Save() => String.Join("|",
            Weapon, Attack, Damage, Hitpoints);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Weapon = save[0];
            Attack = int.Parse(save[1]);
            Damage = int.Parse(save[2]);
            Hitpoints = int.Parse(save[3]);

            IsProtagonist = true;
        }
    }
}
