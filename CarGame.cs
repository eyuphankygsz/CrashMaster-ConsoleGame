using CrashMaster.User;
using System.Diagnostics;
using System.Numerics;

namespace CrashMaster
{
    internal class CarGame
    {
        private const int WIDTH = 16;
        private const int HEIGHT = 28;

        private Car _selectedCar;
        private MainMenu _mainMenu;

        private bool _end = false;
        private readonly Stopwatch _speedWatch = new Stopwatch();
        private readonly Stopwatch _controllerWatch = new Stopwatch();

        private string[] _carDrawing = new string[]
        {
            "÷  ÷",
            "████",
            "*^^*"
        };

        private int _obstacleCurrentRow;
        private int _obstacleGap = 8;
        private List<Vector2> _obstaclePositions;


        private int _pointCurrentRow;
        private int _pointGap = 5;
        private List<Vector2> _pointPositions;
        private Vector2 _lastPointPos;

        private Vector2 _currentCarPos, _oldCarPos;
        private int _carLane = 1;

        Random rand = new Random();
        public void PrepareGame()
        {
            new WindowBuilder();
            new CarFactory();
            Main();
        }

        public void Start()
        {
            _end = false;

            _obstacleCurrentRow = HEIGHT - 12;
            _obstaclePositions = new List<Vector2>();

            _pointCurrentRow = HEIGHT - 8;
            _pointPositions = new List<Vector2>();

            _selectedCar = UserStats.Instance.SelectedCar;
            _selectedCar.CurrentSpeed = 0;

            _carLane = 1;
            _currentCarPos = new Vector2(((WIDTH / 3) * _carLane) + 1, HEIGHT - 1);
            _oldCarPos = _currentCarPos;


            PrintFrame();
            CreateRoads(2);

            _speedWatch.Start();
            _controllerWatch.Start();
            float gameLoopTime = 200;
            float controllerTime = 10;

            WindowBuilder.Instance.DrawCar(_carDrawing, _currentCarPos);

            char keyChar = '0';
            while (!_end)
            {
                float speed = _selectedCar.GetCurrentSpeed();
                if (speed != 0)
                    gameLoopTime = 8000 / speed;

                if (Console.KeyAvailable)
                    keyChar = Console.ReadKey(true).KeyChar;

                if (_controllerWatch.ElapsedMilliseconds >= controllerTime)
                {
                    MoveCar(keyChar);
                    CheckHits();
                    _controllerWatch.Restart();
                    keyChar = '0';
                }
                if (speed == 0) continue;
                if (_speedWatch.ElapsedMilliseconds >= gameLoopTime)
                {
                    _obstacleCurrentRow++;
                    MoveEnvironment();
                    WindowBuilder.Instance.DrawPoints(_pointPositions);
                    WindowBuilder.Instance.DrawObstacles(_obstaclePositions);
                    CreateObstacles(false);
                    _speedWatch.Restart();
                }
            }
            UserStats.Instance.EarnMoney((int)MathF.Ceiling(UserStats.Instance.CurrentScore / 20f));
            UserStats.Instance.TryHighScore();
            WindowBuilder.Instance.GameOver(WIDTH, HEIGHT);

            UserStats.Instance.CurrentScore = 0;
            Console.ReadKey(true);

        }


        private void MoveCar(char keyChar)
        {
            DrawCar();

            switch (char.ToUpper(keyChar))
            {
                case 'A':
                    _carLane--;
                    if (_carLane < 0) _carLane = 2;
                    break;
                case 'D':
                    _carLane++;
                    break;
                case 'W':
                    _selectedCar.Accelerate(faster: true);
                    break;
                case 'S':
                    _selectedCar.Accelerate(faster: false);
                    break;
                default:
                    break;
            }
            WindowBuilder.Instance.DisplayCurrentStats(_selectedCar, WIDTH);
            _carLane %= 3;
            _currentCarPos = new Vector2(((WIDTH / 3) * _carLane) + 1, HEIGHT - 1);
        }

       private void DrawCar()
        {
            if (_currentCarPos != _oldCarPos)
            {
                WindowBuilder.Instance.ClearCar(_oldCarPos);
                WindowBuilder.Instance.DrawCar(_carDrawing, _currentCarPos);
                _oldCarPos = _currentCarPos;
            }
        }
        void PrintFrame()
        {
            WindowBuilder.Instance.DrawWindow(WIDTH, HEIGHT);
        }

