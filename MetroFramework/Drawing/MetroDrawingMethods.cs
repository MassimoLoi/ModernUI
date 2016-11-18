/**
 * MetroFramework - ExtendedRendering - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2016 Angelo Cresta, http://quarztech.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
 using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace MetroFramework.Drawing
{
    public static class MetroDrawingMethods
    {
        #region ... Color Manipulation ...
        public static Color GetDarkColor(Color clr)
        {
            Color c = new Color();
            int r, g, b;

            r = clr.R - 18;
            g = clr.G - 18;
            b = clr.B - 18;

            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;

            c = Color.FromArgb(r, g, b);
            return c;
        }


        public static Color GetLightColor(Color clr)
        {
            Color c = new Color();
            int r, g, b;

            r = clr.R + 18;
            g = clr.G + 18;
            b = clr.B + 18;

            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;

            c = Color.FromArgb(r, g, b);
            return c;
        }
        public static Color GetDarkColor(Color c, byte d)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            if ((c.R > d)) r = (byte)(c.R - d);
            if ((c.G > d)) g = (byte)(c.G - d);
            if ((c.B > d)) b = (byte)(c.B - d);

            Color c1 = Color.FromArgb(r, g, b);
            return c1;
        }

        public static Color GetLightColor(Color c, byte d)
        {
            byte r = 255;
            byte g = 255;
            byte b = 255;

            if (((int)c.R + (int)d <= 255)) r = (byte)(c.R + d);
            if (((int)c.G + (int)d <= 255)) g = (byte)(c.G + d);
            if (((int)c.B + (int)d <= 255)) b = (byte)(c.B + d);

            Color c2 = Color.FromArgb(r, g, b);
            return c2;
        }


        public static Color GetSystemDarkColor(Color clr)
        {
            return ControlPaint.Dark(clr);
        }
        public static Color GetSystemDarkDarkColor(Color clr)
        {
            return ControlPaint.DarkDark(clr);
        }

        public static Color GetSystemLightColor(Color clr)
        {
            return ControlPaint.Light(clr);
        }
        public static Color GetSystemLightLightColor(Color clr)
        {
            return ControlPaint.LightLight(clr);
        }

        public static Brush GetBrush(Rectangle rect, Color ColorBegin, Color ColorEnd, PaletteColorStyle ColorStyle, float Angle, VisualOrientation orientation, bool PreserveColors)
        {
            Blend blend1 = new Blend(4);
            Blend blend2;
            Blend blend3;
            Blend blend4;
            Blend blend5;
            Blend blend6;
            Blend blend7;
            Blend blend8;
            Blend blend9;
            Blend blend10;
            Blend blend11;
            Blend blend12;

            // One time creation of the blend for the status strip gradient brush
            Blend blend13 = new Blend();
            blend13.Positions = new float[] { 0.0f, 0.25f, 0.25f, 0.57f, 0.86f, 1.0f };
            blend13.Factors = new float[] { 0.1f, 0.6f, 1.0f, 0.4f, 0.0f, 0.95f };

            //Default
            blend1.Positions[0] = 0f;
            blend1.Factors[0] = 1f;
            blend1.Positions[1] = 0.4f;
            blend1.Factors[1] = 0.5f;
            blend1.Positions[2] = 0.4f;
            blend1.Factors[2] = 0.1f; //0 darker 0.3 lighter (middle line)
            blend1.Positions[3] = 1f;
            blend1.Factors[3] = 0.9f;

            float[] numArray = new float[4];
            numArray[2] = 1f;
            numArray[3] = 1f;
            blend2 = new Blend();
            float[] numArray2 = new float[4];
            numArray2[2] = 1f;
            numArray2[3] = 1f;
            blend2.Factors = numArray2;
            blend2.Positions = new float[] { 0f, 0.33f, 0.33f, 1f };
            blend3 = new Blend();
            float[] numArray3 = new float[4];
            numArray3[2] = 1f;
            numArray3[3] = 1f;
            blend3.Factors = numArray3;
            blend3.Positions = new float[] { 0f, 0.5f, 0.5f, 1f };
            blend4 = new Blend();
            float[] numArray4 = new float[4];
            numArray4[3] = 1f;
            blend4.Factors = numArray4;
            blend4.Positions = new float[] { 0f, 0.9f, 0.9f, 1f };
            blend5 = new Blend();
            blend5.Factors = new float[] { 0f, 0.5f, 1f, 0.05f };
            blend5.Positions = new float[] { 0f, 0.45f, 0.45f, 1f };
            blend6 = new Blend();
            blend6.Factors = new float[] { 0f, 0f, 0.25f, 0.7f, 1f, 1f };
            blend6.Positions = new float[] { 0f, 0.1f, 0.2f, 0.3f, 0.5f, 1f };
            blend7 = new Blend();
            blend7.Factors = new float[] { 0.15f, 0.75f, 1f, 1f };
            blend7.Positions = new float[] { 0f, 0.45f, 0.45f, 1f };
            blend8 = new Blend();
            blend8.Factors = new float[] { 0.8f, 0.2f, 0f, 0.07f, 1f };
            blend8.Positions = new float[] { 0f, 0.33f, 0.33f, 0.43f, 1f };
            blend9 = new Blend();
            blend9.Factors = new float[] { 1f, 0.7f, 0.7f, 0f, 0.1f, 0.55f, 1f, 1f };
            blend9.Positions = new float[] { 0f, 0.16f, 0.33f, 0.35f, 0.51f, 0.85f, 0.85f, 1f };
            blend10 = new Blend();
            blend10.Factors = new float[] { 1f, 0.78f, 0.48f, 1f, 1f };
            blend10.Positions = new float[] { 0f, 0.33f, 0.33f, 0.9f, 1f };
            blend11 = new Blend();
            blend12 = new Blend();
            blend12.Factors = numArray;
            blend12.Positions = new float[] { 0f, 0.25f, 0.25f, 1f };


            //For Gefault Type Only
            if (ColorStyle == PaletteColorStyle.Default)
            {
                LinearGradientBrush lb;
                if (PreserveColors)
                { lb = new LinearGradientBrush(rect, ColorEnd, ColorBegin, Angle); }
                else
                { lb = new LinearGradientBrush(rect, GetDarkColor(ColorEnd), GetLightColor(ColorBegin), Angle); }

                lb.Blend = blend1;
                return lb;
            }


            //Solid
            if (ColorStyle == PaletteColorStyle.Solid)
            {
                return new SolidBrush(ColorBegin);
            }

            switch (orientation)
            {
                case VisualOrientation.Bottom:
                    Angle += 180f;
                    break;

                case VisualOrientation.Left:
                    Angle -= 90f;
                    break;

                case VisualOrientation.Right:
                    Angle += 90f;
                    break;
            }

            //One Note Specific
            if (ColorStyle == PaletteColorStyle.OneNote)
            {
                ColorBegin = Color.White;
            }

            //Others
            LinearGradientBrush brush = new LinearGradientBrush(rect, ColorBegin, ColorEnd, Angle);
            switch (ColorStyle)
            {
                case PaletteColorStyle.Status:
                    brush.Blend = blend13;
                    return brush;

                case PaletteColorStyle.Switch25:
                    brush.Blend = blend12;
                    return brush;

                case PaletteColorStyle.Switch33:
                    brush.Blend = blend2;
                    return brush;

                case PaletteColorStyle.Switch50:
                    brush.Blend = blend3;
                    return brush;

                case PaletteColorStyle.Switch90:
                    brush.Blend = blend4;
                    return brush;

                case PaletteColorStyle.Linear:
                    return brush;

                case PaletteColorStyle.Rounded:
                    brush.SetSigmaBellShape(1f, 1f);
                    return brush;

                case PaletteColorStyle.Rounding2:
                    brush.Blend = blend8;
                    return brush;

                case PaletteColorStyle.Rounding3:
                    brush.Blend = blend9;
                    return brush;

                case PaletteColorStyle.Rounding4:
                    brush.Blend = blend10;
                    return brush;

                case PaletteColorStyle.Sigma:
                    brush.SetSigmaBellShape(0.5f);
                    return brush;

                case PaletteColorStyle.HalfCut:
                    brush.Blend = blend5;
                    return brush;

                case PaletteColorStyle.QuarterPhase:
                    brush.Blend = blend6;
                    return brush;

                case PaletteColorStyle.OneNote:
                    brush.Blend = blend7;
                    return brush;
            }
            return brush;
        }

        public enum CornerType
        {
            Rounded,
            Squared
        }

        public enum VisualOrientation
        {
            Top,
            Bottom,
            Left,
            Right
        }

        public enum PaletteColorStyle
        {
            Default,
            Solid,
            Switch25,
            Switch33,
            Switch50,
            Switch90,
            Linear,
            Rounded,
            Rounding2,
            Rounding3,
            Rounding4,
            Sigma,
            HalfCut,
            QuarterPhase,
            OneNote,
            Status
        }

        public static Color ColorFromAhsb(int Alpha, float Hue, float Saturation, float Brightness)
        {

            if (0 > Alpha || 255 < Alpha)
            {
                throw new ArgumentOutOfRangeException("a", Alpha, "InvalidAlpha");
            }
            if (0f > Hue || 360f < Hue)
            {
                throw new ArgumentOutOfRangeException("h", Hue, "InvalidHue");
            }
            if (0f > Saturation || 1f < Saturation)
            {
                throw new ArgumentOutOfRangeException("s", Saturation, "InvalidSaturation");
            }
            if (0f > Brightness || 1f < Brightness)
            {
                throw new ArgumentOutOfRangeException("b", Brightness, "InvalidBrightness");
            }

            if (0 == Saturation)
            {
                return Color.FromArgb(Alpha, Convert.ToInt32(Brightness * 255),
                  Convert.ToInt32(Brightness * 255), Convert.ToInt32(Brightness * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < Brightness)
            {
                fMax = Brightness - (Brightness * Saturation) + Saturation;
                fMin = Brightness + (Brightness * Saturation) - Saturation;
            }
            else
            {
                fMax = Brightness + (Brightness * Saturation);
                fMin = Brightness - (Brightness * Saturation);
            }

            iSextant = (int)Math.Floor(Hue / 60f);
            if (300f <= Hue)
            {
                Hue -= 360f;
            }
            Hue /= 60f;
            Hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = Hue * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - Hue * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(Alpha, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(Alpha, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(Alpha, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(Alpha, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(Alpha, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(Alpha, iMax, iMid, iMin);
            }
        }

        #endregion

        #region ... Graphic Paths ...

        public static GraphicsPath CreateRectGraphicsPath(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(rect);
            return path;
        }

        public static GraphicsPath GetRoundedSquarePath(Rectangle bounds, int radius)
        {
            int x = bounds.X, y = bounds.Y, w = bounds.Width, h = bounds.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);				//Upper left corner
            path.AddArc(x + w - radius, y, radius, radius, 270, 90);			//Upper right corner
            path.AddArc(x + w - radius, y + h - radius, radius, radius, 0, 90);		//Lower right corner
            path.AddArc(x, y + h - radius, radius, radius, 90, 90);			//Lower left corner
            path.CloseFigure();
            return path;
        }
        #endregion

        #region ... Graphic Polygon ...
        public static PointF[] GetPoligonPointsFromPath(GraphicsPath path)
        {
            return path.PathPoints;
        }
        #endregion

        #region  ... Utils ...
        public static StringFormat GetStringFormat(ContentAlignment contentAlignment)
        {
            if (!Enum.IsDefined(typeof(ContentAlignment), (int)contentAlignment))
                throw new System.ComponentModel.InvalidEnumArgumentException(
                    "contentAlignment", (int)contentAlignment, typeof(ContentAlignment));

            StringFormat stringFormat = new StringFormat();

            switch (contentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleLeft:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleRight:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.TopCenter:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.TopLeft:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopRight:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomCenter:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.BottomLeft:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.BottomRight:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
            }

            return stringFormat;
        }
        #endregion
    }
}
