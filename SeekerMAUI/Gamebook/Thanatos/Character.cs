using System;

namespace SeekerMAUI.Gamebook.Thanatos
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _cycle;
        public int Cycle
        {
            get => _cycle;
            set => _cycle = Game.Param.Setter(value, _cycle, this);
        }

        public override void Init()
        {
            base.Init();

            Cycle = 0;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Cycle = this.Cycle,
        };

        public override string Save() => Cycle.ToString();

        public override void Load(string saveLine)
        {
            Cycle = int.Parse(saveLine);
            IsProtagonist = true;
        }
    }
}
