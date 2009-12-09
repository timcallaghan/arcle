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

namespace Arbaureal.Arcle.Shapes
{
    public class RightZigZagShape : BaseShape
    {
        public RightZigZagShape(bool fInitialise) 
        {
            if (fInitialise)
            {
                m_ColourFill = Colors.Green;

                m_BlockArray[0] = new UnitBlock
                                    (
                                        Dimensions.OuterRadius - 2 * Dimensions.SegmentWidth,
                                        Dimensions.OuterRadius - Dimensions.SegmentWidth,
                                        0,
                                        m_StrokeThickness,
                                        m_ColourFill,
                                        m_ColourOutline
                                    );
                m_BlockArray[1] = new UnitBlock
                                    (
                                        Dimensions.OuterRadius - 2 * Dimensions.SegmentWidth,
                                        Dimensions.OuterRadius - Dimensions.SegmentWidth,
                                        Dimensions.BaseUnitBlockAngle * 1.0,
                                        m_StrokeThickness,
                                        m_ColourFill,
                                        m_ColourOutline
                                    );
                m_BlockArray[2] = new UnitBlock
                                    (
                                        Dimensions.OuterRadius - Dimensions.SegmentWidth,
                                        Dimensions.OuterRadius,
                                        Dimensions.BaseUnitBlockAngle * 1.0,
                                        m_StrokeThickness,
                                        m_ColourFill,
                                        m_ColourOutline
                                    );
                m_BlockArray[3] = new UnitBlock
                                    (
                                        Dimensions.OuterRadius - Dimensions.SegmentWidth,
                                        Dimensions.OuterRadius,
                                        Dimensions.BaseUnitBlockAngle * 2.0,
                                        m_StrokeThickness,
                                        m_ColourFill,
                                        m_ColourOutline
                                    );

                CentreShape(13, 12);
            }
        }

        public override BaseShape CopyDerivedShape()
        {
            return new RightZigZagShape(false);
        }

        public override void Rotate()
        {
            switch (m_RotationState)
            {
                case RotationState.RotationZero:
                    m_BlockArray[0].MoveBlockOut();
                    m_BlockArray[0].MoveBlockOut();

                    m_BlockArray[3].RotateLeft();
                    m_BlockArray[3].RotateLeft();

                    m_RotationState = RotationState.RotationOne;
                    break;
                case RotationState.RotationOne:
                    m_BlockArray[0].MoveBlockIn();
                    m_BlockArray[0].MoveBlockIn();

                    m_BlockArray[3].RotateRight();
                    m_BlockArray[3].RotateRight();

                    m_RotationState = RotationState.RotationZero;
                    break;
            }
        }
    }
}
