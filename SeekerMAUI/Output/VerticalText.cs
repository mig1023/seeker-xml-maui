using System;

namespace SeekerMAUI.Output
{
    public class VerticalText : BindableObject, IDrawable 
    {
        public List<string> statusLines { get; set; }

        private float XPosition(int index)
        {
            int count = statusLines.Count;
            double heightPart = (int)DeviceDisplay.MainDisplayInfo.Width / count;

            if (index == 0)
            {
                return 10;
            }
            else
            {
                return (float)heightPart * index;
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FontSize = 12;
            canvas.Rotate(90);

            int index = 0;

            foreach (var status in statusLines)
            {
                canvas.DrawString(status, XPosition(index), -5, HorizontalAlignment.Left);
                index += 1;
            }
        }
    }
}
