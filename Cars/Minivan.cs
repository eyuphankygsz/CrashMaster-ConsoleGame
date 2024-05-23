namespace CrashMaster
{
    internal class Minivan : Car
    {
        public override void Setup()
        {
            _maxSpeed = 100;
            _acceleration = 10;
            _carName = "Minivan";
            _price = 100;
            _logo = new string[] {
                "  _____________",
                " //__][__][___|",
                "|   -|     _ o|",
                "\\--(_)----(_)-'"
            };
        }

    }
}
