using System;

namespace SeekerMAUI.Output
{
    public class VerticalText : BindableObject, IDrawable 
    {
        public List<string> statusLines { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FontSize = Constants.VERTICAL_FONT;
            canvas.Rotate(90);

            int index = 0;
            int count = statusLines.Count;
            double heightPart = (int)DeviceDisplay.MainDisplayInfo.Width / count;
            float yposText = Constants.VERTICAL_YPOS_TEXT;
            float yposLine = Constants.VERTICAL_YPOS_LINE;

            foreach (var status in statusLines)
            {
                string line = status.ToString();
                float xpos = (float)((heightPart * index) + (heightPart / 2));

                if (status.Contains("CROSSEDOUT|"))
                {
                    line = line.Replace("CROSSEDOUT|", String.Empty);
                    float length = line.Length * Constants.VERTICAL_LINE_LEN;

                    canvas.DrawLine(xpos - length, yposLine, xpos + length, yposLine);
                }
                
                canvas.DrawString(line, xpos, yposText, HorizontalAlignment.Center);
                
                index += 1;
            }
        }
    }
}
