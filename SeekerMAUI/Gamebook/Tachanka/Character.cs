using System;
using System.Linq;

namespace SeekerMAUI.Gamebook.Tachanka
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public List<Crew> Team { get; set; }

        private int _horseEndurance;
        public int HorseEndurance
        {
            get => _horseEndurance;
            set => _horseEndurance = Game.Param.Setter(value, max: 10, _horseEndurance, this);
        }

        private int _wheels;
        public int Wheels
        {
            get => _wheels;
            set => _wheels = Game.Param.Setter(value, max: 5, _wheels, this);
        }

        private int _carriage;
        public int Carriage
        {
            get => _carriage;
            set => _carriage = Game.Param.Setter(value, max: 5, _carriage, this);
        }

        private int _harness;
        public int Harness
        {
            get => _harness;
            set => _harness = Game.Param.Setter(value, max: 5, _harness, this);
        }

        private int _springs;
        public int Springs
        {
            get => _springs;
            set => _springs = Game.Param.Setter(value, max: 5, _springs, this);
        }

        private int _time;
        public int Time
        {
            get => _time;
            set => _time = Game.Param.Setter(value, _time, this);
        }

        private int _cartridges;
        public int Cartridges
        {
            get => _cartridges;
            set => _cartridges = Game.Param.Setter(value, _cartridges, this);
        }
        
        private int _grenades;
        public int Grenades
        {
            get => _grenades;
            set => _grenades = Game.Param.Setter(value, _grenades, this);
        }
        
        private int _money;
        public int Money
        {
            get => _money;
            set => _money = Game.Param.Setter(value, _money, this);
        }

        private int _food;
        public int Food
        {
            get => _food;
            set => _food = Game.Param.Setter(value, _food, this);
        }

        private int _medicines;
        public int Medicines
        {
            get => _medicines;
            set => _medicines = Game.Param.Setter(value, _medicines, this);
        }

        public override void Init()
        {
            base.Init();

            Team = new List<Crew>();

            HorseEndurance = 10;
            Wheels = 5;
            Carriage = 5;
            Harness = 5;
            Springs = 5;
            Time = 28;
            Cartridges = 500;
            Grenades = 2;
            Money = 20;
            Food = 5;
            Medicines = 2;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            HorseEndurance = this.HorseEndurance,
            Wheels = this.Wheels,
            Carriage = this.Carriage,
            Harness = this.Harness,
            Springs = this.Springs,
            Time = this.Time,
            Cartridges = this.Cartridges,
            Grenades = this.Grenades,
            Money = this.Money,
            Food = this.Food,
            Medicines = this.Medicines,
        };

        public override string Save()
        {
            var team = String.Join("%", Team.Select(x => x.Serialize()));

            return String.Join("|", team, HorseEndurance, Wheels, Carriage, Harness,
                Springs, Time, Cartridges, Grenades, Money, Food, Medicines);
        }

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Team = new List<Crew>();

            foreach (string crew in save[0].Split('%'))
                Team.Add(Crew.Deserialize(crew));

            HorseEndurance = int.Parse(save[1]);
            Wheels = int.Parse(save[2]);
            Carriage = int.Parse(save[3]);
            Harness = int.Parse(save[4]);
            Springs = int.Parse(save[5]);
            Time = int.Parse(save[6]);
            Cartridges = int.Parse(save[7]);
            Grenades = int.Parse(save[8]);
            Money = int.Parse(save[9]);
            Food = int.Parse(save[10]);
            Medicines = int.Parse(save[11]);
            IsProtagonist = true;
        }
    }
}
