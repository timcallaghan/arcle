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

using Arbaureal.Arcle.Shapes;
using Arbaureal.Arcle.Utilities;
using System.Windows.Navigation;

namespace Arbaureal.Arcle.Engine
{
    public class Game
    {
        private CompositionTargetGameLoop m_GameLoop;
        private BaseShape m_currentShape;
        private Canvas m_gameSurface;
        private SoundBufferArray m_Sounds;
        private TimeSpan m_ElapsedGameTime;
        private TimeSpan m_ShapeFallInterval;
        private List<UnitBlock> m_SettledBlocks;
        private NavigationService m_NavigationService;
        private YesNoPopup m_QuitDialog;
        private int m_Score;

        public delegate void ScoreUpdateHandler(object sender, int nScore);
        public event ScoreUpdateHandler ScoreUpdateEvent;

        public Game(Canvas gameSurface, Panel LayoutRoot)
        {
            m_gameSurface = gameSurface;

            m_Sounds = new SoundBufferArray(new Uri("/Arcle;component/Sounds/Shift_Rings.mp3", UriKind.Relative),10,LayoutRoot);

            m_GameLoop = new CompositionTargetGameLoop();
            m_GameLoop.Update += new CompositionTargetGameLoop.UpdateHandler(gameLoop_Update);

            m_ElapsedGameTime = new TimeSpan(0, 0, 0);
            m_ShapeFallInterval = new TimeSpan(0, 0, 0, 0, 500);

            GridGenerator.AddGridToCanvas
                (
                    m_gameSurface, 
                    Dimensions.OuterRadius, 
                    Dimensions.InnerRadius, 
                    Dimensions.SegmentWidth, 
                    Dimensions.NumberOfUnitsPerCircle
                );

            m_currentShape = ShapeGenerator.GetNextShape();
            m_currentShape.AddToGameSurface(m_gameSurface);

            m_SettledBlocks = new List<UnitBlock>();

            m_QuitDialog = new YesNoPopup();
            m_QuitDialog.Title = "Quit Current Game?";
            m_QuitDialog.Closed += new EventHandler(quitDialog_Closed);

            m_Score = 0;
        }

        public NavigationService NavigationService
        {
            set
            {
                m_NavigationService = value;
            }
        }

        public void StartGame()
        {
            m_Score = 0;
            m_GameLoop.Start();
        }

        public void StopGame()
        {
            m_GameLoop.Stop();

            // Transition to high score table
        }

        public void PauseGame()
        {
            m_GameLoop.Stop();
        }

        public void ResumeGame()
        {
            m_GameLoop.Start();
        }

        void gameLoop_Update(object sender, TimeSpan elapsed)
        {
            m_ElapsedGameTime += elapsed;

            if (m_ElapsedGameTime > m_ShapeFallInterval)
            {
                m_ElapsedGameTime -= m_ShapeFallInterval;

                if (CanMoveOut())
                {
                    m_currentShape.MoveOut();
                }
                else
                {
                    m_SettledBlocks.AddRange(m_currentShape.GetUnitBlocks());

                    ClearCompletedRows();

                    m_currentShape = ShapeGenerator.GetNextShape();
                    m_currentShape.AddToGameSurface(m_gameSurface);
                    
                    if (!m_currentShape.CanMoveOut())
                    {
                        // This is the end of the game!
                    }
                }
            }
        }

        void ClearCompletedRows()
        {
            List<int> listBlockIndices = new List<int>(Dimensions.NumberOfUnitsPerCircle);
            int nRowsCompleted = 0;
            for (int nRadius = Dimensions.InnerRadius; nRadius < Dimensions.OuterRadius; nRadius += Dimensions.SegmentWidth)
            {
                listBlockIndices.Clear();

                bool fHasCompleteRow = true;
                for 
                (
                    int nCurrentSegment = 0; 
                    nCurrentSegment < Dimensions.NumberOfUnitsPerCircle
                    &&
                    fHasCompleteRow; 
                    ++nCurrentSegment
                )
                {
                    double angleToTest = Dimensions.BaseUnitBlockAngle / 2.0 + (double)(nCurrentSegment) * Dimensions.BaseUnitBlockAngle;

                    if (angleToTest > 180.0)
                    {
                        angleToTest -= 360.0;
                    }

                    bool fFoundBlockAtPosition = false;

                    for (int nIndex = 0; nIndex < m_SettledBlocks.Count; ++nIndex)
                    {
                        if (m_SettledBlocks[nIndex].Radius == nRadius && m_SettledBlocks[nIndex].Angle == angleToTest)
                        {
                            fFoundBlockAtPosition = true;
                            listBlockIndices.Add(nIndex);
                            break;
                        }
                    }

                    fHasCompleteRow &= fFoundBlockAtPosition;
                }

                if (fHasCompleteRow)
                {
                    ++nRowsCompleted;
                    listBlockIndices.Sort();

                    for (int nIndexIntoArray = listBlockIndices.Count - 1; nIndexIntoArray >= 0; --nIndexIntoArray)
                    {
                        int nIndex = listBlockIndices[nIndexIntoArray];

                        m_gameSurface.Children.Remove(m_SettledBlocks[nIndex]);
                        m_SettledBlocks.RemoveAt(nIndex);
                    }

                    foreach (UnitBlock block in m_SettledBlocks)
                    {
                        if (block.Radius < nRadius)
                        {
                            block.MoveBlockOut();
                        }
                    }
                }
            }

            UpdateScore(nRowsCompleted);
        }

