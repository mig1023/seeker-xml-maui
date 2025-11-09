using static SeekerMAUI.Output.Buttons;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<int, string> Buttons { get; set; }

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
