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

using Arbaureal.Arcle.Utilities;

namespace Arbaureal.Arcle.Shapes
{
    public class UnitBlock : UserControl
    {
        private RotateTransform m_rotateTransform;
        private TranslateTransform m_translateTransform;
        private ScaleTransform m_scaleTransform;

        public Storyboard m_StoryBoard;
        private DoubleAnimation m_DoubleAnimation;

        int m_currentInnerRadius;

        public int Radius
        {
            get
            {
                return m_currentInnerRadius;
            }
        }

        public double Angle
        {
            get
            {
                return m_rotateTransform.Angle;
            }

        }

        public UnitBlock
        (
            double innerRadius,
            double outerRadius,
            double rotation,
            double strokeThickness,
            Color fillColour,
            Color strokeColor
        )
        {
            m_currentInnerRadius = (int)(innerRadius);

            double centreAngle = Dimensions.BaseUnitBlockAngle / 2.0;

            Point startPoint = new Point();
            startPoint.X = Math.Cos(MathHelpers.DegreesToRadians(-centreAngle));
            startPoint.Y = Math.Sin(MathHelpers.DegreesToRadians(-centreAngle));

            Point endPoint = new Point();
            endPoint.X = Math.Cos(MathHelpers.DegreesToRadians(centreAngle));
            endPoint.Y = Math.Sin(MathHelpers.DegreesToRadians(centreAngle));

            TransformGroup transformGroup = new TransformGroup();

            //double centreRadius = outerRadius * startPoint.X;// +(outerRadius - innerRadius) / 2.0;

            //Point centreOfBlock = new Point();
            //centreOfBlock.X = outerRadius * startPoint.X;
            //centreOfBlock.Y = 0.0;

            m_scaleTransform = new ScaleTransform();
            //double innerMostPoint = innerRadius* startPoint.X;
            //scaleTransform.CenterX = innerMostPoint + (outerRadius - innerMostPoint) / 2.0;
            //scaleTransform.CenterY = centreOfBlock.Y;
            m_translateTransform = new TranslateTransform();
            m_rotateTransform = new RotateTransform();
            m_rotateTransform.Angle = rotation + centreAngle;

            transformGroup.Children.Add(m_scaleTransform);
            transformGroup.Children.Add(m_translateTransform);
            transformGroup.Children.Add(m_rotateTransform);
            this.RenderTransform = transformGroup;

            RenderContent
                (
                    fillColour, 
                    strokeColor, 
                    strokeThickness,
                    innerRadius,
                    outerRadius,
                    startPoint,
                    endPoint
                );

            //InitStoryBoard();
        }

        public void InitStoryBoard()
        {
            m_StoryBoard = new Storyboard();
            m_DoubleAnimation = new DoubleAnimation();
            m_DoubleAnimation.From = 1.0;
            m_DoubleAnimation.To = 0.5;
            Duration duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            m_DoubleAnimation.Duration = duration;
            m_DoubleAnimation.AutoReverse = true;

            m_StoryBoard.Duration = duration;
            m_StoryBoard.AutoReverse = true;
            m_StoryBoard.RepeatBehavior = RepeatBehavior.Forever;
            m_StoryBoard.Children.Add(m_DoubleAnimation);
            Storyboard.SetTarget(m_DoubleAnimation, this.Content);
            Storyboard.SetTargetProperty(m_DoubleAnimation, new PropertyPath("(Opacity)"));
            
            m_StoryBoard.Begin();
        }

        private UnitBlock()
        {
        }

        public UnitBlock CreateCopyForHitTesting()
        {
            UnitBlock copyOfBlock = new UnitBlock();

            copyOfBlock.m_scaleTransform = new ScaleTransform();
            copyOfBlock.m_scaleTransform.ScaleX = m_scaleTransform.ScaleX;
            copyOfBlock.m_scaleTransform.ScaleY = m_scaleTransform.ScaleY;
            copyOfBlock.m_scaleTransform.CenterX = m_scaleTransform.CenterX;
            copyOfBlock.m_scaleTransform.CenterY = m_scaleTransform.CenterY;

            copyOfBlock.m_translateTransform = new TranslateTransform();
            copyOfBlock.m_translateTransform.X = m_translateTransform.X;
            copyOfBlock.m_translateTransform.Y = m_translateTransform.Y;

            copyOfBlock.m_rotateTransform = new RotateTransform();
            copyOfBlock.m_rotateTransform.Angle = m_rotateTransform.Angle;
            copyOfBlock.m_rotateTransform.CenterX = m_rotateTransform.CenterX;
            copyOfBlock.m_rotateTransform.CenterY = m_rotateTransform.CenterY;

            copyOfBlock.m_currentInnerRadius = m_currentInnerRadius;

            return copyOfBlock;
        }