        private void UpdateScore(int nRowsCompleted)
        {
            m_Score += nRowsCompleted;

            if (ScoreUpdateEvent != null)
            {
                ScoreUpdateEvent(this, m_Score);
            }
        }

        bool CanMoveOut()
        {
            bool fCanMoveOut = false;

            if (m_currentShape.CanMoveOut())
            {
                fCanMoveOut = true;

                BaseShape copyForHitTest = m_currentShape.CreateCopyForHitTesting();
                copyForHitTest.MoveOut();

                foreach (UnitBlock block in m_SettledBlocks)
                {
                    if (copyForHitTest.Intersects(block))
                    {
                        fCanMoveOut = false;
                    }
                }
            }

            return fCanMoveOut;
        }

        bool CanMoveLeft()
        {
            bool fCanMoveLeft = true;

            BaseShape copyForHitTest = m_currentShape.CreateCopyForHitTesting();
            copyForHitTest.MoveLeft();

            foreach (UnitBlock block in m_SettledBlocks)
            {
                if (copyForHitTest.Intersects(block))
                {
                    fCanMoveLeft = false;
                }
            }

            return fCanMoveLeft;
        }

        bool CanMoveRight()
        {
            bool fCanMoveRight = true;

            BaseShape copyForHitTest = m_currentShape.CreateCopyForHitTesting();
            copyForHitTest.MoveRight();

            foreach (UnitBlock block in m_SettledBlocks)
            {
                if (copyForHitTest.Intersects(block))
                {
                    fCanMoveRight = false;
                }
            }

            return fCanMoveRight;
        }

        bool CanRotate()
        {
            bool fCanRotate = true;

            BaseShape copyForHitTest = m_currentShape.CreateCopyForHitTesting();
            copyForHitTest.Rotate();

            foreach (UnitBlock block in m_SettledBlocks)
            {
                if (copyForHitTest.Intersects(block))
                {
                    fCanRotate = false;
                }
            }

            return fCanRotate;
        }

        public void KeyDownEventHandler(object sender, KeyEventArgs e)
        {

            // do your game loop processing here
            if ((e.Key == Key.A) || (e.Key == Key.Left))
            {
                if (CanMoveLeft())
                {
                    m_currentShape.MoveLeft();
                    m_Sounds.PlaySoundFromBuffer();
                }
            }
            else if ((e.Key == Key.D) || (e.Key == Key.Right))
            {
                if (CanMoveRight())
                {
                    m_currentShape.MoveRight();
                    m_Sounds.PlaySoundFromBuffer();
                }
            }
            else if ((e.Key == Key.W) || (e.Key == Key.Up))
            {
                if (CanRotate())
                {
                    m_currentShape.Rotate();
                    m_Sounds.PlaySoundFromBuffer();
                }
            }
            else if ((e.Key == Key.S) || (e.Key == Key.Down))
            {
                if (CanMoveOut())
                {
                    m_currentShape.MoveOut();
                    m_Sounds.PlaySoundFromBuffer();
                }
            }
            else if (e.Key == Key.Space)
            {
                while (CanMoveOut())
                {
                    m_currentShape.MoveOut();
                }
            }
            else if (e.Key == Key.Escape)
            {
                PauseGame();                 
                m_QuitDialog.Show();
            }
        }

        void quitDialog_Closed(object sender, EventArgs e)
        {
            if (m_QuitDialog.DialogResult == true)
            {             
                m_NavigationService.Source = new Uri("/Menu", UriKind.Relative);
            }
            else
            {
                ResumeGame();                
            }
        } 
    }
}
