// Decompiled with JetBrains decompiler
// Type: SSTap.View.KaguyaButton
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

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
