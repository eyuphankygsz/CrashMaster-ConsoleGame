namespace CrashMaster.Cars
{
    internal class ClassicalCar : Car
    {
        public override void Setup()
        {
            _maxSpeed = 110;
            _acceleration = 15;
            _carName = "Classical Car";
            _price = 1000;
            _logo = new string[] {
                "    ____       ",
                " __/  |_\\_     ",
                "|  _     _``-. ",
                "'-(_)---(_)--' "
            };
        }

    }
}
