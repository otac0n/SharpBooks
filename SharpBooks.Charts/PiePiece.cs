using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace SharpBooks.Charts
{
    internal class PiePiece : Shape
    {
        public static readonly DependencyProperty OuterRadiusProperty =
            DependencyProperty.Register(
                "OuterRadius",
                typeof(double),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register(
                "InnerRadius",
                typeof(double),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RadiusPunchProperty =
            DependencyProperty.Register(
                "RadiusPunch",
                typeof(double),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                "StartAngle",
                typeof(double),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty OuterAngleProperty =
            DependencyProperty.Register(
                "OuterAngle",
                typeof(double),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty InnerAngleProperty =
            DependencyProperty.Register(
                "InnerAngle",
                typeof(double),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty TotalLevelsProperty =
            DependencyProperty.Register(
                "TotalLevels",
                typeof(int),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(2, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty OuterLevelProperty =
            DependencyProperty.Register(
                "OuterLevel",
                typeof(int),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty InnerLevelProperty =
            DependencyProperty.Register(
                "InnerLevel",
                typeof(int),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(
                "Center",
                typeof(Point),
                typeof(PiePiece),
                new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Point Center
        {
            get
            {
                return (Point)GetValue(CenterProperty);
            }

            set
            {
                SetValue(CenterProperty, value);
            }
        }

        public double OuterRadius
        {
            get
            {
                return (double)GetValue(OuterRadiusProperty);
            }

            set
            {
                SetValue(OuterRadiusProperty, value);
            }
        }

        public double InnerRadius
        {
            get
            {
                return (double)GetValue(InnerRadiusProperty);
            }

            set
            {
                SetValue(InnerRadiusProperty, value);
            }
        }

        public double RadiusPunch
        {
            get
            {
                return (double)GetValue(RadiusPunchProperty);
            }

            set
            {
                SetValue(RadiusPunchProperty, value);
            }
        }

        public double StartAngle
        {
            get
            {
                return (double)GetValue(StartAngleProperty);
            }

            set
            {
                SetValue(StartAngleProperty, value);
            }
        }

        public double OuterAngle
        {
            get
            {
                return (double)GetValue(OuterAngleProperty);
            }

            set
            {
                SetValue(OuterAngleProperty, value);
            }
        }

        public double InnerAngle
        {
            get
            {
                return (double)GetValue(InnerAngleProperty);
            }

            set
            {
                SetValue(InnerAngleProperty, value);
            }
        }

        public int TotalLevels
        {
            get
            {
                return (int)GetValue(TotalLevelsProperty);
            }

            set
            {
                SetValue(TotalLevelsProperty, value);
            }
        }

        public int OuterLevel
        {
            get
            {
                return (int)GetValue(OuterLevelProperty);
            }

            set
            {
                SetValue(OuterLevelProperty, value);
            }
        }

        public int InnerLevel
        {
            get
            {
                return (int)GetValue(InnerLevelProperty);
            }

            set
            {
                SetValue(InnerLevelProperty, value);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            Point c = this.Center;

            var levels = this.TotalLevels <= 0 ? 1 : this.TotalLevels;
            var levelSize = (this.OuterRadius - this.InnerRadius) / levels;

            var outerArcRadius = this.InnerRadius + (this.OuterLevel + 1) * levelSize + this.RadiusPunch;
            var outerSubArcRadius = this.InnerRadius + this.OuterLevel * levelSize + this.RadiusPunch;
            var innerArcRadius = this.InnerRadius + this.InnerLevel * levelSize + this.RadiusPunch;

            var outerArcSize = new Size(outerArcRadius, outerArcRadius);
            var outerArcStart = PolarToCartesian(this.StartAngle, outerArcRadius);
            var outerArcEnd = PolarToCartesian(this.StartAngle + this.OuterAngle, outerArcRadius);

            var outerSubArcSize = new Size(outerSubArcRadius, outerSubArcRadius);
            var outerSubArcStart = PolarToCartesian(this.StartAngle + this.OuterAngle, outerSubArcRadius);
            var outerSubArcEnd = PolarToCartesian(this.StartAngle + this.InnerAngle, outerSubArcRadius);

            var innerArcSize = new Size(innerArcRadius, innerArcRadius);
            var innerArcStart = PolarToCartesian(this.StartAngle + this.InnerAngle, innerArcRadius);
            var innerArcEnd = PolarToCartesian(this.StartAngle, innerArcRadius);

            outerArcStart.Offset(c.X, c.Y);
            outerArcEnd.Offset(c.X, c.Y);
            outerSubArcStart.Offset(c.X, c.Y);
            outerSubArcEnd.Offset(c.X, c.Y);
            innerArcStart.Offset(c.X, c.Y);
            innerArcEnd.Offset(c.X, c.Y);

            context.BeginFigure(innerArcEnd, true, true);
            context.LineTo(outerArcStart, true, true);
            context.ArcTo(outerArcEnd, outerArcSize, 0.0d, this.OuterAngle > 180.0d, this.OuterAngle > 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, true);
            context.LineTo(outerSubArcStart, true, true);
            context.ArcTo(outerSubArcEnd, outerSubArcSize, 0.0d, Math.Abs(this.InnerAngle - this.OuterAngle) > 180.0d, this.InnerAngle > this.OuterAngle ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, true);
            context.LineTo(innerArcStart, true, true);
            context.ArcTo(innerArcEnd, innerArcSize, 0.0d, this.InnerAngle > 180.0d, this.InnerAngle < 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, true);
        }

        private Point PolarToCartesian(double angle, double radius)
        {
            angle = angle * Math.PI / 180.0d;

            return new Point(
                radius * Math.Cos(angle),
                radius * Math.Sin(angle));
        }
    }
}
