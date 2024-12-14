using System;

namespace SeekerMAUI.Gamebook.BehindTheThrone
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public int Agility { get; set; }

        public int Marksmanship { get; set; }

        public int Swashbuckling { get; set; }

        private int _vitality;
        public int Vitality
        {
            get => _vitality;
            set => _vitality = Game.Param.Setter(value, _vitality, this);
        }

        public override void Init()
        {
            base.Init();

            Agility = Game.Dice.Roll() + 3;
            Marksmanship = Game.Dice.Roll() + 3;
            Swashbuckling = Game.Dice.Roll() + 3;
            Vitality = Game.Dice.Roll() + 10;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Agility = this.Agility,
            Marksmanship = this.Marksmanship,
            Swashbuckling = this.Swashbuckling,
            Vitality = this.Vitality,
        };

        public override string Save() => String.Join("|",
            Agility, Marksmanship, Swashbuckling, Vitality);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Agility = int.Parse(save[0]);
            Marksmanship = int.Parse(save[1]);
            Swashbuckling = int.Parse(save[2]);
            Vitality = int.Parse(save[3]);

            IsProtagonist = true;
        }
    }
}
