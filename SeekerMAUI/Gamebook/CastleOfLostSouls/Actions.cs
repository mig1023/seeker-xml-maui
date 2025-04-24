using System;

namespace SeekerMAUI.Gamebook.CastleOfLostSouls
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Честь: {Character.Protagonist.Honor}",
            $"Доспехи: {Character.Protagonist.Armour}",
            $"Золото: {Character.Protagonist.Gold}",
        };

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Боевая доблесть: {Character.Protagonist.Combat}",
            $"Телосложение: {Character.Protagonist.Constitution}",
            $"Сообразительность: {Character.Protagonist.Ingenuity}",
            $"Магическая стойкость: {Character.Protagonist.Resistence}",
        };

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
            {
                string line = $"{enemy.Name}\nБоевая доблесть " +
                    $"{enemy.Combat}  Телосложение {enemy.Constitution}";

                if (enemy.Armour > 0)
                    line += $"  Доспехи {enemy.Armour}";

                if (enemy.Ingenuity > 0)
                    line += $"  Сообразительность {enemy.Ingenuity}";

                enemies.Add(line);
            }

            return enemies;
        }
    }
}
