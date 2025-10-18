using System;

namespace SeekerMAUI.Gamebook.KnightOfTheLivingDead
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public string Memory { get; set; }

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

            Memory = string.Empty;
            Weapon = "Меч";
            Attack = 0;
            Damage = 0;
            Hitpoints = 1;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Memory = this.Memory,
            Name = this.Name,
            Weapon = this.Weapon,
            Attack = this.Attack,
            Damage = this.Damage,
            Hitpoints = this.Hitpoints,
        };

        public override string Save() => String.Join("|",
            Memory, Weapon, Attack, Damage, Hitpoints);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Memory = save[0];
            Weapon = save[1];
            Attack = int.Parse(save[2]);
            Damage = int.Parse(save[3]);
            Hitpoints = int.Parse(save[4]);

            IsProtagonist = true;
        }
    }
}
