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

using Arbaureal.Arcle.Shapes;

namespace Arbaureal.Arcle.Utilities
{
    public class ShapeGenerator
    {
        private static Random m_random;

        static ShapeGenerator()
        {
            m_random = new Random(DateTime.Now.Millisecond);
        }

        public static BaseShape GetNextShape()
        {
            int value = m_random.Next(7);
            int angle = m_random.Next(Dimensions.NumberOfUnitsPerCircle - 1);
            switch (value)
            {
            case 0:
                return new StraightShape(true, angle);
            case 1:
                return new BoxShape(true, angle);
            case 2:
                return new TShape(true, angle);
            case 3:
                return new LeftZigZagShape(true, angle);
            case 4:
                return new RightZigZagShape(true, angle);
            case 5:
                return new LShape(true, angle);
            case 6:
                return new JShape(true, angle);
            default:
                return new BoxShape(true, angle);
            }
        }
    }
}
