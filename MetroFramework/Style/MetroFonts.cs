/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
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
using System.Drawing;

namespace MetroFramework
{
    #region "   Enums   "
    public enum MetroLabelSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroLabelWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum MetroListViewSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroListViewWeight
    {
        Light,
        Regular,
        Bold
    }


    public enum MetroLinkSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroLinkWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum MetroTextBoxSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroTextBoxWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum MetroProgressBarSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroProgressBarWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum MetroTabControlSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroTabControlWeight
    {
        Light,
        Regular,
        Bold
    }
    public enum MetroDateTimeSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroDateTimeWeight
    {
        Light,
        Regular,
        Bold
    }
    public enum MetroButtonSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroButtonWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum MetroCheckBoxSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroCheckBoxWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum MetroComboBoxSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroComboBoxWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum MetroDataGridSize
    {
        Small,
        Medium,
        Tall
    }

    public enum MetroDataGridWeight
    {
        Light,
        Regular,
        Bold
    }
    #endregion

    #region "   Fonts   "
    public sealed class MetroFonts
    {
        private static Font GetSaveFont(string key, FontStyle style, float size)
        {
            return new Font(key, size, style, GraphicsUnit.Pixel);
        }

        public static Font DefaultLight(float size)
        {
            return GetSaveFont("Segoe UI Light", FontStyle.Regular, size);
        }

        public static Font Default(float size)
        {
            return GetSaveFont("Segoe UI", FontStyle.Regular, size);
        }

        public static Font DefaultBold(float size)
        {
            return GetSaveFont("Segoe UI", FontStyle.Bold, size);
        }

        public static Font Title
        {
            get { return DefaultLight(24f); }
        }

        public static Font Subtitle
        {
            get { return Default(14f); }
        }

        public static Font DateTime(float size)
        {
            return GetSaveFont("Segoe UI", FontStyle.Regular, size);
        }

        public static Font Tile
        {
            get { return Default(14f); }
        }

        public static Font TileCount
        {
            get { return Default(44f); }
        }

