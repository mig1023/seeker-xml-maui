using System;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        private int _weapons;
        public int Weapons
        {
            get => _weapons;
            set => _weapons = Game.Param.Setter(value, _weapons, this);
        }

        private int _shields;
        public int MaxShields { get; set; }
        public int Shields
        {
            get => _shields;
            set => _shields = Game.Param.Setter(value, max: MaxShields, _shields, this);
        }

        private int _luck;
        public int Luck
        {
            get => _luck;
            set => _luck = Game.Param.Setter(value, _luck, this);
        }

        private int _skill;
        public int Skill
        {
            get => _skill;
            set => _skill = Game.Param.Setter(value, _skill, this);
        }

        private int _stamina;
        public int MaxStamina { get; set; }
        public int Stamina
        {
            get => _stamina;
            set => _stamina = Game.Param.Setter(value, max: MaxStamina, _stamina, this);
        }

        public static Character Captain { get; set; }
        public static Character ScienseOfficer { get; set; }
        public static Character MedicalOfficer { get; set; }
        public static Character EngineeringOfficer { get; set; }
        public static Character SecurityOfficer { get; set; }
        public static Character SecurityGuard1 { get; set; }
        public static Character SecurityGuard2 { get; set; }

        private Character Crew()
        {
            Skill = Game.Dice.Roll() + 6;
            MaxStamina = Game.Dice.Roll() + 12;
            Stamina = MaxStamina;

            return this;
        }

        public override void Init()
        {
            base.Init();

            Weapons = Game.Dice.Roll() + 6;
            MaxShields = Game.Dice.Roll() + 12;
            Shields = MaxShields;
            Luck = Game.Dice.Roll() + 6;

            Captain = new Character().Crew();
            ScienseOfficer = new Character().Crew();
            MedicalOfficer = new Character().Crew();
            EngineeringOfficer = new Character().Crew();
            SecurityOfficer = new Character().Crew();
            SecurityGuard1 = new Character().Crew();
            SecurityGuard2 = new Character().Crew();
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,
            Weapons = this.Weapons,
            MaxShields = this.MaxShields,
            Shields = this.Shields,
            Luck = this.Luck,
        };

        private string SaveCrew(Character crew) =>
            $"{crew.Skill}:{crew.MaxStamina}:{crew.Stamina}";



        public override string Save() => String.Join("|",
            Weapons, MaxShields, Shields, Luck,
            SaveCrew(Captain), SaveCrew(ScienseOfficer), SaveCrew(MedicalOfficer),
            SaveCrew(EngineeringOfficer), SaveCrew(SecurityOfficer),
            SaveCrew(SecurityGuard1), SaveCrew(SecurityGuard2)
        );

        private Character LoadCrew(string loadLine)
        {
            var load = loadLine.Split(':');

            return new Character
            {
                Skill = int.Parse(load[0]),
                MaxStamina = int.Parse(load[1]),
                Stamina = int.Parse(load[2]),
            };
        }

        public override void Load(string saveLine)
        {
            var save = saveLine.Split('|');

            Weapons = int.Parse(save[0]);
            MaxShields = int.Parse(save[1]);
            Shields = int.Parse(save[2]);
            Luck = int.Parse(save[3]);

            Captain = LoadCrew(save[4]);
            ScienseOfficer = LoadCrew(save[5]);
            MedicalOfficer = LoadCrew(save[6]);
            EngineeringOfficer = LoadCrew(save[7]);
            SecurityOfficer = LoadCrew(save[8]);
            SecurityGuard1 = LoadCrew(save[9]);
            SecurityGuard2 = LoadCrew(save[10]);

            IsProtagonist = true;
        }
    }
}
