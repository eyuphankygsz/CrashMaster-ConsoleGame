using CrashMaster.User;
using System.Diagnostics;

namespace CrashMaster
{
    internal class CarSelection
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private const int WIDTH = 50;
        private const int HEIGHT = 28;
        private MainMenu _mainMenu;

        string[] _selectionKeys = { "Q", "E", "A", "D" };


        private Car[] _cars;
        private Car _selectedCar;
        private int _currentCarIndex;
        private string _warning;
        public CarSelection(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
        }
        public void SelectWindow()
        {
            PrintFrame();
            GetCars();
            PrintCar();
            Controls();
        }
        private void GetCars()
        {
            if (_cars == null)
                _cars = CarFactory.Instance.GetCars();
        }
        private void PrintCar()
        {
            PrintCarStats();
            WindowBuilder.Instance.UpdateMoney(WIDTH, HEIGHT);
        }
        private void PrintCarStats()
        {
            WindowBuilder.Instance.ClearWindow(WIDTH, HEIGHT);
            _selectedCar = _cars[_currentCarIndex];

            WindowBuilder.Instance.WriteText(_selectedCar.GetLogo(), WIDTH, 1, true);
            WindowBuilder.Instance.WriteText(_selectedCar.GetName(), WIDTH, 8, AlignType.Center);

            WindowBuilder.Instance.WriteText("Stats:", WIDTH, 13, AlignType.Left);
            WindowBuilder.Instance.WriteText($"Max Speed: {_selectedCar.GetMaxSpeed()}", WIDTH, 14, AlignType.Left);
            WindowBuilder.Instance.WriteText($"Acceleration Speed: {_selectedCar.GetCarAccelerationSpeed()}", WIDTH, 15, AlignType.Left);

            WindowBuilder.Instance.WriteText($"Press A to LEFT", WIDTH, 20, AlignType.Left);
            WindowBuilder.Instance.WriteText($"Press D to RIGHT", WIDTH, 20, AlignType.Right);

            if (!UserStats.Instance.OwnedCars[_currentCarIndex])
            {
                WindowBuilder.Instance.WriteText($"Price: {_selectedCar.GetPrice()}", WIDTH, 13, AlignType.Right);
                WindowBuilder.Instance.WriteText($"Press E to BUY", WIDTH, 23, AlignType.Center);
            }
            else
            {
                WindowBuilder.Instance.WriteText($"YOU HAVE BOUGHT THIS CAR!", WIDTH, 13, AlignType.Right);
                if (UserStats.Instance.SelectedCar != _selectedCar)
                    WindowBuilder.Instance.WriteText($"Press E to SELECT", WIDTH, 23, AlignType.Center);
            }

            if (!String.IsNullOrEmpty(_warning))
                WindowBuilder.Instance.ClearWrite(_warning, WIDTH, 6, AlignType.Center);
            else
                WindowBuilder.Instance.ClearWrite(" ", WIDTH, 6, AlignType.Center);
            _warning = null;

            WindowBuilder.Instance.WriteText($"Press Q to BACK", WIDTH, 24, AlignType.Center);




        }

        private void Controls()
        {
            const int maxTime = 200; // Time in milliseconds to wait between key presses
            _stopwatch.Start();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    char keyChar = Console.ReadKey(true).KeyChar;

                    if (_stopwatch.ElapsedMilliseconds >= maxTime)
                    {

                        switch (char.ToUpper(keyChar))
                        {
                            case 'Q':
                                _mainMenu.PrintMenu();
                                return;
                            case 'E':
                                SelectCar();
                                break;
                            case 'A':
                                _currentCarIndex--;
                                if (_currentCarIndex < 0) _currentCarIndex = _cars.Length - 1;
                                break;
                            case 'D':
                                _currentCarIndex++;
                                break;
                            default:
                                continue;
                        }

                        _currentCarIndex %= _cars.Length;
                        PrintCar();
                        _stopwatch.Restart();
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void SelectCar()
        {
            if (UserStats.Instance.OwnedCars[_currentCarIndex])
            {
                UserStats.Instance.SelectCar(_selectedCar);
                return;
            }

            if (UserStats.Instance.Money >= _selectedCar.GetPrice())
                BuyCar();
            else
            {
                _warning = "YOU DON'T HAVE ENOUGH MONEY!";
            }

        }

        private void BuyCar()
        {
            UserStats.Instance.OwnedCars[_currentCarIndex] = true;
            UserStats.Instance.SelectCar(_selectedCar);
            UserStats.Instance.EarnMoney(-_selectedCar.GetPrice());
            PrintCar();
        }
        private void PrintFrame()
        {
            Console.Clear();
            WindowBuilder.Instance.DrawWindow(WIDTH, HEIGHT);
        }
    }
}
