using System;
using static SeekerMAUI.Game.Data;

namespace SeekerMAUI.Output
{
    class StatusBar
    {
        public static List<Label> Main(List<string> statusLines)
        {
            List<Label> statusLabels = new List<Label>();

            string textColor = Game.Data.Constants.GetColor(ColorTypes.StatusFont);

            foreach (string status in statusLines)
            {
                Label label = new Label
                {
                    Text = status + Convert.ToChar(160),
                    FontSize = Constants.STATUSBAR_FONT,
                    TextColor = (String.IsNullOrEmpty(textColor) ? Colors.White : Color.FromHex(textColor)),
                    BackgroundColor = Color.FromHex(Game.Data.Constants.GetColor(ColorTypes.StatusBar)),

                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

                statusLabels.Add(label);
            }

            return statusLabels;
        }
    }
}
