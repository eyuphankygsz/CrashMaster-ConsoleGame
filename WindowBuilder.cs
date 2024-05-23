using CrashMaster.User;
using System.Numerics;

namespace CrashMaster
{
    internal class WindowBuilder
    {
        public WindowBuilder()
        {
            Instance = this;
        }

        public static WindowBuilder Instance { get; private set; }

        #region Windows
        public void DrawWindow(int width, int height)
        {
            Console.Clear();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1)
                    {
                        Console.Write("-");
                    }
                    else if (j == 0 || j == width - 1)
                    {
                        Console.Write("|");
                    }
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
            UpdateMoney(width, height);
        }
        public void ClearWindow(int width, int height)
        {
            for (int i = 1; i < height - 2; i++)
            {
                Console.SetCursorPosition(1, i);
                for (int j = 1; j < width - 1; j++)
                    Console.Write(" ");

            }
            Console.SetCursorPosition(width, height - 1);

        }
        #endregion
        public void UpdateMoney(int width, int height)
        {
            ClearLine(width, height);
            Console.SetCursorPosition(0, height);
            Console.WriteLine("Money: " + UserStats.Instance.Money);
        }
        #region Texts
        public void ClearWrite(string text, int width, int top, AlignType align)
        {
            ClearLine(width, top);
            switch (align)
            {
                case AlignType.Left:
                    Console.SetCursorPosition(2, top);
                    Console.Write(text);
                    break;
                case AlignType.Right:
                    Console.SetCursorPosition(width - text.Length - 2, top);
                    Console.Write(text);
                    break;
                case AlignType.Center:
                    Console.SetCursorPosition((width / 2) - (text.Length / 2), top);
                    Console.Write(text);
                    break;
            }
        }
        private void ClearLine(int width, int top)
        {
            Console.SetCursorPosition(1, top);
            for (int i = 0; i < width - 2; i++)
                Console.Write(" ");
        }
        public void WriteText(string text, int width, int top, AlignType align)
        {
            switch (align)
            {
                case AlignType.Left:
                    Console.SetCursorPosition(2, top);
                    Console.Write(text);
                    break;
                case AlignType.Right:
                    Console.SetCursorPosition(width - text.Length - 2, top);
                    Console.Write(text);
                    break;
                case AlignType.Center:
                    Console.SetCursorPosition((width / 2) - (text.Length / 2), top);
                    Console.Write(text);
                    break;
            }
        }
        public void WriteText(string[] text, int left, int top, bool center)
        {
            if (!center)
                for (int i = 0; i < text.Length; i++)
                {
                    Console.SetCursorPosition(left, top + i);
                    Console.Write(text[i]);
                }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    Console.SetCursorPosition((left / 2) - (text[i].Length / 2), top + i);
                    Console.Write(text[i]);
                }
            }
        }
        #endregion
        #region Road
        public void DrawRoad(int left, int height)
        {
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(left, i);
                Console.Write("|");
            }
        }
        public void ClearRoad(int left, int top)
        {
            Console.SetCursorPosition(left, top);
            Console.Write("    ");
        }
        #endregion
        #region Obstacles/Points
        public void DrawObstacles(List<Vector2> _positions)
        {
            for (int i = 0; i < _positions.Count; i++)
            {
                int posX = (int)_positions[i].X;
                int posY = (int)_positions[i].Y;
                int oldPosY = (posY - 1) < 1 ? 1 : posY - 1;

                ClearRoad(posX, oldPosY);

                Console.SetCursorPosition(posX, posY);
                Console.Write("████");
            }
        }
        public void DrawPoints(List<Vector2> _positions)
        {
            for (int i = 0; i < _positions.Count; i++)
            {
                int posX = (int)_positions[i].X;
                int posY = (int)_positions[i].Y;
                int oldPosY = (posY - 1) < 1 ? 1 : posY - 1;

                ClearRoad(posX, oldPosY);

                Console.SetCursorPosition(posX, posY);
                Console.Write(" ºº ");
            }
        }
        public void ClearPoint(Vector2 pointPos, Vector2 carPos)
        {

            if (pointPos.Y == carPos.Y - 3)
            {
                Console.SetCursorPosition((int)pointPos.X, (int)pointPos.Y);
                Console.Write("    ");
            }

        }
        #endregion
        #region Car
        public void DrawCar(string[] carDrawing, Vector2 pos)
        {
            for (int i = 2; i >= 0; i--)
            {
                Console.SetCursorPosition((int)pos.X, (int)pos.Y - i);
                Console.Write(carDrawing[2 - i]);
            }

        }
        public void ClearCar(Vector2 pos)
        {
            for (int i = 2; i >= 0; i--)
            {
                Console.SetCursorPosition((int)pos.X, (int)pos.Y - i);
                Console.Write("    ");
            }
        }
        public void DisplayCurrentStats(Car car, int width)
        {
            Console.SetCursorPosition(width, 0);
            Console.Write("Car: " + car.GetName());
            Console.SetCursorPosition(width, 1);
            Console.Write("                     ");
            Console.SetCursorPosition(width, 1);
            Console.Write("CurrentSpeed: " + car.GetCurrentSpeed() + "/" + car.GetMaxSpeed());
            Console.SetCursorPosition(width, 2);
            Console.Write("High Score: " + UserStats.Instance.HighScore);
            Console.SetCursorPosition(width, 3);
            Console.Write("Score: " + UserStats.Instance.CurrentScore);


            Console.SetCursorPosition(width, 8);
            Console.Write("CONTROLS: ");
            Console.SetCursorPosition(width, 9);
            Console.Write("W: Speed UP");
            Console.SetCursorPosition(width, 10);
            Console.Write("S: Speed DOWN");
            Console.SetCursorPosition(width, 11);
            Console.Write("A: LEFT");
            Console.SetCursorPosition(width, 12);
            Console.Write("D: RIGHT");

            Console.SetCursorPosition(width, 16);
            Console.Write("ENVIRONMENT: ");
            Console.SetCursorPosition(width, 17);
            Console.Write(" ºº : Points | Increased by speed");
            Console.SetCursorPosition(width, 18);
            Console.Write("████: Obstacles | Hit and die");

        }
        #endregion

        public void GameOver(int width, int height)
        {
            ClearWindow(width, height);
            WriteText("|--GAME OVER--|", width, 10, AlignType.Center);
        }
    }
    public enum AlignType
    {
        Left,
        Center,
        Right
    }
}
