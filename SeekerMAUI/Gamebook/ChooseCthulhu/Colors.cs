using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.ChooseCthulhu
{
    class Colors
    {
        private static int Sub(int color, int value)
        {
            if (color >= value)
            {
                return color - value;
            }
            else
            {
                return 0;
            }
        }

        public static List<int> Mod(List<int> color)
        {
            color[0] = Sub(color[0], 7);
            color[1] = Sub(color[1], 6);
            color[2] = Sub(color[2], 6);

            return color;
        }

        public static string Hex(int r, int g, int b)
        {
            System.Drawing.Color myColor = System.Drawing.Color.FromArgb(r, g, b);
            return myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
        }

        public static string СontrastBorder(List<int> color, List<int> button)
        {
            if ((color == null) || (button == null))
            {
                return string.Empty;
            }
            else if (color[0] <= 24)
            {
                return Constants.CONTRAST_BORDER_DEFAULT;
            }
            else
            {
                return Hex(button[0], button[1], button[2]);
            }
        }

        public static string СontrastText(List<int> color)
        {
            int minDarkness = Constants.IsSecondPart() ? 124 : 66;

            if ((color == null) || (color[0] > minDarkness))
            {
                return Constants.CONTRAST_TEXT_DEFAULT;
            }
            else if (Constants.IsSecondPart())
            {
                return Constants.CONTRAST_TEXT_LIGHT_SECOND;
            }
            else
            {
                return Constants.CONTRAST_TEXT_LIGHT_FIRST;
            }
        }
    }
}
