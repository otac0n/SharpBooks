﻿//-----------------------------------------------------------------------
// <copyright file="ListViewRenderer.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public enum ListViewItemState
    {
        Normal = 1,
        Hot = 2,
        Selected = 3,
        Disabled = 4,
        SelectedNotFocus = 5,
    }

    public static class ListItemRenderer
    {
        private const TextFormatFlags StandardFlags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;

        [ThreadStatic]
        private static VisualStyleRenderer visualStyleRenderer;

        private static bool renderMatchingApplicationState = true;

        private static bool isSupported;

        static ListItemRenderer()
        {
            isSupported = VisualStyleRenderer.IsSupported
                       && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ListView.Item.Normal)
                       && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ListView.Item.Hot)
                       && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ListView.Item.Selected)
                       && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ListView.Item.Disabled)
                       && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ListView.Item.SelectedNotFocus);
        }

        private static bool RenderWithVisualStyles
        {
            get
            {
                return (renderMatchingApplicationState ? Application.RenderWithVisualStyles : true) && isSupported;
            }
        }

        public static bool RenderMatchingApplicationState
        {
            get { return renderMatchingApplicationState; }

            set { renderMatchingApplicationState = value; }
        }

        private static void InitializeRenderer(ListViewItemState state)
        {
            if (visualStyleRenderer == null)
            {
                visualStyleRenderer = new VisualStyleRenderer(
                    VisualStyleElement.ListView.Item.Normal.ClassName,
                    VisualStyleElement.ListView.Item.Normal.Part,
                    (int)state);
            }
            else
            {
                visualStyleRenderer.SetParameters(
                    VisualStyleElement.ListView.Item.Normal.ClassName,
                    VisualStyleElement.ListView.Item.Normal.Part,
                    (int)state);
            }
        }

        public static void RenderBackground(Graphics g, Rectangle bounds, Brush background, ListViewItemState state)
        {
            if (RenderWithVisualStyles)
            {
                InitializeRenderer(state);
                visualStyleRenderer.DrawBackground(g, bounds);
            }
            else
            {
                if (state == ListViewItemState.Selected)
                {
                    g.FillRectangle(SystemBrushes.Highlight, bounds);
                }
                else if (background != null)
                {
                    g.FillRectangle(background, bounds);
                }
            }
        }

        private static void RenderItemsText(Graphics g, Rectangle[] textRectangles, string[] itemsText, Font font, TextFormatFlags[] flags = null)
        {
            if (itemsText == null)
            {
                throw new ArgumentNullException("itemsText");
            }

            if (textRectangles == null)
            {
                throw new ArgumentNullException("textRectangles");
            }

            var length = itemsText.Length;
            if (length != textRectangles.Length)
            {
                throw new ArgumentOutOfRangeException("textRectangles");
            }

            if (flags == null)
            {
                for (int i = 0; i < length; i++)
                {
                    RenderItemText(g, textRectangles[i], itemsText[i], font, StandardFlags);
                }
            }
            else
            {
                if (length != flags.Length)
                {
                    throw new ArgumentOutOfRangeException("flags");
                }

                for (int i = 0; i < length; i++)
                {
                    RenderItemText(g, textRectangles[i], itemsText[i], font, flags[i]);
                }
            }
        }

        private static void RenderItemsText(Graphics g, Rectangle[] textRectangles, string[] itemsText, Font[] fonts, TextFormatFlags[] flags = null)
        {
            if (itemsText == null)
            {
                throw new ArgumentNullException("itemsText");
            }

            if (textRectangles == null)
            {
                throw new ArgumentNullException("textRectangles");
            }

            if (fonts == null)
            {
                throw new ArgumentNullException("fonts");
            }

            var length = itemsText.Length;
            if (length != textRectangles.Length)
            {
                throw new ArgumentOutOfRangeException("textRectangles");
            }

            if (length != fonts.Length)
            {
                throw new ArgumentOutOfRangeException("fonts");
            }

            if (flags == null)
            {
                for (int i = 0; i < length; i++)
                {
                    RenderItemText(g, textRectangles[i], itemsText[i], fonts[i], StandardFlags);
                }
            }
            else
            {
                if (length != flags.Length)
                {
                    throw new ArgumentOutOfRangeException("flags");
                }

                for (int i = 0; i < length; i++)
                {
                    RenderItemText(g, textRectangles[i], itemsText[i], fonts[i], flags[i]);
                }
            }
        }

        private static void RenderItemText(Graphics g, Rectangle textRectangle, string itemText, Font font, TextFormatFlags flags)
        {
            TextRenderer.DrawText(g, itemText, font, textRectangle, SystemColors.WindowText, flags);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Rectangle textRectangle, string itemText, Font font, ListViewItemState state)
        {
            RenderBackground(g, bounds, null, state);
            RenderItemText(g, textRectangle, itemText, font, StandardFlags);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Rectangle textRectangle, string itemText, Font font, TextFormatFlags flags, ListViewItemState state)
        {
            RenderBackground(g, bounds, null, state);
            RenderItemText(g, textRectangle, itemText, font, flags);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Rectangle[] textRectangles, string[] itemsText, Font font, ListViewItemState state)
        {
            RenderBackground(g, bounds, null, state);
            RenderItemsText(g, textRectangles, itemsText, font);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Rectangle[] textRectangles, string[] itemsText, Font font, TextFormatFlags[] flags, ListViewItemState state)
        {
            RenderBackground(g, bounds, null, state);
            RenderItemsText(g, textRectangles, itemsText, font, flags);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Rectangle[] textRectangles, string[] itemsText, Font[] fonts, TextFormatFlags[] flags, ListViewItemState state)
        {
            RenderBackground(g, bounds, null, state);
            RenderItemsText(g, textRectangles, itemsText, fonts, flags);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Brush background, Rectangle textRectangle, string itemText, Font font, ListViewItemState state)
        {
            RenderBackground(g, bounds, background, state);
            RenderItemText(g, textRectangle, itemText, font, StandardFlags);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Brush background, Rectangle textRectangle, string itemText, Font font, TextFormatFlags flags, ListViewItemState state)
        {
            RenderBackground(g, bounds, background, state);
            RenderItemText(g, textRectangle, itemText, font, flags);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Brush background, Rectangle[] textRectangles, string[] itemsText, Font font, ListViewItemState state)
        {
            RenderBackground(g, bounds, background, state);
            RenderItemsText(g, textRectangles, itemsText, font);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Brush background, Rectangle[] textRectangles, string[] itemsText, Font font, TextFormatFlags[] flags, ListViewItemState state)
        {
            RenderBackground(g, bounds, background, state);
            RenderItemsText(g, textRectangles, itemsText, font, flags);
        }

        public static void RenderItems(Graphics g, Rectangle bounds, Brush background, Rectangle[] textRectangles, string[] itemsText, Font[] fonts, TextFormatFlags[] flags, ListViewItemState state)
        {
            RenderBackground(g, bounds, background, state);
            RenderItemsText(g, textRectangles, itemsText, fonts, flags);
        }
    }
}
