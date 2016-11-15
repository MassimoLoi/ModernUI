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
using System.Diagnostics;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
    [System.Drawing.ToolboxBitmapAttribute(typeof(System.Windows.Forms.ColorDialog)), ToolboxItem(true)]
    public partial class MetroRendererManager : Component
    {
        #region Inialization
        public MetroRendererManager()
        {
            components = new System.ComponentModel.Container();
            rnd = Renderer.MetroRenderer;
        }

        public MetroRendererManager(IContainer container)
        {
            container.Add(this);

            components = new System.ComponentModel.Container();
            rnd = Renderer.MetroRenderer;
        }
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Variables
        Renderer rnd = Renderer.MetroRenderer;
        MetroColorStyle style = MetroColorStyle.Blue;
        MetroThemeStyle theme = MetroThemeStyle.Light;
        #endregion

        #region Properties
        public MetroColorStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                SetValues();
            }
        }
        public MetroThemeStyle Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                SetValues();
            }
        }
        public Renderer Renderers
        {
            get { return rnd; }
            set
            {
                rnd = value;
                SetValues();
            }
        }
        #endregion

        #region Functions
        public void SetValues()
        {
            ToolStripManager.Renderer = new MetroCTXRenderer(theme, style);
        }
        #endregion
    }

    public enum Renderer
    {
        MetroRenderer
    }


}
