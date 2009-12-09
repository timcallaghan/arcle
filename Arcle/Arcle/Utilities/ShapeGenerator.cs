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
            switch (value)
            {
            case 0:
                return new StraightShape(true);
            case 1:
                return new BoxShape(true);
            case 2:
                return new TShape(true);
            case 3:
                return new LeftZigZagShape(true);
            case 4:
                return new RightZigZagShape(true);
            case 5:
                return new LShape(true);
            case 6:
                return new JShape(true);
            default:
                return new BoxShape(true);
            }
        }
    }
}
