using static SeekerMAUI.Output.Buttons;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, string> StatNames { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }

        public static Dictionary<int, string> Buttons { get; set; }

        public static int StoryPart()
        {
            int part = 0;
            int current = Game.Data.CurrentParagraphID;

            foreach (int startParagraph in Buttons.Keys.OrderBy(x => x))
            {
                if (current >= startParagraph)
                    part += 1;
            }

            return part > 0 ? part : 1;
        }

        public override string GetColor(ButtonTypes type)
        {
            if ((type == ButtonTypes.Main) || (type == ButtonTypes.Action))
            {
                string currentColor = String.Empty;

                foreach (int startParagraph in Constants.Buttons.Keys.OrderBy(x => x))
                {
                    if (Game.Data.CurrentParagraphID >= startParagraph)
                        currentColor = Constants.Buttons[startParagraph];
                }

                return currentColor;
            }
            else
            {
                return base.GetColor(type);
            }
        }
    }
}
