using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Arbaureal.Arcle.Utilities
{
    public class GridGenerator
    {
        public static void AddGridToCanvas
        (
            Canvas gameSurface,
            int outerRadius,
            int innerRadius,
            int segmentWidth,
            int numberOfSegmentsPerCircle
        )
        {
            Color gridColor = Colors.Gray;
            double gridThickness = 2.0;

            for (int nRadius = innerRadius; nRadius <= outerRadius; nRadius += segmentWidth)
            {
                TranslateTransform translateTransform = new TranslateTransform();
                translateTransform.X = -nRadius;
                translateTransform.Y = -nRadius;

                Ellipse currentCircle = new Ellipse();
                currentCircle.RenderTransform = translateTransform;
                currentCircle.Width = nRadius*2;
                currentCircle.Height = nRadius*2;
                currentCircle.Stroke = new SolidColorBrush(gridColor);
                currentCircle.StrokeThickness = gridThickness;

                gameSurface.Children.Add(currentCircle);
            }

            double segmentAngle = 360.0 / (double)(numberOfSegmentsPerCircle);

            for (int nCurrentSegment = 0; nCurrentSegment < numberOfSegmentsPerCircle; ++nCurrentSegment)
            {
                double currentAngle = nCurrentSegment * segmentAngle;


                Line currentLine = new Line();
                currentLine.X1 = 0.0 * Math.Cos(MathHelpers.DegreesToRadians(currentAngle));
                currentLine.Y1 = 0.0 * Math.Sin(MathHelpers.DegreesToRadians(currentAngle));
                currentLine.X2 = outerRadius * Math.Cos(MathHelpers.DegreesToRadians(currentAngle));
                currentLine.Y2 = outerRadius * Math.Sin(MathHelpers.DegreesToRadians(currentAngle));
                currentLine.Stroke = new SolidColorBrush(gridColor);
                currentLine.StrokeThickness = gridThickness;

                gameSurface.Children.Add(currentLine);
            }
        }
    }
}
