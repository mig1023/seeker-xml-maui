using System;

namespace SeekerMAUI.Gamebook.KoshcheisChain
{
    class Dices
    {
        public static List<string> Games(Actions actions)
        {
            var diceGame = new List<string>();

            var dice = Game.Dice.Roll();

            diceGame.Add($"BIG|Вы выбросили: {Game.Dice.Symbol(dice)}");

            if (dice >= 4)
            {
                Game.Buttons.Disable("Fail");
                return actions.Win(diceGame, withoutSpace: true);
            }
            else
            {
                Game.Buttons.Disable("Win");
                return actions.Fail(diceGame, withoutSpace: true);
            }
        }

        public static List<string> Path(bool ringEffect)
        {
            var dice = Octagon.Roll();

            if (ringEffect && (dice == 7))
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

        public static List<string> Gambling()
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
    }
}
