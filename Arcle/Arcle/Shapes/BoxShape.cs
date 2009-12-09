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
    public class BoxShape : BaseShape
    {
        public BoxShape(bool fInitialise) 
        {
            if (fInitialise)
            {
                m_ColourFill = Colors.Cyan;

                m_BlockArray[0] = new UnitBlock
                                    (
                                        Dimensions.OuterRadius - Dimensions.SegmentWidth,
                                        Dimensions.OuterRadius,
                                        0,
                                        m_StrokeThickness,
                                        m_ColourFill,
                                        m_ColourOutline
                                    );
                m_BlockArray[1] = new UnitBlock
                                    (
                                        Dimensions.OuterRadius - Dimensions.SegmentWidth,
                                        Dimensions.OuterRadius,
                                        Dimensions.BaseUnitBlockAngle * 1.0,
                                        m_StrokeThickness,
                                        m_ColourFill,
                                        m_ColourOutline
                                    );
                m_BlockArray[2] = new UnitBlock
                                    (
                                        Dimensions.OuterRadius - 2 * Dimensions.SegmentWidth,
                                        Dimensions.OuterRadius - Dimensions.SegmentWidth,
                                        0,
                                        m_StrokeThickness,
                                        m_ColourFill,
                                        m_ColourOutline
                                    );
                m_BlockArray[3] = new UnitBlock
                                    (
                                        Dimensions.OuterRadius - 2 * Dimensions.SegmentWidth,
                                        Dimensions.OuterRadius - Dimensions.SegmentWidth,
                                        Dimensions.BaseUnitBlockAngle * 1.0,
                                        m_StrokeThickness,
                                        m_ColourFill,
                                        m_ColourOutline
                                    );

                CentreShape(13, 12);
            }
        }

        public override BaseShape CopyDerivedShape()
        {
            return new BoxShape(false);
        }
    }
}
