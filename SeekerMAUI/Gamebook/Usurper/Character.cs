using System;

namespace SeekerMAUI.Gamebook.Usurper
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _influence;
        public int Influence
        {
            get => _influence;
            set => _influence = Game.Param.Setter(value, _influence, this);
        }

        private int _health;
        public int Health
        {
            get => _health;
            set => _health = Game.Param.Setter(value, _health, this);
        }
        
        private int _loyalty;
        public int Loyalty
        {
            get => _loyalty;
            set => _loyalty = Game.Param.Setter(value, _loyalty, this);
        }

        private int _stability;
        public int Stability
        {
            get => _stability;
            set => _stability = Game.Param.Setter(value, _stability, this);
        }

        public override void Init()
        {
            base.Init();

            Influence = 4;
            Health = 7;
            Loyalty = 0;
            Stability = 0;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Influence = this.Influence,
            Health = this.Health,
            Loyalty = this.Loyalty,
            Stability = this.Stability,
        };

        public override string Save() => String.Join("|",
            Name, Influence, Health, Loyalty, Stability);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Name = save[0];
            Influence = int.Parse(save[1]);
            Health = int.Parse(save[2]);
            Loyalty = int.Parse(save[3]);
            Stability = int.Parse(save[4]);

            IsProtagonist = true;
        }
    }
}
