using System;

namespace SeekerMAUI.Output
{
    public class VerticalText : BindableObject, IDrawable 
    {
        public List<string> statusLines { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FontSize = 12;
            canvas.Rotate(90);

            int index = 0;
            int count = statusLines.Count;
            double heightPart = (int)DeviceDisplay.MainDisplayInfo.Width / count;

            foreach (var status in statusLines)
            {
                float xpos = (float)((heightPart * index) + (heightPart / 2));
                canvas.DrawString(status, xpos, -5, HorizontalAlignment.Center);
                index += 1;
            }
        }
    }
}
