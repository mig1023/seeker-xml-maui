using System;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public bool SpaceCombat { get; set; }
        public bool HandToHandCombat { get; set; }
        public bool BlasterCombat { get; set; }

        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Вооружение: {Character.Protagonist.Weapons}",
            $"Защита: {Character.Protagonist.Shields}/{Character.Protagonist.MaxShields}",
            $"Удача: {Character.Protagonist.Luck}"
        };

        private void AddStatusLine(ref List<string> statuses, string name, Character crew)
        {
            string isAlive = crew.Stamina > 0 ? string.Empty : "CROSSEDOUT|";
            statuses.Add($"{isAlive}{name}");
        }

        public override List<string> AdditionalStatus()
        {
            var statusLines = new List<string>();

            AddStatusLine(ref statusLines, "Капитан", Character.Captain);
            AddStatusLine(ref statusLines, "Оф.по науке", Character.ScienseOfficer);
            AddStatusLine(ref statusLines, "Врач", Character.MedicalOfficer);
            AddStatusLine(ref statusLines, "Инженер", Character.EngineeringOfficer);
            AddStatusLine(ref statusLines, "Нач.без", Character.SecurityOfficer);
            AddStatusLine(ref statusLines, "Солдат", Character.SecurityGuard1);
            AddStatusLine(ref statusLines, "Солдат", Character.SecurityGuard2);

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
