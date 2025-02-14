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
                string wounded = crew.Wounded ? "(ранен)" : string.Empty;
                status.Add($"{crew.Name}{wounded}");
            }

            return status;
        }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Кони: {Character.Protagonist.HorseEndurance}/10",
            $"Тачанка: {Character.Protagonist.Wheels}/5 " +
                $"{Character.Protagonist.Carriage}/5 " +
                $"{Character.Protagonist.Harness}/5 " +
                $"{Character.Protagonist.Springs}/5",
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

        public List<string> Crew()
        {
            return new List<string> { "OK!" };
        }
    }
}
