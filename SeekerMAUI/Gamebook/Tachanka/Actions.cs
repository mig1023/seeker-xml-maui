using System;

namespace SeekerMAUI.Gamebook.Tachanka
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status()
        {
            if (Character.Protagonist.Team.Count <= 0)
            {
                return null;
            }

            var status = new List<string>();

            foreach (Crew crew in Character.Protagonist.Team)
            {
                var female = crew.Name == "Варя" ? "а" : string.Empty;
                var wounded = crew.Wounded ? $"(ранен{female})" : string.Empty;
                status.Add($"{crew.Name}{wounded}");
            }

            return status;
        }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Кони: {Character.Protagonist.HorseEndurance}/10",
            $"Тачанка: " +
                $"{Character.Protagonist.Wheels}/" +
                $"{Character.Protagonist.Carriage}/" +
                $"{Character.Protagonist.Harness}/" +
                $"{Character.Protagonist.Springs}",
            $"Время: {Character.Protagonist.Time}/28",
            $"Патроны: {Character.Protagonist.Cartridges}",
            $"Гранаты: {Character.Protagonist.Grenades}",
            $"Еда: {Character.Protagonist.Food}",
            $"Лекарства: {Character.Protagonist.Medicines}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText)
        {
            toEndParagraph = 0;

            bool byWheels = Character.Protagonist.Wheels <= 0;
            bool byCarriage = Character.Protagonist.Carriage <= 0;
            bool byHarness = Character.Protagonist.Harness <= 0;
            bool bySprings = Character.Protagonist.Springs <= 0;

            if (Character.Protagonist.HorseEndurance <= 0)
            {
                toEndText = "Вы загнали лошадей...";
            }
            else if (byWheels || byCarriage || byHarness || bySprings)
            {
                toEndText = "Тачанка разбита...";
            }
            else if (Character.Protagonist.Time <= 0)
            {
                toEndText = "Время вышло...";
            }
            else
            {
                toEndText = string.Empty;
                return false;
            }

            toEndText += " Придётся начать сначала...";
            return true;
        }

        private bool AvailabilityNode(string option)
        {
            var inTeam = Character.Protagonist.Team
                .Where(x => x.Name == option.Replace("!", String.Empty))
                .Count() > 0;

            var inSkills = Character.Protagonist.Team
                .Where(x => x.Skill.Contains(option.Replace("!", String.Empty)))
                .Count() > 0;

            var isTriggered = Game.Option.IsTriggered(option.Replace("!", String.Empty).Trim());

            if (option == "Есть место в тачанке")
            {
                return Character.Protagonist.Team.Count() < 3;
            }
            else if (option.Contains("!"))
            {
                return !inTeam && !inSkills && !isTriggered;
            }
            else
            {
                return inTeam || inSkills || isTriggered;
            }
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains(","))
            {
                var availability = option
                    .Split(',')
                    .Where(x => !AvailabilityNode(x.Trim()))
                    .Count() == 0;

                return availability;
            }
            else if (option.Contains("|"))
            {
                var availability = option
                   .Split('|')
                   .Where(x => AvailabilityNode(x.Trim()))
                   .Count() > 0;

                return availability;
            }
            else
            {
                return AvailabilityNode(option);
            }
        }
    }
}