        private void CreateRoads(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int pos = (WIDTH / 3) * (i + 1);
                WindowBuilder.Instance.DrawRoad(pos, HEIGHT);
            }
            CreatePoints(true);
            CreateObstacles(true);
        }
        private void CreatePoints(bool start)
        {
            DestroyOuts(_pointPositions);
            if (start)
            {
                while (_pointCurrentRow != 1)
                {
                    int luck = rand.Next(0, 101);
                    if (luck < 90)
                        continue;

                    int pos = rand.Next(0, 3);
                    int left = pos * WIDTH / 3;

                    Vector2 newPoint = new Vector2(left + 1, _pointCurrentRow);
                    _pointPositions.Add(newPoint);
                    _pointCurrentRow = _pointCurrentRow - _pointGap < 1 ? 1 : _pointCurrentRow - _pointGap;

                    _lastPointPos = newPoint;
                }
            }
            else if (_pointCurrentRow == _pointGap || (_lastPointPos.Y > 3))
            {
                int luck = rand.Next(0, 101);
                if (luck < 90)
                    return;

                int pos = rand.Next(0, 3);
                int left = pos * WIDTH / 3;

                Vector2 newPoint = new Vector2(left + 1, _pointCurrentRow);
                _pointPositions.Add(newPoint);
                _pointCurrentRow = _pointCurrentRow - _pointGap < 1 ? 1 : _pointCurrentRow - _pointGap;

                _lastPointPos = newPoint;
            }
        }
        private void CreateObstacles(bool start)
        {
            CreatePoints(false);

            if (start)
            {
                while (_obstacleCurrentRow != 1)
                {
                    int obstacleCount = rand.Next(1, 3);

                    for (int i = 0; i < obstacleCount; i++)
                    {
                        int pos = rand.Next(0, 3);
                        int left = pos * WIDTH / 3;
                        _obstaclePositions.Add(new Vector2(left + 1, _obstacleCurrentRow));
                    }
                    _obstacleCurrentRow = _obstacleCurrentRow - _obstacleGap < 1 ? 1 : _obstacleCurrentRow - _obstacleGap;
                }
            }
            else if (_obstacleCurrentRow == _obstacleGap)
            {
                _obstacleCurrentRow = 1;

                int obstacleCount = rand.Next(1, 3);

                for (int i = 0; i < obstacleCount; i++)
                {
                    int pos = rand.Next(0, 3);
                    int left = pos * WIDTH / 3;
                    _obstaclePositions.Add(new Vector2(left + 1, _obstacleCurrentRow));
                }
            }
            DestroyOuts(_obstaclePositions);
        }
        private void DestroyOuts(List<Vector2> _positions)
        {
            List<Vector2> indexes = new List<Vector2>();

            for (int i = 0; i < _positions.Count; i++)
                if (_positions[i].Y >= HEIGHT - 1)
                    indexes.Add(_positions[i]);

            for (int i = 0; i < indexes.Count; i++)
            {
                WindowBuilder.Instance.ClearRoad((int)indexes[i].X, (int)indexes[i].Y);
                _positions.Remove(indexes[i]);
            }
        }
        private void MoveEnvironment()
        {
            for (int i = 0; i < _obstaclePositions.Count; i++)
                _obstaclePositions[i] = new Vector2(_obstaclePositions[i].X, _obstaclePositions[i].Y + 1);

            for (int i = 0; i < _pointPositions.Count; i++)
                _pointPositions[i] = new Vector2(_pointPositions[i].X, _pointPositions[i].Y + 1);
            _lastPointPos = new Vector2(_lastPointPos.X, _lastPointPos.Y + 1);
        }
        private void CheckHits()
        {
            for (int i = 0; i < _obstaclePositions.Count; i++)
            {
                if (_obstaclePositions[i].X == _currentCarPos.X)
                    if (_currentCarPos.Y - 2 <= _obstaclePositions[i].Y && _currentCarPos.Y >= _obstaclePositions[i].Y)
                    {
                        _end = true;
                        return;
                    }
            }
            for (int i = 0; i < _pointPositions.Count; i++)
            {
                if (_pointPositions[i].X == _currentCarPos.X)
                    if (_currentCarPos.Y - 3 <= _pointPositions[i].Y && _currentCarPos.Y >= _pointPositions[i].Y)
                    {
                        UserStats.Instance.CurrentScore += 2 * _selectedCar.CurrentSpeed ;
                        WindowBuilder.Instance.ClearPoint(_pointPositions[i], _currentCarPos);
                        _pointPositions.RemoveAt(i);
                        return;
                    }
            }
        }
        private void Main()
        {
            if (_mainMenu == null)
                _mainMenu = new MainMenu(this);

            _mainMenu.PrintMenu();

        }
    }
}
