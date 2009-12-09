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
using System.Collections.Generic;

namespace Arbaureal.Arcle.Shapes
{
    public class BaseShape
    {
        protected UnitBlock[] m_BlockArray;
        protected Color m_ColourFill;
        protected Color m_ColourOutline = Colors.Black;
        protected RotationState m_RotationState;
        protected double m_StrokeThickness = 3.0;


        protected enum RotationState
        {
            RotationZero,
            RotationOne,
            RotationTwo,
            RotationThree
        };

        public BaseShape()
        {
            m_BlockArray = new UnitBlock[4];
            m_RotationState = RotationState.RotationZero;
        }

        public List<UnitBlock> GetUnitBlocks()
        {
            List<UnitBlock> listBlocks = new List<UnitBlock>(4);

            for (int nIndex = 0; nIndex < 4; ++nIndex)
            {
                listBlocks.Add(m_BlockArray[nIndex]);
            }

            return listBlocks;
        }

        public virtual BaseShape CopyDerivedShape()
        {
            return new BaseShape();
        }

        public BaseShape CreateCopyForHitTesting()
        {
            BaseShape copyOfShape = CopyDerivedShape();

            for (int nIndex = 0; nIndex < m_BlockArray.Length; ++nIndex)
            {
                copyOfShape.m_BlockArray[nIndex] = m_BlockArray[nIndex].CreateCopyForHitTesting();
            }

            copyOfShape.m_RotationState = m_RotationState;

            return copyOfShape;
        }

        public bool Intersects(BaseShape shapeToTest)
        {
            bool fIntersects = false;

            foreach (UnitBlock block in m_BlockArray)
            {
                fIntersects |= shapeToTest.Intersects(block);
            }

            return fIntersects;
        }

        public bool Intersects(UnitBlock blockToTest)
        {
            bool fIntersects = false;

            foreach (UnitBlock block in m_BlockArray)
            {
                fIntersects |= block.Intersects(blockToTest);
            }

            return fIntersects;
        }

        public bool CanMoveOut()
        {
            bool fCanMoveOut = true;

            foreach (UnitBlock block in m_BlockArray)
            {
                fCanMoveOut &= block.CanMoveBlockOut();
            }

            return fCanMoveOut;
        }

        public void MoveOut()
        {
            foreach (UnitBlock block in m_BlockArray)
            {
                block.MoveBlockOut();
            }
        }

        public void MoveIn()
        {
            foreach (UnitBlock block in m_BlockArray)
            {
                block.MoveBlockIn();
            }
        }

        public void MoveRight()
        {
            foreach (UnitBlock block in m_BlockArray)
            {
                block.RotateRight();
            }
        }

        public void MoveLeft()
        {
            foreach (UnitBlock block in m_BlockArray)
            {
                block.RotateLeft();
            }
        }

        public void AddToGameSurface(Canvas gameSurface)
        {
            foreach (UnitBlock block in m_BlockArray)
            {
                gameSurface.Children.Add(block);
            }
        }

        public virtual void Rotate()
        {
        }

        protected void CentreShape(int nMoveLeft, int nMoveIn)
        {
            for (int nIndex = 0; nIndex < nMoveLeft; ++nIndex)
            {
                MoveLeft();
            }

            for (int nIndex = 0; nIndex < nMoveIn; ++nIndex)
            {
                MoveIn();
            }
        }
    }
}
