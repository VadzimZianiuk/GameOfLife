using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Grid
    {
        private readonly int _sizeX;
        private readonly int _sizeY;
        private const byte PixelSize = 5;
        private readonly Cell[][] _cells;
        private readonly Canvas _drawCanvas;
        private readonly Ellipse[][] _cellsVisuals;

        public Grid(Canvas c)
        {
            _drawCanvas = c;
            _sizeX = (int)c.Width / PixelSize;
            _sizeY = (int)c.Height / PixelSize;
            _cells = new Cell[_sizeX][];
            _cellsVisuals = new Ellipse[_sizeX][];

            var rnd = new Random();
            for (int i = 0; i < _sizeX; i++)
            {
                _cells[i] = new Cell[_sizeY];
                _cellsVisuals[i] = new Ellipse[_sizeY];

                for (int j = 0; j < _sizeY; j++)
                {
                    _cells[i][j] = new Cell() { IsAlive = rnd.NextDouble() > 0.8 };

                    var ellipse = new Ellipse()
                    {
                        Width = PixelSize,
                        Height = PixelSize,
                        Margin = new Thickness(i * PixelSize, j * PixelSize, 0, 0),
                        Fill = Brushes.Gray
                    };

                    ellipse.MouseMove += MouseMove;
                    ellipse.MouseLeftButtonDown += MouseMove;
                    _cellsVisuals[i][j] = ellipse;

                    _drawCanvas.Children.Add(ellipse);
                    UpdateGraphics(i, j);
                }
            }

            UpdateNeighbors();
        }

        public void Clear()
        {
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    _cells[i][j].Clear();
                    _cellsVisuals[i][j].Fill = Brushes.Gray;
                }
            }
        }


        void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var cellVisual = sender as Ellipse;
                int i = (int)(cellVisual.Margin.Left / PixelSize);
                int j = (int)(cellVisual.Margin.Top / PixelSize);

                var cell = _cells[i][j];
                if (!cell.IsAlive)
                {
                    cell.IsAlive = true;
                    cell.Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        public void UpdateGraphics(int i, int j)
        {
            var cell = _cells[i][j];
            _cellsVisuals[i][j].Fill = cell.IsAlive
                                                  ? (cell.Age < 2 ? Brushes.White : Brushes.DarkGray)
                                                  : Brushes.Gray;
        }

        public void UpdateToNextGeneration()
        {
            UpdateNeighbors();

            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    var cell = _cells[i][j];
                    if (cell.IsAlive)
                    {
                        if (cell.CountNeighbors == 2 || cell.CountNeighbors == 3)
                        {
                            cell.Age++; 
                        }
                        else
                        {
                            cell.IsAlive = false;
                            cell.Age = 0;
                        }
                    }
                    else if (_cells[i][j].CountNeighbors == 3)
                    {
                        cell.IsAlive = true;
                        cell.Age = 0;
                    }

                    UpdateGraphics(i, j);
                }
            }
        }

        public void UpdateNeighbors()
        {
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    byte count = 0;

                    if (i != _sizeX - 1 && _cells[i + 1][j].IsAlive) count++;
                    if (i != _sizeX - 1 && j != _sizeY - 1 && _cells[i + 1][j + 1].IsAlive) count++;
                    if (j != _sizeY - 1 && _cells[i][j + 1].IsAlive) count++;
                    if (i != 0 && j != _sizeY - 1 && _cells[i - 1][j + 1].IsAlive) count++;
                    if (i != 0 && _cells[i - 1][j].IsAlive) count++;
                    if (i != 0 && j != 0 && _cells[i - 1][j - 1].IsAlive) count++;
                    if (j != 0 && _cells[i][j - 1].IsAlive) count++;
                    if (i != _sizeX - 1 && j != 0 && _cells[i + 1][j - 1].IsAlive) count++;

                    _cells[i][j].CountNeighbors = count;
                }
            }
        }
    }
}