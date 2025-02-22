using SeekerMAUI.Game;
using System;
using System.Xml.Linq;

namespace SeekerMAUI.Gamebook.Tachanka
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Names { get; set; }
        public bool OnlyWound { get; set; }

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
                var wounded = crew.Wounded ? $" (ранен{female})" : string.Empty;
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
            if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities);
            }

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

        public List<string> Damage()
        {
            var lines = new List<string> { $"BIG|Проверка ущерба тачанке:" };

            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            var dice = firstDice + secondDice;

            lines.Add($"На кубиках выпало: {Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} = {dice}");

            Damage(dice, ref lines);

            Game.Buttons.Disable("Fight, Flee");

            return lines;
        }

        private void Damage(int dice, ref List<string> lines)
        {
            switch (dice)
            {
                case 2:
                    lines.Add("BIG|BAD|Ущерб нанесён выносливости коней!");
                    Character.Protagonist.HorseEndurance -= 1;
                    lines.Add($"BOLD|Теперь их выносливость равна {Character.Protagonist.HorseEndurance}");
                    return;
                case 3:
                    Wound(0, ref lines);
                    return;
                case 4:
                    lines.Add("BIG|BAD|Ущерб нанесён колёсам!");
                    Character.Protagonist.Wheels -= 1;
                    lines.Add($"BOLD|Теперь прочность колёс равна {Character.Protagonist.Wheels}");
                    return;
                case 5:
                case 10:
                    lines.Add("BIG|BAD|Ущерб нанесён коляске!");
                    Character.Protagonist.Carriage -= 1;
                    lines.Add($"BOLD|Теперь прочность коляски равна {Character.Protagonist.Carriage}");
                    return;
                case 6:
                    lines.Add("BIG|BAD|Ущерб нанесён упряжи!");
                    Character.Protagonist.Harness -= 1;
                    lines.Add($"BOLD|Теперь прочность упряжи равна {Character.Protagonist.Harness}");
                    return;
                case 9:
                    Wound(2, ref lines);
                    return;
                case 11:
                    Wound(1, ref lines);
                    return;
                case 12:
                    lines.Add("BIG|BAD|Ущерб нанесён рессорам!");
                    Character.Protagonist.Springs -= 1;
                    lines.Add($"BOLD|Теперь прочность рессор равна {Character.Protagonist.Springs}");
                    return;
                default:
                    lines.Add("BIG|GOOD|Обошлось!");
                    lines.Add("BOLD|Никакого ущерба и никто не ранен!");
                    return;
            }
        }

        private void Wound(int index, ref List<string> lines)
        {
            if (Character.Protagonist.Team.Count < index + 1)
            {
                lines.Add("BIG|GOOD|Обошлось!");
                lines.Add("BOLD|Пуля просвистела через место недостающего члена отряда!");
                return;
            }

            var crew = Character.Protagonist.Team[index];
            var female = crew.Name == "Варя" ? "а" : string.Empty;

            if (crew.Wounded)
            {
                lines.Add($"BIG|BAD|Случайной пулей убит{female} {crew.Name}!");
                lines.Add("BOLD|В вашем отряде стало на одного бойца меньше...");

                Character.Protagonist.Team.Remove(crew);
            }
            else
            {
                lines.Add($"BIG|BAD|Случайной пулей ранен{female} {crew.Name}!");
                crew.Wounded = true;
            }
        }

        public List<string> Dead()
        {
            var lines = new List<string>();

            var names = Names
                .Split(',')
                .Select(x => x.Trim());

            foreach (var name in names)
            {
                var inList = -1;

                for (var i = 0; i < Character.Protagonist.Team.Count; i++)
                {
                    if (Character.Protagonist.Team[i].Name == name)
                        inList = i;
                }

                if (inList >= 0)
                {
                    var female = name == "Варя" ? "а" : string.Empty;
                    var already = Character.Protagonist.Team[inList].Wounded;

                    if (!OnlyWound || already)
                    {
                        lines.Add($"BOLD|BIG|BAD|Случайной пулей убит{female} {name}!");

                        Character.Protagonist.Team.RemoveAt(inList);
                    }
                    else
                    {
                        lines.Add($"BOLD|BIG|BAD|Случайной пулей ранен{female} {name}!");

                        Character.Protagonist.Team[inList].Wounded = true;
                    }
                    
                    return lines;
                }
                else
                {
                    lines.Add($"GOOD|{name} не в отряде.");
                }
            }

            lines.Add($"BOLD|BIG|GOOD|Никого не задело! Вот это везение! :)");

            return lines;
        }
    }
}
