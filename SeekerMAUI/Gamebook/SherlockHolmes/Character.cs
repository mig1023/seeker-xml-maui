using System;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _dexterity;
        public int Dexterity
        {
            get => _dexterity;
            set => _dexterity = Game.Param.Setter(value, _dexterity, this);
        }

        private int _ingenuity;
        public int Ingenuity
        {
            get => _ingenuity;
            set => _ingenuity = Game.Param.Setter(value, _ingenuity, this);
        }

        private int _intuition;
        public int Intuition
        {
            get => _intuition;
            set => _intuition = Game.Param.Setter(value, _intuition, this);
        }

        private int _eloquence;
        public int Eloquence
        {
            get => _eloquence;
            set => _eloquence = Game.Param.Setter(value, _eloquence, this);
        }

        private int _observation;
        public int Observation
        {
            get => _observation;
            set => _observation = Game.Param.Setter(value, _observation, this);
        }

        private int _erudition;
        public int Erudition
        {
            get => _erudition;
            set => _erudition = Game.Param.Setter(value, _erudition, this);
        }

        public int StatBonuses { get; set; }

        public override void Init()
        {
            base.Init();

            Dexterity = 0;
            Ingenuity = 0;
            Intuition = 0;
            Eloquence = 0;
            Observation = 0;
            Erudition = 0;
            StatBonuses = 0;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Dexterity = this.Dexterity,
            Ingenuity = this.Ingenuity,
            Intuition = this.Intuition,
            Eloquence = this.Eloquence,
            Observation = this.Observation,
            Erudition = this.Erudition,
            StatBonuses = this.StatBonuses,
        };

        public override string Save() => String.Join("|",
            Dexterity, Ingenuity, Intuition, Eloquence, Observation, Erudition, StatBonuses);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Dexterity = int.Parse(save[0]);
            Ingenuity = int.Parse(save[1]);
            Intuition = int.Parse(save[2]);
            Eloquence = int.Parse(save[3]);
            Observation = int.Parse(save[4]);
            Erudition = int.Parse(save[5]);
            StatBonuses = int.Parse(save[6]);

            IsProtagonist = true;
        }
    }
}
