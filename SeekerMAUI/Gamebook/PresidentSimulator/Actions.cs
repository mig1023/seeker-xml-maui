using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Android.Net.Http;
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

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains("|"))
            {
                return option.Split('|').Where(x => Game.Option.IsTriggered(x.Trim())).Count() > 0;
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
                foreach (string line in option.Split(','))
                {
                    if (Game.Services.AvailabilityByСomparison(line))
                    {
                        var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                            line, Constants.Availabilities, onlyFailTrueReturn: true);

                        if (fail)
                            return false;
                    }
                    else if (line == "СИЛЫ ВОЙСК И ПОВСТАНЦЕВ РАВНЫ")
                    {
                        return Character.Protagonist.Army == Character.Protagonist.Rebels;
                    }
                    else if (line == "СИЛЫ ВОЙСК НЕ МЕНЬШЕ СИЛЫ ПОВСТАНЦЕВ")
                    {
                        return Character.Protagonist.Army >= Character.Protagonist.Rebels;
                    }
                    else if (line == "СИЛЫ ВОЙСК МЕНЬШЕ СИЛЫ ПОВСТАНЦЕВ")
                    {
                        return Character.Protagonist.Army < Character.Protagonist.Rebels;
                    }
                    else if (line == "СИЛЫ ВОЙСК БОЛЬШЕ СИЛЫ ПОВСТАНЦЕВ")
                    {
                        int level = Services.LevelParse(line);
                        return Character.Protagonist.Army > Character.Protagonist.Rebels + level;
                    }
                    else if (line.Contains("!"))
                    {
                        if (Option.IsTriggered(line.Replace("!", String.Empty).Trim()))
                            return false;
                    }
                    else if (!Option.IsTriggered(line.Trim()))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
