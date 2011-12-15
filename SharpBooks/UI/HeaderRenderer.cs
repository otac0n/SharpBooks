﻿//-----------------------------------------------------------------------
// <copyright file="HeaderRenderer.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms.VisualStyles;
    using System.Windows.Forms;
    using System.Drawing;

    public enum HeaderItemState
    {
        Normal = 1,
        Hot = 2,
        Pressed = 3
    }

    public static class HeaderRenderer
    {
        [ThreadStatic]
        private static VisualStyleRenderer visualStyleRenderer;

        private static bool renderMatchingApplicationState = true;

        private static bool RenderWithVisualStyles
        {
            get
            {
                return renderMatchingApplicationState ? Application.RenderWithVisualStyles : true;
            }
        }

        public static bool RenderMatchingApplicationState
        {
            get { return renderMatchingApplicationState; }

            set { renderMatchingApplicationState = value; }
        }

        private static ButtonState ConvertToButtonState(HeaderItemState state)
        {
            if (state == HeaderItemState.Pressed)
            {
                return ButtonState.Pushed;
            }

            return ButtonState.Normal;
        }

        private static void InitializeRenderer(int state)
        {
            if (visualStyleRenderer == null)
            {
                visualStyleRenderer = new VisualStyleRenderer(
                    VisualStyleElement.Header.Item.Normal.ClassName,
                    VisualStyleElement.Header.Item.Normal.Part,
                    state);
            }
            else
            {
                visualStyleRenderer.SetParameters(
                    VisualStyleElement.Header.Item.Normal.ClassName,
                    VisualStyleElement.Header.Item.Normal.Part,
                    state);
            }
        }

        public static void DrawHeader(Graphics g, Rectangle bounds, HeaderItemState state)
        {
            if (RenderWithVisualStyles)
            {
                InitializeRenderer((int)state);
                visualStyleRenderer.DrawBackground(g, bounds);
            }
            else
            {
                ControlPaint.DrawButton(g, bounds, ConvertToButtonState(state));
            }
        }

        public static void DrawHeader(Graphics g, Rectangle bounds, Image image, Rectangle imageBounds, HeaderItemState state)
        {
            if (RenderWithVisualStyles)
            {
                InitializeRenderer((int)state);
                visualStyleRenderer.DrawBackground(g, bounds);
                visualStyleRenderer.DrawImage(g, imageBounds, image);
            }
            else
            {
                ControlPaint.DrawButton(g, bounds, ConvertToButtonState(state));
                g.DrawImage(image, imageBounds);
            }
        }

        public static void DrawHeader(Graphics g, Rectangle bounds, string headerText, Font font, HeaderItemState state)
        {
            DrawHeader(g, bounds, headerText, font, TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis, state);
        }

        public static void DrawHeader(Graphics g, Rectangle bounds, string headerText, Font font, TextFormatFlags flags, HeaderItemState state)
        {
            Color text;
            if (RenderWithVisualStyles)
            {
                InitializeRenderer((int)state);
                visualStyleRenderer.DrawBackground(g, bounds);
                text = visualStyleRenderer.GetColor(ColorProperty.TextColor);
            }
            else
            {
                ControlPaint.DrawButton(g, bounds, ConvertToButtonState(state));
                text = SystemColors.ControlText;
            }

            TextRenderer.DrawText(g, headerText, font, Rectangle.Inflate(bounds, -3, -3), text, flags);
        }

        public static void DrawHeader(Graphics g, Rectangle bounds, string headerText, Font font, Image image, Rectangle imageBounds, HeaderItemState state)
        {
            DrawHeader(g, bounds, headerText, font, TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis, image, imageBounds, state);
        }

        public static void DrawHeader(Graphics g, Rectangle bounds, string headerText, Font font, TextFormatFlags flags, Image image, Rectangle imageBounds, HeaderItemState state)
        {
            Color text;
            if (RenderWithVisualStyles)
            {
                InitializeRenderer((int)state);
                visualStyleRenderer.DrawBackground(g, bounds);
                visualStyleRenderer.DrawImage(g, imageBounds, image);
                text = visualStyleRenderer.GetColor(ColorProperty.TextColor);
            }
            else
            {
                ControlPaint.DrawButton(g, bounds, ConvertToButtonState(state));
                g.DrawImage(image, imageBounds);
                text = SystemColors.ControlText;
            }

            TextRenderer.DrawText(g, headerText, font, Rectangle.Inflate(bounds, -3, -3), text, flags);
        }
    }
}
