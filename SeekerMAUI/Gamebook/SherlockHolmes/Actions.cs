using System;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Stat { get; set; }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Dexterity}",
            $"Изобретательность: {Character.Protagonist.Ingenuity}",
            $"Интуиция: {Character.Protagonist.Intuition}",
            $"Красноречие: {Character.Protagonist.Eloquence}",
            $"Наблюдательность: {Character.Protagonist.Observation}",
            $"Эрудиция: {Character.Protagonist.Erudition}",
        };

        public override List<string> Representer()
        {
            if (!String.IsNullOrEmpty(Stat))
            {
                int currentStat = GetProperty(Character.Protagonist, Stat);
                string diffLine = String.Empty;

                if (currentStat > 0)
                {
                    string count = Game.Services.CoinsNoun(currentStat, "единица", "единицы", "единицы");
                    diffLine = $"\n+{currentStat} {count}";
                }

                return new List<string> { $"{Head}{diffLine}" };
            }
            else if (!String.IsNullOrEmpty(Head))
            {
                return new List<string> { Head };
            }
            else
            {
                return new List<string> { };
            }
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (!String.IsNullOrEmpty(Stat))
            {
                int stat = GetProperty(Character.Protagonist, Stat);

                if (secondButton)
                    return (stat > 0);
                else
                    return (Character.Protagonist.StatBonuses > 0);
            }
            else
            {
                return true;
            }
        }

        public List<string> Get() =>
            ChangeProtagonistParam(Stat, Character.Protagonist, "StatBonuses");

        public List<string> Decrease() =>
            ChangeProtagonistParam(Stat, Character.Protagonist, "StatBonuses", decrease: true);
    }
}
