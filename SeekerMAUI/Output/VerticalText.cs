using Microsoft.Maui.Graphics;
using System;
using static SeekerMAUI.Game.Data;

namespace SeekerMAUI.Output
{
    public class VerticalText : BindableObject, IDrawable 
    {
        public List<string> StatusLines { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            string textColor = Game.Data.Constants.GetColor(ColorTypes.AdditionalFont);
            bool equalParts = Game.Data.Constants.GetBool("EqualPartsInStatuses");

            canvas.FontSize = Constants.VERTICAL_FONT;

            var color = (String.IsNullOrEmpty(textColor) ?
                Colors.Black : Color.FromHex(textColor));
            canvas.FontColor = color;
            canvas.StrokeColor = color;

            canvas.Rotate(90);

            double statusLength = StatusLines.Sum(x => x.Length);
            float yposText = Constants.VERTICAL_YPOS_TEXT;
            float yposLine = Constants.VERTICAL_YPOS_LINE;
            double displayWidth = DeviceDisplay.MainDisplayInfo.Width - Constants.HORIZONTAL_STATUS_SIZE;

            double allHeights = 0, lenPart = 0;

            foreach (var status in StatusLines)
            {
                if (equalParts)
                {
                    lenPart = (double)1 / StatusLines.Count;
                }
                else
                {
                    lenPart = (double)status.Length / statusLength;
                }

                double heightPart = displayWidth * lenPart;              
                string line = status.ToString();
                float xpos = (float)(allHeights + (heightPart / 2));
                allHeights += heightPart;

                if (status.Contains("BOLD|"))
                {
                    line = line.Replace("BOLD|", String.Empty);

                    canvas.Font = new Microsoft.Maui.Graphics.Font(string.Empty, Constants.VERTICAL_BOLD_TEXT);
                }
                else
                {
                    canvas.Font = new Microsoft.Maui.Graphics.Font(string.Empty);
                }

                if (status.Contains("CROSSEDOUT|"))
                {
                    line = line.Replace("CROSSEDOUT|", String.Empty);

                    float length = (line.Length * Constants.VERTICAL_LINE_LEN) +
                        Constants.VERTICAL_LINE_BONUS;

                    canvas.DrawLine(xpos - length, yposLine, xpos + length, yposLine);
                }

                canvas.DrawString(line, xpos, yposText, HorizontalAlignment.Center);
            }
        }
    }
}
