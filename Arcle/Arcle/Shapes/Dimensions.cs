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
    public class Dimensions
    {
        private static int m_numberOfUnitsPerCircle = 48;
        private static double m_baseUnitBlockAngle = 360.0 / (double)m_numberOfUnitsPerCircle;
        private static int m_OuterRadius = 290;
        private static int m_InnerRadius = 50;
        private static int m_SegmentWidth = 20;

        public static int NumberOfUnitsPerCircle
        {
            get
            {
                return m_numberOfUnitsPerCircle;
            }
        }

        public static double BaseUnitBlockAngle
        {
            get
            {
                return m_baseUnitBlockAngle;
            }
        }

        public static int SegmentWidth
        {
            get
            {
                return m_SegmentWidth;
            }
        }

        public static int OuterRadius
        {
            get
            {
                return m_OuterRadius;
            }
        }

        public static int InnerRadius
        {
            get
            {
                return m_InnerRadius;
            }
        }
    }
}
