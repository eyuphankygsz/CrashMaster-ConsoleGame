namespace CrashMaster.Cars
{
    internal class ElectricalCar : Car
    {
        public override void Setup()
        {
            _maxSpeed = 160;
            _acceleration = 25;
            _carName = "Electrical Car";
            _price = 4000;
            _logo = new string[] {
                "  ______     ",
                " /|_||_\\`.__ ",
                "(   _    _ _\\",
                "=`-(_)--(_)-'"
            };
        }
    }
}
