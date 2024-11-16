using System;

namespace SeekerMAUI.Gamebook.KoshcheisChain
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _strength;
        public int MaxStrength { get; set; }
        public int Strength
        {
            get => _strength;
            set => _strength = Game.Param.Setter(value, max: MaxStrength, _strength, this);
        }

        private int _extrasensory;
        public int Extrasensory
        {
            get => _extrasensory;
            set => _extrasensory = Game.Param.Setter(value, _extrasensory, this);
        }

        private int _skill;
        public int Skill
        {
            get => _skill;
            set => _skill = Game.Param.Setter(value, _skill, this);
        }

        private int _money;
        public int Money
        {
            get => _money;
            set => _money = Game.Param.Setter(value, _money, this);
        }

        public override void Init()
        {
            base.Init();

            MaxStrength = Octagon.RollValue(dices: 2) + 20;
            Strength = MaxStrength;
            Skill = Octagon.RollValue() + 4;
            Extrasensory = Octagon.RollValue(dices: 2) + 3;
            Money = 30;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            MaxStrength = this.MaxStrength,
            Strength = this.Strength,
            Skill = this.Skill,
            Extrasensory = this.Extrasensory,
            Money = this.Money,
        };

        public override string Save() => String.Join("|",
            MaxStrength, Strength, Skill, Extrasensory, Money);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            MaxStrength = int.Parse(save[0]);
            Strength = int.Parse(save[1]);
            Skill = int.Parse(save[2]);
            Extrasensory = int.Parse(save[3]);
            Money = int.Parse(save[4]);

            IsProtagonist = true;
        }
    }
}
