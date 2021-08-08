// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public enum HeaderItemState
    {
        Normal = 1,
        Hot = 2,
        Pressed = 3
    }

    public static class HeaderRenderer
    {
        private static bool isSupported;
        [ThreadStatic]
        private static VisualStyleRenderer visualStyleRenderer;

        static HeaderRenderer()
        {
            isSupported = VisualStyleRenderer.IsSupported
                       && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.Item.Normal)
                       && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.Item.Hot)
                       && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.Item.Pressed);
        }

        public static bool RenderMatchingApplicationState { get; set; } = true;

        private static bool RenderWithVisualStyles => (!RenderMatchingApplicationState || Application.RenderWithVisualStyles) && isSupported;

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
    }
}
