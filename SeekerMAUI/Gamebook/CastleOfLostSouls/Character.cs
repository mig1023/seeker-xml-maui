using System;

namespace SeekerMAUI.Gamebook.CastleOfLostSouls
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _combat;
        public int Combat
        {
            get => _combat;
            set => _combat = Game.Param.Setter(value);
        }

        private int _constitution;
        public int Constitution
        {
            get => _constitution;
            set => _constitution = Game.Param.Setter(value);
        }

        private int _ingenuity;
        public int Ingenuity
        {
            get => _ingenuity;
            set => _ingenuity = Game.Param.Setter(value);
        }

        private int _resistence;
        public int Resistence
        {
            get => _resistence;
            set => _resistence = Game.Param.Setter(value);
        }

        private int _honor;
        public int Honor
        {
            get => _honor;
            set => _honor = Game.Param.Setter(value);
        }

        private int _armour;
        public int Armour
        {
            get => _armour;
            set => _armour = Game.Param.Setter(value);
        }

        private int _gold;
        public int Gold
        {
            get => _gold;
            set => _gold = Game.Param.Setter(value, _gold, this);
        }

        public override void Init()
        {
            base.Init();

            Combat = Game.Dice.Roll() + 5;
            Constitution = Game.Dice.Roll(dices: 2) + 10;
            Ingenuity = Game.Dice.Roll() + 5;
            Honor = 3;
            Armour = 2;
            Gold = 10;

            Resistence = Game.Dice.Roll(dices: 2) + 3;

            if (Combat > 6)
                Resistence += 1;

            if (Constitution < 15)
                Resistence += 1;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Combat = this.Combat,
            Constitution = this.Constitution,
            Ingenuity = this.Ingenuity,
            Honor = this.Honor,
            Armour = this.Armour,
            Gold = this.Gold,
            Resistence = this.Resistence,
        };

        public override string Save() => String.Join("|",
            Combat, Constitution, Ingenuity, Honor, Armour, Gold, Resistence
        );

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Combat = int.Parse(save[0]);
            Constitution = int.Parse(save[1]);
            Ingenuity = int.Parse(save[2]);
            Honor = int.Parse(save[3]);
            Armour = int.Parse(save[4]);
            Gold = int.Parse(save[5]);
            Resistence = int.Parse(save[6]);

            IsProtagonist = true;
        }
    }
}
