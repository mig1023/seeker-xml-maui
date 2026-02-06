using SeekerMAUI.Gamebook.CreatureOfHavoc;
using System;

namespace SeekerMAUI.Gamebook.NoYourGrace
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;

        public int Ivo { get; set; }
        public int Army { get; set; }
        public int Nobles { get; set; }
        public int Traders { get; set; }
        public int Church { get; set; }

        public int Week { get; set; }
        public int Gold { get; set; }
        public int Davern { get; set; }
        public int Debt { get; set; }
        public int FlorentinisDebt { get; set; }
        public int Accusation { get; set; }

        public int Vitality { get; set; }
        public int Dexterity { get; set; }
        public int Sorcery { get; set; }
        public int Ingredients { get; set; }
        public int VictoryPoints { get; set; }

        public int Income { get; set; }
        public int Research { get; set; }
        public int Romance { get; set; }
        public int Random { get; set; }
        public int Dice { get; set; }
        public int Fatigue { get; set; }
        public int LectureFatigue { get; set; }
        public int Label { get; set; }
        public int Unit { get; set; }
        public int Feats { get; set; }

        public int Charge { get; set; }
        public int Fent { get; set; }
        public int Lunge { get; set; }
        public int Attack { get; set; }
        public int Movement { get; set; }
        public int Defense { get; set; }

        public int LorsuliaDefense { get; set; }
        public int LorsuliaActions { get; set; }

        public int LoyalistForces { get; set; }
        public int RebelForces { get; set; }

        public int EnemyDefense { get; set; }
        public int EnemyFeint { get; set; }
        public int EnemyHit { get; set; }
        public int LorsuliaHitpoints { get; set; }
        public int EnemyHitpoints { get; set; }
        public int EnemyActions { get; set; }
        public int EnemyAgility { get; set; }


        public override void Init()
        {
            base.Init();

            Ivo = 30;
            Army = 30;
            Nobles = 30;
            Traders = 30;
            Church = 30;

            Week = 1;
            Gold = 5;
            Davern = 30;
            Debt = 0;
            FlorentinisDebt = 0;
            Accusation = 0;

            Vitality = 0;
            Dexterity = 1;
            Sorcery = 1;
            Ingredients = 0;
            VictoryPoints = 0;

            Income = 100;
            Research = 0;
            Romance = 0;
            Random = 0;
            Dice = 6;
            Fatigue = 2;
            LectureFatigue = 2;
            Label = 0;
            Unit = 1;
            Feats = 0;

            Charge = 0;
            Fent = 0;
            Lunge = 0;
            Attack = 0;
            Movement = 0;
            Defense = 0;

            LorsuliaDefense = 0;
            LorsuliaActions = 0;

            LoyalistForces = 0;
            RebelForces = 0;

            EnemyDefense = 0;
            EnemyFeint = 1;
            EnemyHit = 0;
            LorsuliaHitpoints = 1;
            EnemyHitpoints = 1;
            EnemyActions = 0;
            EnemyAgility = 1;
        }

        public Character Clone() => new Character()
        {
            IsProtagonist = this.IsProtagonist,
            Name = this.Name,

            Ivo = this.Ivo,
            Army = this.Army,
            Nobles = this.Nobles,
            Traders = this.Traders,
            Church = this.Church,

            Week = this.Week,
            Gold = this.Gold,
            Davern = this.Davern,
            Debt = this.Debt,
            FlorentinisDebt = this.FlorentinisDebt,
            Accusation = this.Accusation,

            Vitality = this.Vitality,
            Dexterity = this.Dexterity,
            Sorcery = this.Sorcery,
            Ingredients = this.Ingredients,
            VictoryPoints = this.VictoryPoints,

            Income = this.Income,
            Research = this.Research,
            Romance = this.Romance,
            Random = this.Random,
            Dice = this.Dice,
            Fatigue = this.Fatigue,
            LectureFatigue = this.LectureFatigue,
            Label = this.Label,
            Unit = this.Unit,
            Feats = this.Feats,

            Charge = this.Charge,
            Fent = this.Fent,
            Lunge = this.Lunge,
            Attack = this.Attack,
            Movement = this.Movement,
            Defense = this.Defense,

            LorsuliaDefense = this.LorsuliaDefense,
            LorsuliaActions = this.LorsuliaActions,

            LoyalistForces = this.LoyalistForces,
            RebelForces = this.RebelForces,

            EnemyDefense = this.EnemyDefense,
            EnemyFeint = this.EnemyFeint,
            EnemyHit = this.EnemyHit,
            LorsuliaHitpoints = this.LorsuliaHitpoints,
            EnemyHitpoints = this.EnemyHitpoints,
            EnemyActions = this.EnemyActions,
            EnemyAgility = this.EnemyAgility,
        };

        public override string Save() => String.Join("|",
            Name,
            Ivo, Army, Nobles, Traders, Church,
            Week, Gold, Davern, Debt, FlorentinisDebt, Accusation,
            Vitality, Dexterity, Sorcery, Ingredients, VictoryPoints,
            Income, Research, Romance, Random, Dice, Fatigue, LectureFatigue, Label, Unit, Feats,
            Charge, Fent, Lunge, Attack, Movement, Defense,
            LorsuliaDefense, LorsuliaActions,
            LoyalistForces, RebelForces,
            EnemyDefense, EnemyFeint, EnemyHit, LorsuliaHitpoints, EnemyHitpoints, EnemyActions, EnemyAgility);

        public override void Load(string saveLine)
        {
            string[] save = saveLine.Split('|');

            Name = save[0];

            Ivo = int.Parse(save[1]);
            Army = int.Parse(save[2]);
            Nobles = int.Parse(save[3]);
            Traders = int.Parse(save[4]);
            Church = int.Parse(save[5]);

            Week = int.Parse(save[6]);
            Gold = int.Parse(save[7]);
            Davern = int.Parse(save[8]);
            Debt = int.Parse(save[9]);
            FlorentinisDebt = int.Parse(save[10]);
            Accusation = int.Parse(save[11]);

            Vitality = int.Parse(save[12]);
            Dexterity = int.Parse(save[13]);
            Sorcery = int.Parse(save[14]);
            Ingredients = int.Parse(save[15]);
            VictoryPoints = int.Parse(save[16]);

            Income = int.Parse(save[17]);
            Research = int.Parse(save[18]);
            Romance = int.Parse(save[19]);
            Random = int.Parse(save[20]);
            Dice = int.Parse(save[21]);
            Fatigue = int.Parse(save[22]);
            LectureFatigue = int.Parse(save[23]);
            Label = int.Parse(save[24]);
            Unit = int.Parse(save[25]);
            Feats = int.Parse(save[26]);

            Charge = int.Parse(save[27]);
            Fent = int.Parse(save[28]);
            Lunge = int.Parse(save[29]);
            Attack = int.Parse(save[30]);
            Movement = int.Parse(save[31]);
            Defense = int.Parse(save[32]);

            LorsuliaDefense = int.Parse(save[33]);
            LorsuliaActions = int.Parse(save[34]);

            LoyalistForces = int.Parse(save[35]);
            RebelForces = int.Parse(save[36]);

            EnemyDefense = int.Parse(save[37]);
            EnemyFeint = int.Parse(save[38]);
            EnemyHit = int.Parse(save[39]);
            LorsuliaHitpoints = int.Parse(save[40]);
            EnemyHitpoints = int.Parse(save[41]);
            EnemyActions = int.Parse(save[42]);
            EnemyAgility = int.Parse(save[43]);

            IsProtagonist = true;
        }
    }
}
