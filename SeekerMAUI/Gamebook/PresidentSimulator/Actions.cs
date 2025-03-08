using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Maui.Controls.Shapes;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.PresidentSimulator
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() => new List<string>
        {
            $"Год: {Character.Protagonist.Year}",
            $"Рейтинг: {Character.Protagonist.Rating}%",
            $"Монетки: {Character.Protagonist.Money}",
        };

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Лояльность бизнеса: {Character.Protagonist.BusinessLoyalty}",
            $"Лояльность армии: {Character.Protagonist.ArmyLoyalty}",
            $"Отношения с США: {Character.Protagonist.RelationWithUSA}",
            $"Отношения с СССР: {Character.Protagonist.RelationWithUSSR}",
        };

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                return !fail;
            }
            else if (option == "СИЛЫ ВОЙСК И ПОВСТАНЦЕВ РАВНЫ")
            {
                return Character.Protagonist.Army == Character.Protagonist.Rebels;
            }
            else if (option == "СИЛЫ ВОЙСК НЕ МЕНЬШЕ СИЛЫ ПОВСТАНЦЕВ")
            {
                return Character.Protagonist.Army >= Character.Protagonist.Rebels;
            }
            else if (option == "СИЛЫ ВОЙСК МЕНЬШЕ СИЛЫ ПОВСТАНЦЕВ")
            {
                return Character.Protagonist.Army < Character.Protagonist.Rebels;
            }
            else if (option == "СИЛЫ ВОЙСК БОЛЬШЕ СИЛЫ ПОВСТАНЦЕВ")
            {
                int level = Services.LevelParse(option);
                return Character.Protagonist.Army > Character.Protagonist.Rebels + level;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains(";"))
            {
                string[] options = option.Split(';');

                int optionMustBe = int.Parse(options[0]);
                int optionCount = options.Where(x => Game.Option.IsTriggered(x.Trim())).Count();

                return optionCount >= optionMustBe;
            }
            else
            {
                return base.Availability(option);
            }
        }
    }
}
