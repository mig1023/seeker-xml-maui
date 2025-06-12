using SeekerMAUI.Gamebook.CreatureOfHavoc;
using System;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public bool SpaceCombat { get; set; }
        public bool HandToHandCombat { get; set; }
        public bool BlasterCombat { get; set; }

        public string Crew { get; set; }
        public int Max { get; set; }

        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Вооружение: {Character.Protagonist.Weapons}",
            $"Защита: {Character.Protagonist.Shields}/{Character.Protagonist.MaxShields}",
            $"Удача: {Character.Protagonist.Luck}"
        };

        public override List<string> AdditionalStatus()
        {
            var statusLines = new List<string>();

            foreach (var team in Constants.Team)
            {
                var crew = Character.Team[team];
                var name = Constants.Names[team];
                string isAlive = crew.Stamina > 0 ? string.Empty : "CROSSEDOUT|";
                statusLines.Add($"{isAlive}{name}");
            }

            return statusLines;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Shields, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemies == null)
                return enemies;

            if (SpaceCombat)
            {
                var enemy = Enemies.First();
                enemies.Add($"{enemy.Name}\nвооружение {enemy.Weapons}  защита {enemy.Shields}");
            }

            return enemies;
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (Type == "Select")
            {
                var count = Character.Team.Where(x => x.Value.Selected).Count();
                var isAlive = Character.Team[Crew].Stamina > 0;

                return isAlive && (count < Max);
            }
            else
            {
                return true;
            }
        }

        public List<string> Select()
        {
            Character.Team[Crew].Selected = true;

            return new List<string> { "RELOAD" };
        }

        public List<string> Fight()
        {
            if (SpaceCombat)
            {
                return Fights.SpaceCombat(this, Enemies);
            }
            else if (HandToHandCombat)
            {
                return Fights.HandToHandCombat(this, Enemies);
            }
            else
            {
                return Fights.BlasterCombat(this, Enemies);
            }
        }
    }
}
