using System;

namespace SeekerMAUI.Gamebook.Usurper
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() => new List<string>
        {
            $"Влияние: {Character.Protagonist.Influence}",
            $"Здоровье: {Character.Protagonist.Health}",
            $"Лояльность: {Character.Protagonist.Loyalty}",
        };

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Стабильность: {Character.Protagonist.Stability}",
            $"Милосердие: {Character.Protagonist.Mercy}",
            $"Деспотия: {Character.Protagonist.Despotism}",
        };

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities);
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public List<string> Random()
        {
            List<string> random = new List<string>();

            random.Add($"BIG|Получить новое случайное знание (возможно совпадение с уже полученным):");

            var dice = Game.Dice.Roll(size: 9);

            random.Add($"Случайное число: {dice}");

            var randomSkill = Constants.Random[dice];

            Game.Option.Trigger(randomSkill);

            random.Add($"BIG|BOLD|Вы получаете талант: {randomSkill.ToUpper()}");

            random.Add($"GRAY|...Фантастическое пространство схлопнулось быстрее, чем человек успел моргнуть. " +
                $"Вы приходите в себя, после чего берёте с собой корону и покидаете Молдспайр.");

            return random;
        }
    }
}
