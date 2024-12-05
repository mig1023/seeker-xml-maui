using System;

namespace SeekerMAUI.Gamebook.Trap
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string EnemyName = String.Empty;
        public int EnemyAttack = 0;

        public override List<string> Status() => new List<string>
        {
            $"Сила: {Character.Protagonist.Strength}",
            $"Ловкость: {Character.Protagonist.Skill}",
            $"Обаяние: {Character.Protagonist.Charm}",
        };

        public override List<string> AdditionalStatus()
        {
            var karm = Character.Protagonist.Karma < 0 ? "минус " : String.Empty;
            var money = Character.Protagonist.Gold;
            var gold = Game.Services.CoinsNoun(money, "ой", "ых", "ых");

            return new List<string>
            {
                $"Здоровье: {Character.Protagonist.Hitpoints}/100",
                $"Карма: {karm}{Character.Protagonist.Karma}",
                $"Деньги: {Character.Protagonist.Gold} золот{gold}",
            };
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Hitpoints, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            if (String.IsNullOrEmpty(EnemyName))
            {
                return new List<string>();
            }
            else
            {
                return new List<string> { $"{EnemyName.ToUpper()}\nАтака {EnemyAttack}" };
            }
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities);
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public List<string> Fight()
        {
            var fight = new List<string>();
            var heroAttack = Character.Protagonist.Strength;

            fight.Add($"ВАША АТАКА: Сила = {heroAttack}");

            foreach (string equip in Character.Protagonist.Equipment)
            {
                string[] data = equip.Split(",");
                fight.Add($"GRAY|+{data[1]} за {data[0]}");
                heroAttack += int.Parse(data[1]);
            }

            fight.Add($"ИТОГО: Атака = {heroAttack}\n");
            fight.Add($"{EnemyName.ToUpper()}: Атака = {EnemyAttack}\n");

            if (heroAttack > EnemyAttack)
            {
                fight.Add($"BOLD|GOOD|Ваша атака выше, чем у противника!");
                fight.Add($"Вы побеждаете без потери здоровья!");
            }
            else if (heroAttack == EnemyAttack)
            {
                fight.Add($"BOLD|Ваши атаки с противником равны!");
                fight.Add($"BAD|Вы побеждаете, потеряв 25 баллов здоровья!");

                Character.Protagonist.Hitpoints -= 25;
            }
            else if ((heroAttack + 1) == EnemyAttack)
            {
                fight.Add($"BOLD|Ваша атака на единицу меньше, чем у противника!");
                fight.Add($"BAD|Вы побеждаете, потеряв 75 баллов здоровья!");

                Character.Protagonist.Hitpoints -= 75;
            }
            else
            {
                fight.Add($"BOLD|BAD|Ваша атака более чем на единицу меньше, чем у противника!");
                fight.Add($"BAD|Вы погибаете в этом бою...");

                Character.Protagonist.Hitpoints = 0;
            }

            return fight;
        }
    }
}
