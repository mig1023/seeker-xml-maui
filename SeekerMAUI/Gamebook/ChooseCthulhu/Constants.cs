using static SeekerMAUI.Output.Buttons;
using static SeekerMAUI.Game.Data;

namespace SeekerMAUI.Gamebook.ChooseCthulhu
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, string> Availabilities { get; set; }

        public static Dictionary<int, string> Buttons { get; set; }

        public static string CONTRAST_BORDER_DEFAULT = "#234249";
        public static string CONTRAST_TEXT_DEFAULT = "#082126";
        public static string CONTRAST_TEXT_LIGHT_FIRST = "#cfd9db";
        public static string CONTRAST_TEXT_LIGHT_SECOND = "#edd8a5";

        private static int FirstPartSize = 78;

        public static bool IsSecondPart() =>
            Game.Data.CurrentParagraphID > FirstPartSize;

        public static void ChangeBackground()
        {
            if (Character.Protagonist.BtnColor == null)
                return;

            Character.Protagonist.BackColor = Colors.Mod(Character.Protagonist.BackColor);
            Character.Protagonist.BtnColor = Colors.Mod(Character.Protagonist.BtnColor);
        }

        private List<int> ParseColors(string line)
        {
            var colors = line
                .Split(",")
                .Select(x => int.Parse(x))
                .ToList();

            return colors;
        }

        public override string GetColor(ButtonTypes type)
        {
            var mainButtons = (type == ButtonTypes.Main) || (type == ButtonTypes.Option);
            var supplButtons = (type == ButtonTypes.System) || (type == ButtonTypes.Continue);
            var start = Buttons.ContainsKey(Game.Data.CurrentParagraphID);

            if (Game.Settings.IsEnabled("WithoutStyles"))
            {
                return base.GetColor(type);
            }
            else if (mainButtons || supplButtons)
            {


                if (Character.Protagonist.BtnColor != null)
                {
                    var rColor = Character.Protagonist.BtnColor[0];
                    var gColor = Character.Protagonist.BtnColor[1];
                    var bColor = Character.Protagonist.BtnColor[2];

                    return Colors.Hex(rColor, gColor, bColor);
                }
                else
                {
                    return base.GetColor(type);
                }
            }
            else if (type == ButtonTypes.Border)
            {
                return Colors.СontrastBorder(Character.Protagonist.BackColor, Character.Protagonist.BtnColor);
            }
            else
            {
                return base.GetColor(type);
            }
        }

        public override string GetColor(ColorTypes type)
        {
            if (Game.Settings.IsEnabled("WithoutStyles"))
            {
                return base.GetColor(type);
            }
            else if ((type == ColorTypes.Background) && (Game.Data.CurrentParagraphID == 0))
            {
                return Output.Constants.COLOR_WHITE;
            }
            else if (type == ColorTypes.Background)
            {
                if (Buttons.ContainsKey(Game.Data.CurrentParagraphID) && (Character.Protagonist.BackColor == null))
                {
                    var colors = Buttons[Game.Data.CurrentParagraphID].Split(";");
                    Character.Protagonist.BackColor = ParseColors(colors[0]);
                    Character.Protagonist.BtnColor = ParseColors(colors[1]);
                }

                if (Character.Protagonist.BackColor != null)
                {
                    int rColor = Character.Protagonist.BackColor[0];
                    int gColor = Character.Protagonist.BackColor[1];
                    int bColor = Character.Protagonist.BackColor[2];

                    return Colors.Hex(rColor, gColor, bColor);
                }
                else
                {
                    return base.GetColor(type);
                }
            }
            else if (type == ColorTypes.Font)
            {
                return Colors.СontrastText(Character.Protagonist.BackColor);
            }
            else
            {
                return base.GetColor(type);
            }
        }
    }
}
