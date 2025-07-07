using System;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Dice
    {
        private static List<string> Line(List<string> lines, string text, string tag)
        {
            lines.Add($"BIG|BOLD|{text}!");
            Game.Buttons.Disable(tag);

            return lines;
        }

        public static List<string> Result(List<string> lines, bool first,
            string firstText, string firstTag, string secondText, string secondTag)
        {
            return first ? Line(lines, firstText, firstTag) : Line(lines, secondText, secondTag);
        }
    }
}
