// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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

        private static readonly VisualStyleElement baseElement;

        private static readonly bool isSupported;

        [ThreadStatic]
        private static VisualStyleRenderer visualStyleRenderer;

        static ListItemRenderer()
        {
            if (!VisualStyleRenderer.IsSupported)
            {
                isSupported = false;
                return;
            }

            VisualStyleElement
                normal = VisualStyleElement.ListView.Item.Normal,
                hot = VisualStyleElement.ListView.Item.Hot,
                selected = VisualStyleElement.ListView.Item.Selected,
                disabled = VisualStyleElement.ListView.Item.Disabled,
                selectedNotFocus = VisualStyleElement.ListView.Item.SelectedNotFocus;

            isSupported = VisualStyleRenderer.IsElementDefined(normal) &&
                          VisualStyleRenderer.IsElementDefined(hot) &&
                          VisualStyleRenderer.IsElementDefined(selected) &&
                          VisualStyleRenderer.IsElementDefined(disabled) &&
                          VisualStyleRenderer.IsElementDefined(selectedNotFocus);

            if (isSupported)
            {
                baseElement = normal;
                return;
            }

            normal = VisualStyleElement.CreateElement("Explorer::ListView", 1, (int)ListViewItemState.Normal);
            hot = VisualStyleElement.CreateElement("Explorer::ListView", 1, (int)ListViewItemState.Hot);
            selected = VisualStyleElement.CreateElement("Explorer::ListView", 1, (int)ListViewItemState.Selected);
            disabled = VisualStyleElement.CreateElement("Explorer::ListView", 1, (int)ListViewItemState.Disabled);
            selectedNotFocus = VisualStyleElement.CreateElement("Explorer::ListView", 1, (int)ListViewItemState.SelectedNotFocus);

            isSupported = VisualStyleRenderer.IsElementDefined(normal) &&
                          VisualStyleRenderer.IsElementDefined(hot) &&
                          VisualStyleRenderer.IsElementDefined(selected) &&
                          VisualStyleRenderer.IsElementDefined(disabled) &&
                          VisualStyleRenderer.IsElementDefined(selectedNotFocus);

            if (isSupported)
            {
                baseElement = normal;
                return;
            }
        }

        public static bool RenderMatchingApplicationState { get; set; } = true;

        private static bool RenderWithVisualStyles => (!RenderMatchingApplicationState || Application.RenderWithVisualStyles) && isSupported;

        public static void RenderBackground(Graphics g, Rectangle bounds, Brush background, ListViewItemState state)
        {
            if (state == ListViewItemState.Normal)
            {
                if (background != null)
                {
                    g.FillRectangle(background, bounds);
                }
            }
            else if (RenderWithVisualStyles)
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

        private static void InitializeRenderer(ListViewItemState state)
        {
            if (visualStyleRenderer == null)
            {
                visualStyleRenderer = new VisualStyleRenderer(
                    baseElement.ClassName,
                    baseElement.Part,
                    (int)state);
            }
            else
            {
                visualStyleRenderer.SetParameters(
                    baseElement.ClassName,
                    baseElement.Part,
                    (int)state);
            }
        }

        private static void RenderItemsText(Graphics g, Rectangle[] textRectangles, string[] itemsText, Font font, TextFormatFlags[] flags = null)
        {
            if (itemsText == null)
            {
                throw new ArgumentNullException(nameof(itemsText));
            }

            if (textRectangles == null)
            {
                throw new ArgumentNullException(nameof(textRectangles));
            }

            var length = itemsText.Length;
            if (length != textRectangles.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(textRectangles));
            }

            if (flags == null)
            {
                for (var i = 0; i < length; i++)
                {
                    RenderItemText(g, textRectangles[i], itemsText[i], font, StandardFlags);
                }
            }
            else
            {
                if (length != flags.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(flags));
                }

                for (var i = 0; i < length; i++)
                {
                    RenderItemText(g, textRectangles[i], itemsText[i], font, flags[i]);
                }
            }
        }

        private static void RenderItemsText(Graphics g, Rectangle[] textRectangles, string[] itemsText, Font[] fonts, TextFormatFlags[] flags = null)
        {
            if (itemsText == null)
            {
                throw new ArgumentNullException(nameof(itemsText));
            }

            if (textRectangles == null)
            {
                throw new ArgumentNullException(nameof(textRectangles));
            }

            if (fonts == null)
            {
                throw new ArgumentNullException(nameof(fonts));
            }

            var length = itemsText.Length;
            if (length != textRectangles.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(textRectangles));
            }

            if (length != fonts.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(fonts));
            }

            if (flags == null)
            {
                for (var i = 0; i < length; i++)
                {
                    RenderItemText(g, textRectangles[i], itemsText[i], fonts[i], StandardFlags);
                }
            }
            else
            {
                if (length != flags.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(flags));
                }

                for (var i = 0; i < length; i++)
                {
                    RenderItemText(g, textRectangles[i], itemsText[i], fonts[i], flags[i]);
                }
            }
        }

        private static void RenderItemText(Graphics g, Rectangle textRectangle, string itemText, Font font, TextFormatFlags flags)
        {
            TextRenderer.DrawText(g, itemText, font, textRectangle, SystemColors.WindowText, flags);
        }
    }
}
