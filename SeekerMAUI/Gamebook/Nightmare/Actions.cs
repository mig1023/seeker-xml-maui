using System.Collections.Generic;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.Nightmare
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        private void CoinResult(ref List<string> lines, string text, List<string> buttons)
        {
            lines.Add($"BIG|BOLD|{text}!");
            Buttons.Disable(string.Join(",", buttons));
        }

        public List<string> Coin()
        {
            List<string> lines = new List<string> { "Бросаем монетку:" };

            bool coin = Dice.Roll() % 2 == 0;

            if (coin)
            {
                lines.Add("BIG|BOLD|GOOD|Выпал ОРЁЛ!");
                Buttons.Disable("Tail");
            }
            else
            {
                lines.Add("BIG|BOLD|BAD|Выпал РЕШКА!");
                Buttons.Disable("Head");
            }

            return lines;
        }
    }
}
