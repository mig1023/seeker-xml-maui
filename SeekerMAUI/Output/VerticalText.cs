using System;
using static SeekerMAUI.Game.Data;

namespace SeekerMAUI.Output
{
    public class VerticalText : BindableObject, IDrawable 
    {
        public List<string> statusLines { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            string textColor = Game.Data.Constants.GetColor(ColorTypes.AdditionalFont);

            canvas.FontSize = Constants.VERTICAL_FONT;

            var color = (String.IsNullOrEmpty(textColor) ?
                Colors.Black : Color.FromHex(textColor));
            canvas.FontColor = color;
            canvas.StrokeColor = color;

            canvas.Rotate(90);

            int index = 0;
            int count = statusLines.Count;
            double heightPart = (int)DeviceDisplay.MainDisplayInfo.Width / count;
            float yposText = Constants.VERTICAL_YPOS_TEXT;
            float yposLine = Constants.VERTICAL_YPOS_LINE;
            float horizontal = Constants.HORIZONTAL_HEIGHT;

            foreach (var status in statusLines)
            {
                string line = status.ToString();
                float correction = horizontal - ((horizontal / count) * (count - index + 1));
                float xpos = (float)((heightPart * index) + (heightPart / 2)) - correction;

                if (status.Contains("CROSSEDOUT|"))
                {
                    line = line.Replace("CROSSEDOUT|", String.Empty);

                    float length = (line.Length * Constants.VERTICAL_LINE_LEN) + 
                        Constants.VERTICAL_LINE_BONUS;

                    canvas.DrawString(line, xpos, yposText, HorizontalAlignment.Center);
                    canvas.DrawLine(xpos - length, yposLine, xpos + length, yposLine);
                }
                else
                {
                    canvas.DrawString(line, xpos, yposText, HorizontalAlignment.Center);
                }
                
                index += 1;
            }
        }
    }
}
