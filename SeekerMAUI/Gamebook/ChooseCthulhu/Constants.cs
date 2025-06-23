using static SeekerMAUI.Output.Buttons;
using static SeekerMAUI.Game.Data;

namespace SeekerMAUI.Gamebook.ChooseCthulhu
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, string> Availabilities { get; set; }

        public static void ChangeBackground()
        {
            Character.Protagonist.BackColor = Colors.Mod(Character.Protagonist.BackColor);
            Character.Protagonist.BtnColor = Colors.Mod(Character.Protagonist.BtnColor);
        }

        public override string GetColor(ButtonTypes type)
        {
            bool mainDuttons = (type == ButtonTypes.Main) || (type == ButtonTypes.Option);
            bool supplButtons = (type == ButtonTypes.System) || (type == ButtonTypes.Continue);

            if (Game.Settings.IsEnabled("WithoutStyles"))
            {
                return base.GetColor(type);
            }
            else if (mainDuttons || supplButtons)
            {
                int rColor = Character.Protagonist.BtnColor[0];
                int gColor = Character.Protagonist.BtnColor[1];
                int bColor = Character.Protagonist.BtnColor[2];

                return Colors.Hex(rColor, gColor, bColor);
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
                int rColor = Character.Protagonist.BackColor[0];
                int gColor = Character.Protagonist.BackColor[1];
                int bColor = Character.Protagonist.BackColor[2];

                return Colors.Hex(rColor, gColor, bColor);
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