        public void RenderContent
        (
            Color fillColour, 
            Color strokeColor, 
            double strokeThickness,
            double innerRadius,
            double outerRadius,
            Point startPoint,
            Point endPoint
        )
        {
            Point pathStartPoint = new Point(innerRadius * startPoint.X, innerRadius * startPoint.Y);

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = pathStartPoint;

            LineSegment lineSeg1 = new LineSegment();
            lineSeg1.Point = new Point(outerRadius * startPoint.X, outerRadius * startPoint.Y);

            pathFigure.Segments.Add(lineSeg1);

            ArcSegment arcSeg1 = new ArcSegment();
            arcSeg1.Size = new Size(outerRadius, outerRadius);
            arcSeg1.IsLargeArc = false;
            arcSeg1.SweepDirection = SweepDirection.Clockwise;
            arcSeg1.Point = new Point(outerRadius * endPoint.X, outerRadius * endPoint.Y);

            pathFigure.Segments.Add(arcSeg1);

            LineSegment lineSeg2 = new LineSegment();
            lineSeg2.Point = new Point(innerRadius * endPoint.X, innerRadius * endPoint.Y);

            pathFigure.Segments.Add(lineSeg2);

            ArcSegment arcSeg2 = new ArcSegment();
            arcSeg2.Size = new Size(innerRadius, innerRadius);
            arcSeg2.IsLargeArc = false;
            arcSeg2.SweepDirection = SweepDirection.Counterclockwise;
            arcSeg2.Point = new Point(innerRadius * startPoint.X, innerRadius * startPoint.Y);

            pathFigure.Segments.Add(arcSeg2);

            PathGeometry pathGeom = new PathGeometry();
            pathGeom.Figures.Add(pathFigure);

            Path path = new Path();
            path.Stroke = new SolidColorBrush(strokeColor);
            path.StrokeThickness = strokeThickness;
            path.Fill = new SolidColorBrush(fillColour);
            path.Data = pathGeom;

            this.Content = path;   
        }

        public void RotateLeft()
        {
            double angle = m_rotateTransform.Angle - Dimensions.BaseUnitBlockAngle;
            if (angle < -180.0)
            {
                angle += 360.0;
            }

            m_rotateTransform.Angle = angle;
        }

        public void RotateRight()
        {
            double angle = m_rotateTransform.Angle + Dimensions.BaseUnitBlockAngle;
            if (angle > 180.0)
            {
                angle -= 360.0;
            }

            m_rotateTransform.Angle = angle;
        }

        public void MoveBlockOut()
        {
            m_currentInnerRadius += Dimensions.SegmentWidth;
            //scaleTransform.CenterX += Dimensions.SegmentWidth;
            m_scaleTransform.ScaleY *= (1.0 + (double)(Dimensions.SegmentWidth) / m_currentInnerRadius);
            m_translateTransform.X += Dimensions.SegmentWidth;
        }

        public void MoveBlockIn()
        {
            m_scaleTransform.ScaleY *= m_currentInnerRadius / (m_currentInnerRadius + (double)(Dimensions.SegmentWidth));
            //scaleTransform.CenterX -= Dimensions.SegmentWidth;
            m_currentInnerRadius -= Dimensions.SegmentWidth;
            m_translateTransform.X -= Dimensions.SegmentWidth;
        }

        public bool CanMoveBlockOut()
        {
            if (m_currentInnerRadius < (Dimensions.OuterRadius - Dimensions.SegmentWidth))
            {
                return true;
            }

            return false;
        }

        public bool Intersects(UnitBlock blockToTest)
        {
            return (blockToTest.Radius == Radius && blockToTest.Angle == Angle);
        }
    }
}
