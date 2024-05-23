namespace CrashMaster.Cars
{
    internal class RaceCar: Car
    {
        public override void Setup()
        {
            _maxSpeed = 200;
            _acceleration = 40;
            _carName = "Race Car";
            _price = 8000;

            _logo = new string[] {
                "__        ",
                ".-'--`-._ ",
                "'-O---O--'"
            };
        }
    }
}
