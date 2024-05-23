using CrashMaster.User;
using System.Diagnostics;

namespace CrashMaster
{
    internal class MainMenu
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private const int HEIGHT = 28;
        private const int WIDTH = 50;

        private string _warning;

        private CarGame _carGame;
        private CarSelection _carSelection;
        private UserStats _us = new UserStats();

        bool _skippedMenu;

        public MainMenu(CarGame cg)
        {
            _carGame = cg;
            _carSelection = new CarSelection(this);
        }



        public void PrintMenu()
        {
            while (true)
            {

                Console.Clear();
                PrintFrame();
                if (!_skippedMenu)
                {
                    PrintLogo();
                    SkipLogo();
                }
                else
                {
                    PrintMainMenu();
                }
            }
        }
        private void SkipLogo()
        {
            while (true)
            {
                char key = char.ToUpper(Console.ReadKey(true).KeyChar);

                switch (key)
                {
                    case 'E':
                        WindowBuilder.Instance.ClearWindow(WIDTH, HEIGHT);
                        _skippedMenu = true;
                        PrintMainMenu();
                        break;
                    case 'Q':
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
                break;
            }
        }


        private void PrintMainMenu()
        {
            if (_warning != null)
                WindowBuilder.Instance.ClearWrite(_warning, WIDTH, 3, AlignType.Center);
            _warning = null;

            string[] carLogo = { "    ____", " __/  |_\\_", "|  _     _``-.", "'-(_)---(_)--' " };
            WindowBuilder.Instance.WriteText(carLogo, 18, 6, center: false);

            string[] startButton = {
                "------------------------",
                "|   Press E to Start   |",
                "------------------------" };
            WindowBuilder.Instance.WriteText(startButton, WIDTH, 11, center: true);

            string[] marketButton = {
                "--------------------------",
                "|  Press M to Go Market  |",
                "--------------------------" };
            WindowBuilder.Instance.WriteText(marketButton, WIDTH, 15, center: true);


            string[] exitButton = {
                "---------------------",
                "|  Press Q to Exit  |",
                "---------------------" };
            WindowBuilder.Instance.WriteText(exitButton, WIDTH, 19, center: true);
            MainMenuControls();

        }
        private void MainMenuControls()
        {
            while (true)
            {
                char key = char.ToUpper(Console.ReadKey(true).KeyChar);
                switch (key)
                {
                    case 'E':
                        if (UserStats.Instance.SelectedCar == null)
                        {
                            _warning = "YOU HAVE TO BUY/SELECT A CAR!";
                            break;
                        }
                        _carGame.Start();
                        break;
                    case 'M':
                        _carSelection.SelectWindow();
                        break;
                    case 'Q':
                        Environment.Exit(0);
                        break;
                    default:
                        continue;
                }
                break;
            }
        }

        private void PrintFrame()
        {
            WindowBuilder.Instance.DrawWindow(WIDTH, HEIGHT);
        }
        private void PrintLogo()
        {
            WindowBuilder.Instance.WriteText("WELCOME TO CRASH MASTER!", WIDTH, 5, AlignType.Center);
            WindowBuilder.Instance.WriteText("Press E to play", WIDTH, 14, AlignType.Center);
            WindowBuilder.Instance.WriteText("Press Q to play", WIDTH, 15, AlignType.Center);
            WindowBuilder.Instance.WriteText("Made by Eyüphan Kaygusuz", WIDTH, 21, AlignType.Center);
            WindowBuilder.Instance.WriteText("COMPLETELY", WIDTH, 22, AlignType.Center);
            string[] carLogo = { "    ____", " __/  |_\\_", "|  _     _``-.", "'-(_)---(_)--' " };
            WindowBuilder.Instance.WriteText(carLogo, 18, 10, center: false);

            Console.SetCursorPosition(0, HEIGHT - 1);
        }
    }
}
