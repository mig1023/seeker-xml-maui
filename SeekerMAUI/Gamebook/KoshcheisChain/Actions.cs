using System;
using System.Linq;

namespace SeekerMAUI.Gamebook.KoshcheisChain
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string EnemyName = String.Empty;
        public int EnemyStrength = 0;
        
        public List<Fight> Fights { get; set; }
        public bool ByExtrasensory { get; set; }
        public bool RingEffect { get; set; }
        public bool Forward { get; set; }
        public int StrengthLimit { get; set; }

        public override List<string> AdditionalStatus()
        {
            var money = Character.Protagonist.Money;
            var line = Game.Services.CoinsNoun(money, "ейка", "ейки", "еек");

            var statuses = new List<string>
            {
                $"Сила: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
                $"Экстрасенсорика: {Character.Protagonist.Extrasensory}",
                $"Ловкость: {Character.Protagonist.Skill}",
                $"Богатство: {money} коп{line}",
            };

            if (Character.Protagonist.Staff > 0)
            {
                var staff = Character.Protagonist.Staff;
                statuses.Add($"Жезл: {staff} стрел");
            }

            return statuses;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            if (Price > 0)
            {
                var line = Game.Services.CoinsNoun(Price, "ейка", "ейки", "еек");
                return new List<string> { $"{Head}\n{Price} коп{line}" };
            }
            else if (String.IsNullOrEmpty(EnemyName))
            {
                return new List<string>();
            }
            else
            {
                return new List<string> { $"{EnemyName}\nсила {EnemyStrength}" };
            }
        }

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

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains("Два зелья быстроты"))
            {
                var count = Game.Data.Triggers
                    .Where(x => x == "Зелье быстроты")
                    .Count();

                return count >= 2;
            }
            else if (option.Contains("Флейта и верёвка"))
            {
                var flute = Game.Option.IsTriggered("Флейта");
                var rope = Game.Option.IsTriggered("Верёвка");

                if (option.Contains("!"))
                {
                    return !flute || !rope;
                }
                else
                {
                    return flute && rope;
                }
            }
            else
            {
                return base.Availability(option);
            }
        }

        public override bool IsButtonEnabled(bool secondButton = false) =>
            !Used && !((Price > 0) && (Character.Protagonist.Money < Price));

        private string ActColors(Fight fight)
        {
            if (!String.IsNullOrEmpty(fight.Hero) && fight.Hero != "win")
            {
                return "BOLD|BAD|";
            }
            else
            {
                return "BOLD|GOOD|";
            }
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            int round = 1;

            while (true)
            {
                if (!Forward)
                {
                    fight.Add($"HEAD|BOLD|Раунд: {round}");
                    fight.Add($"{EnemyName} (сила {EnemyStrength})");
                }

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
                        return Win(fight);
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
                        return Win(fight);
                    }
                    else if (act.Hero == "dead")
                    {
                        Character.Protagonist.Strength = 0;
                        return Forward ? fight : Fail(fight);
                    }
                    else if (!String.IsNullOrEmpty(act.Hero))
                    {
                        var bonus = int.Parse(act.Hero);
                        Character.Protagonist.Strength += bonus;

                        var limit = StrengthLimit > 0 ? StrengthLimit : 0;

                        if (!Forward && Character.Protagonist.Strength <= limit)
                            return Fail(fight);
                    }
                    else if (!String.IsNullOrEmpty(act.Enemy))
                    {
                        var bonus = int.Parse(act.Enemy);
                        EnemyStrength += bonus;

                        if (EnemyStrength <= 0)
                            return Win(fight);
                    }
                }

                if (Forward)
                {
                    return fight;
                }

                round += 1;
                fight.Add(String.Empty);
            }
        }

        public List<string> Test()
        {
            Octagon.DoubleRoll(out int firstDice, out int secondDice);

            var level = ByExtrasensory ? Character.Protagonist.Extrasensory : Character.Protagonist.Skill;
            var passed = (firstDice + secondDice) <= level;
            var passedLine = (passed ? "<= " : "> ") + level.ToString();
            var levelType = ByExtrasensory ? "экстрасенсорных способностей " : "ловкости ";
            var ring = (firstDice == 7) || (secondDice == 7);

            if (RingEffect && ring)
            {
                passed = true;
                passedLine = "- выпало волшебное кольцо!";
                Game.Buttons.Disable("WinWithoutRing");
            }

            var test = new List<string> {
                $"Проверка {levelType}: " +
                $"{Octagon.Symbol(firstDice)} + " +
                $"{Octagon.Symbol(secondDice)} " +
                $"{passedLine}" };

            if (passed)
            {
                test.Add("BIG|GOOD|УСПЕХ :)");
                Game.Buttons.Disable("Fail");

                if (RingEffect && !ring)
                    Game.Buttons.Disable("WinWithRing");
            }
            else
            {
                test.Add("BIG|BAD|НЕУДАЧА :(");
                Game.Buttons.Disable("Win");

                if (RingEffect)
                    Game.Buttons.Disable("WinWithRing, WinWithoutRing");
            }

            return test;
        }

        public List<string> GameOfDice()
        {
            var diceGame = new List<string>();

            var dice = Game.Dice.Roll();

            diceGame.Add($"BIG|Вы выбросили: {Game.Dice.Symbol(dice)}");

            if (dice >= 4)
            {
                Game.Buttons.Disable("Fail");
                return Win(diceGame, withoutSpace: true);
            }
            else
            {
                Game.Buttons.Disable("Win");
                return Fail(diceGame, withoutSpace: true);
            }
        }

        public List<string> Gambling()
        {
            Octagon.DoubleRoll(out int firstDice, out int secondDice);
            var dices = firstDice + secondDice;
            var ring = (firstDice == 7) || (secondDice == 7);
            var dicesLine = ring ? String.Empty : $" = {dices}";

            var game = new List<string> { 
                "Вы сделали ставку в 2 копейки",
                $"BIG|Ваш бросок: {Octagon.Symbol(firstDice)} + " +
                $"{Octagon.Symbol(secondDice)}{dicesLine}" };

            if (ring)
            {
                game.Add("GOOD|Вам выпало волшебное кольцо!");
                dices = 12;
            }

            if (dices < 3)
            {
                game.Add("BAD|BOLD|Вас обвиняют в обмане.");
                Game.Buttons.Disable("Win, Fail, Again");
            }
            else if (dices < 7)
            {
                Character.Protagonist.Money -= 2;
                game.Add("BAD|BOLD|Вы проигрываете свою ставку в две копейки.");
                Game.Buttons.Disable("EpicFail, Win");
            }
            else if (dices < 12)
            {
                Character.Protagonist.Money += 2;
                game.Add("GOOD|BOLD|Вы выигрываете четыре серебряных копеечки (двойную ставку).");
                Game.Buttons.Disable("EpicFail, Fail");
            }
            else
            {
                Character.Protagonist.Money += 8;
                game.Add("GOOD|BOLD|Вы выигрываете кон из 10 серебряных копеек!");
                Game.Buttons.Disable("EpicFail, Fail");
            }

            return game;
        }

        public List<string> PathByDice()
        {
            var dice = Octagon.Roll();

            if (RingEffect && (dice == 7))
            {
                // nothing to do
            }
            else if (dice < 3)
            {
                Game.Buttons.Disable("Path3, Path5");
            }
            else if (dice < 5)
            {
                Game.Buttons.Disable("Path0, Path5");
            }
            else
            {
                Game.Buttons.Disable("Path0, Path3");
            }

            return new List<string> { $"BIG|Вы выбросили: {Octagon.Symbol(dice)}" };
        }

        public List<string> Get()
        {
            Character.Protagonist.Money -= Price;

            if (!Multiple)
                Used = true;

            return new List<string> { "RELOAD" };
        }
    }
}
