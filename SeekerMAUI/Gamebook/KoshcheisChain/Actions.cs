using SeekerMAUI.Gamebook.CreatureOfHavoc;
using System;

namespace SeekerMAUI.Gamebook.KoshcheisChain
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string EnemyName = String.Empty;
        public int EnemyStrngth = 0;

        public List<Fight> Fights { get; set; } 

        public override List<string> AdditionalStatus()
        {
            var money = Character.Protagonist.Money;
            var line = Game.Services.CoinsNoun(money, "ейка", "ейки", "еек");

            return new List<string>
            {
                $"Сила: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
                $"Экстрасенсорика: {Character.Protagonist.Extrasensory}",
                $"Ловкость: {Character.Protagonist.Skill}",
                $"Богатство: {money} коп{line}",
            };
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            var enemy = new List<string>();

            if (String.IsNullOrEmpty(EnemyName))
            {
                return new List<string>();
            }
            else
            {
                return new List<string> { $"{EnemyName}\nсила {EnemyStrngth}" };
            }
        }

        private string ActColors(Fight fight)
        {
            if (!String.IsNullOrEmpty(fight.Hero) && fight.Hero != "win")
            {
                return "BAD|";
            }
            else
            {
                return "GOOD|";
            }
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");
                fight.Add($"{EnemyName} (сила {EnemyStrngth})");

                Octagon.DoubleRoll(out int firstDice, out int secondDice);

                int dices = Octagon.RingValueFuse(firstDice) +
                    Octagon.RingValueFuse(secondDice);

                fight.Add($"Бросок кубика: {Octagon.Symbol(firstDice)} + " +
                    $"{Octagon.Symbol(secondDice)} = {dices}");

                var ring = Fights
                    .Where(x => x.Min == "ring")
                    .FirstOrDefault();

                if ((ring != null) && ((firstDice == 7) || (secondDice == 7)))
                {
                    fight.Add($"GOOD|BOLD|{ring.Act}");

                    if (ring.Hero == "restore")
                    {
                        Character.Protagonist.Strength = Character.Protagonist.MaxStrength;
                    }
                    else if (ring.Hero == "win")
                    {
                        fight.Add(String.Empty);
                        fight.Add("BIG|GOOD|Вы ПОБЕДИЛИ :)");
                        return fight;
                    }
                }
                else
                {
                    var act = Fights
                       .Where(x => (Int32.TryParse(x.Min, out int min) ? min : 13) <= dices)
                       .Where(x => (Int32.TryParse(x.Max, out int max) ? max : 0) >= dices)
                       .FirstOrDefault();

                    var color = ActColors(act);
                    fight.Add(color + act.Act);

                    if (act.Hero == "win")
                    {
                        fight.Add(String.Empty);
                        fight.Add("BIG|GOOD|Вы ПОБЕДИЛИ :)");
                        return fight;
                    }
                    else if (act.Hero == "dead")
                    {
                        Character.Protagonist.Strength = 0;

                        fight.Add(String.Empty);
                        fight.Add("BIG|BAD|Вы ПРОИГРАЛИ :(");
                        return fight;
                    }
                    else if (!String.IsNullOrEmpty(act.Hero))
                    {
                        var bonus = int.Parse(act.Hero);
                        Character.Protagonist.Strength += bonus;

                        if (Character.Protagonist.Strength <= 0)
                        {
                            fight.Add(String.Empty);
                            fight.Add("BIG|BAD|Вы ПРОИГРАЛИ :(");
                            return fight;
                        }
                    }
                    else if (!String.IsNullOrEmpty(act.Enemy))
                    {
                        var bonus = int.Parse(act.Enemy);
                        EnemyStrngth += bonus;

                        if (EnemyStrngth <= 0)
                        {
                            fight.Add(String.Empty);
                            fight.Add("BIG|GOOD|Вы ПОБЕДИЛИ :)");
                            return fight;
                        }
                    }
                }

                round += 1;
                fight.Add(String.Empty);
            }
        }
    }
}
