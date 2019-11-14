using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SSTap.View
{
    internal class KaguyaButton : Button
    {
        private GraphicsPath GetRoundPath(RectangleF Rect, int radius)
        {
            float num = (float)radius / 2f;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(Rect.X, Rect.Y, (float)radius, (float)radius, 180f, 90f);
            graphicsPath.AddLine(Rect.X + num, Rect.Y, Rect.Width - num, Rect.Y);
            graphicsPath.AddArc(Rect.X + Rect.Width - (float)radius, Rect.Y, (float)radius, (float)radius, 270f, 90f);
            graphicsPath.AddLine(Rect.Width, Rect.Y + num, Rect.Width, Rect.Height - num);
            graphicsPath.AddArc(Rect.X + Rect.Width - (float)radius, Rect.Y + Rect.Height - (float)radius, (float)radius, (float)radius, 0.0f, 90f);
            graphicsPath.AddLine(Rect.Width - num, Rect.Height, Rect.X + num, Rect.Height);
            graphicsPath.AddArc(Rect.X, Rect.Y + Rect.Height - (float)radius, (float)radius, (float)radius, 90f, 90f);
            graphicsPath.AddLine(Rect.X, Rect.Height - num, Rect.X, Rect.Y + num);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath roundPath = this.GetRoundPath(new RectangleF(0.0f, 0.0f, (float)this.Width, (float)this.Height), 40);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawPath(new Pen(Color.CadetBlue, 1.75f), roundPath);
            base.OnPaint(e);
            this.Region = new Region(roundPath);
        }
    }
}