        public static Font Link(MetroLinkSize linkSize, MetroLinkWeight linkWeight)
        {
            if (linkSize == MetroLinkSize.Small)
            {
                if (linkWeight == MetroLinkWeight.Light)
                    return DefaultLight(12f);
                if (linkWeight == MetroLinkWeight.Regular)
                    return Default(12f);
                if (linkWeight == MetroLinkWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (linkSize == MetroLinkSize.Medium)
            {
                if (linkWeight == MetroLinkWeight.Light)
                    return DefaultLight(14f);
                if (linkWeight == MetroLinkWeight.Regular)
                    return Default(14f);
                if (linkWeight == MetroLinkWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (linkSize == MetroLinkSize.Tall)
            {
                if (linkWeight == MetroLinkWeight.Light)
                    return DefaultLight(18f);
                if (linkWeight == MetroLinkWeight.Regular)
                    return Default(18f);
                if (linkWeight == MetroLinkWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(12f);
        }

        public static Font ComboBox(MetroComboBoxSize linkSize, MetroComboBoxWeight linkWeight)
        {
            if (linkSize == MetroComboBoxSize.Small)
            {
                if (linkWeight == MetroComboBoxWeight.Light)
                    return DefaultLight(12f);
                if (linkWeight == MetroComboBoxWeight.Regular)
                    return Default(12f);
                if (linkWeight == MetroComboBoxWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (linkSize == MetroComboBoxSize.Medium)
            {
                if (linkWeight == MetroComboBoxWeight.Light)
                    return DefaultLight(14f);
                if (linkWeight == MetroComboBoxWeight.Regular)
                    return Default(14f);
                if (linkWeight == MetroComboBoxWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (linkSize == MetroComboBoxSize.Tall)
            {
                if (linkWeight == MetroComboBoxWeight.Light)
                    return DefaultLight(18f);
                if (linkWeight == MetroComboBoxWeight.Regular)
                    return Default(18f);
                if (linkWeight == MetroComboBoxWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(12f);
        }

        public static Font Label(MetroLabelSize labelSize, MetroLabelWeight labelWeight)
        {
            if (labelSize == MetroLabelSize.Small)
            {
                if (labelWeight == MetroLabelWeight.Light)
                    return DefaultLight(12f);
                if (labelWeight == MetroLabelWeight.Regular)
                    return Default(12f);
                if (labelWeight == MetroLabelWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (labelSize == MetroLabelSize.Medium)
            {
                if (labelWeight == MetroLabelWeight.Light)
                    return DefaultLight(14f);
                if (labelWeight == MetroLabelWeight.Regular)
                    return Default(14f);
                if (labelWeight == MetroLabelWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (labelSize == MetroLabelSize.Tall)
            {
                if (labelWeight == MetroLabelWeight.Light)
                    return DefaultLight(18f);
                if (labelWeight == MetroLabelWeight.Regular)
                    return Default(18f);
                if (labelWeight == MetroLabelWeight.Bold)
                    return DefaultBold(18f);
            }

            return DefaultLight(14f);
        }

        public static Font TextBox(MetroTextBoxSize tbSize, MetroTextBoxWeight tbWeight)
        {
            if (tbSize == MetroTextBoxSize.Small)
            {
                if (tbWeight == MetroTextBoxWeight.Light)
                    return DefaultLight(12f);
                if (tbWeight == MetroTextBoxWeight.Regular)
                    return Default(12f);
                if (tbWeight == MetroTextBoxWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (tbSize == MetroTextBoxSize.Medium)
            {
                if (tbWeight == MetroTextBoxWeight.Light)
                    return DefaultLight(14f);
                if (tbWeight == MetroTextBoxWeight.Regular)
                    return Default(14f);
                if (tbWeight == MetroTextBoxWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (tbSize == MetroTextBoxSize.Tall)
            {
                if (tbWeight == MetroTextBoxWeight.Light)
                    return DefaultLight(18f);
                if (tbWeight == MetroTextBoxWeight.Regular)
                    return Default(18f);
                if (tbWeight == MetroTextBoxWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(12f);
        }

        public static Font ProgressBar(MetroProgressBarSize pbSize, MetroProgressBarWeight pbWeight)
        {
            if (pbSize == MetroProgressBarSize.Small)
            {
                if (pbWeight == MetroProgressBarWeight.Light)
                    return DefaultLight(12f);
                if (pbWeight == MetroProgressBarWeight.Regular)
                    return Default(12f);
                if (pbWeight == MetroProgressBarWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (pbSize == MetroProgressBarSize.Medium)
            {
                if (pbWeight == MetroProgressBarWeight.Light)
                    return DefaultLight(14f);
                if (pbWeight == MetroProgressBarWeight.Regular)
                    return Default(14f);
                if (pbWeight == MetroProgressBarWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (pbSize == MetroProgressBarSize.Tall)
            {
                if (pbWeight == MetroProgressBarWeight.Light)
                    return DefaultLight(18f);
                if (pbWeight == MetroProgressBarWeight.Regular)
                    return Default(18f);
                if (pbWeight == MetroProgressBarWeight.Bold)
                    return DefaultBold(18f);
            }

            return DefaultLight(14f);
        }

        public static Font TabControl(MetroTabControlSize tcSize, MetroTabControlWeight tcWeight)
        {
            if (tcSize == MetroTabControlSize.Small)
            {
                if (tcWeight == MetroTabControlWeight.Light)
                    return DefaultLight(12f);
                if (tcWeight == MetroTabControlWeight.Regular)
                    return Default(12f);
                if (tcWeight == MetroTabControlWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (tcSize == MetroTabControlSize.Medium)
            {
                if (tcWeight == MetroTabControlWeight.Light)
                    return DefaultLight(14f);
                if (tcWeight == MetroTabControlWeight.Regular)
                    return Default(14f);
                if (tcWeight == MetroTabControlWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (tcSize == MetroTabControlSize.Tall)
            {
                if (tcWeight == MetroTabControlWeight.Light)
                    return DefaultLight(18f);
                if (tcWeight == MetroTabControlWeight.Regular)
                    return Default(18f);
                if (tcWeight == MetroTabControlWeight.Bold)
                    return DefaultBold(18f);
            }

            return DefaultLight(14f);
        }

        public static Font Button(MetroButtonSize bSize, MetroButtonWeight bWeight)
        {
            if (bSize == MetroButtonSize.Small)
            {
                if (bWeight == MetroButtonWeight.Light)
                    return DefaultLight(11f);
                if (bWeight == MetroButtonWeight.Regular)
                    return Default(11f);
                if (bWeight == MetroButtonWeight.Bold)
                    return DefaultBold(11f);
            }
            else if (bSize == MetroButtonSize.Medium)
            {
                if (bWeight == MetroButtonWeight.Light)
                    return DefaultLight(13f);
                if (bWeight == MetroButtonWeight.Regular)
                    return Default(13f);
                if (bWeight == MetroButtonWeight.Bold)
                    return DefaultBold(13f);
            }
            else if (bSize == MetroButtonSize.Tall)
            {
                if (bWeight == MetroButtonWeight.Light)
                    return DefaultLight(16f);
                if (bWeight == MetroButtonWeight.Regular)
                    return Default(16f);
                if (bWeight == MetroButtonWeight.Bold)
                    return DefaultBold(16f);
            }

            return Default(11f);
        }

        public static Font ListView(MetroListViewSize lvSize, MetroListViewWeight lvWeight)
        {
            if (lvSize == MetroListViewSize.Small)
            {
                if (lvWeight == MetroListViewWeight.Light)
                    return DefaultLight(12f);
                if (lvWeight == MetroListViewWeight.Regular)
                    return Default(12f);
                if (lvWeight == MetroListViewWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (lvSize == MetroListViewSize.Medium)
            {
                if (lvWeight == MetroListViewWeight.Light)
                    return DefaultLight(14f);
                if (lvWeight == MetroListViewWeight.Regular)
                    return Default(14f);
                if (lvWeight == MetroListViewWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (lvSize == MetroListViewSize.Tall)
            {
                if (lvWeight == MetroListViewWeight.Light)
                    return DefaultLight(16f);
                if (lvWeight == MetroListViewWeight.Regular)
                    return Default(16f);
                if (lvWeight == MetroListViewWeight.Bold)
                    return DefaultBold(16f);
            }

            return Default(11f);
        }

        public static Font DateTime(MetroDateTimeSize dtSize, MetroDateTimeWeight dtWeight)
        {
            if (dtSize == MetroDateTimeSize.Small)
            {
                if (dtWeight == MetroDateTimeWeight.Light)
                    return DefaultLight(12f);
                if (dtWeight == MetroDateTimeWeight.Regular)
                    return Default(12f);
                if (dtWeight == MetroDateTimeWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (dtSize == MetroDateTimeSize.Medium)
            {
                if (dtWeight == MetroDateTimeWeight.Light)
                    return DefaultLight(14f);
                if (dtWeight == MetroDateTimeWeight.Regular)
                    return Default(14f);
                if (dtWeight == MetroDateTimeWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (dtSize == MetroDateTimeSize.Tall)
            {
                if (dtWeight == MetroDateTimeWeight.Light)
                    return DefaultLight(18f);
                if (dtWeight == MetroDateTimeWeight.Regular)
                    return Default(18f);
                if (dtWeight == MetroDateTimeWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(12f);
        }

        public static Font DataGrid(MetroDataGridSize dgSize, MetroDataGridWeight dgWeight)
        {
            if (dgSize == MetroDataGridSize.Small)
            {
                if (dgWeight == MetroDataGridWeight.Light)
                    return DefaultLight(12f);
                if (dgWeight == MetroDataGridWeight.Regular)
                    return Default(12f);
                if (dgWeight == MetroDataGridWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (dgSize == MetroDataGridSize.Medium)
            {
                if (dgWeight == MetroDataGridWeight.Light)
                    return DefaultLight(14f);
                if (dgWeight == MetroDataGridWeight.Regular)
                    return Default(14f);
                if (dgWeight == MetroDataGridWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (dgSize == MetroDataGridSize.Tall)
            {
                if (dgWeight == MetroDataGridWeight.Light)
                    return DefaultLight(18f);
                if (dgWeight == MetroDataGridWeight.Regular)
                    return Default(18f);
                if (dgWeight == MetroDataGridWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(11f);
        }

    }
    #endregion
}