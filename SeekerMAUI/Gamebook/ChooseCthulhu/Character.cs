using System;

namespace SeekerMAUI.Gamebook.ChooseCthulhu
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _initiation;
        public int Initiation
        {
            get => _initiation;
            set
            {
                if (IsCursed() && (value < 1))
                    value = 1;

                _initiation = Game.Param.Setter(value, _initiation, this);
            }
        }

        public List<int> BackColor { get; set; }
        public List<int> BtnColor { get; set; }

        public override void Init()
        {
            base.Init();

            Initiation = IsCursed() ? 1 : 0;

            BackColor = null;
            BtnColor = null;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Initiation = this.Initiation,
        };

        public override string Save()
        {
            string backColor = String.Join(",", BackColor);
            string btnColor = String.Join(",", BtnColor);

            return String.Join("|", Initiation, backColor, btnColor);
        }

        public bool IsCursed() =>
            Preferences.Default.Get("ChooseCthulhu_Cursed", String.Empty) == "1";

        public void Cursed() => 
            Preferences.Default.Set("ChooseCthulhu_Cursed", "1");

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Initiation = int.Parse(save[0]);

            BackColor = save[1].Split(',').Select(x => int.Parse(x)).ToList();
            BtnColor = save[2].Split(',').Select(x => int.Parse(x)).ToList();

            IsProtagonist = true;
        }
    }
}
