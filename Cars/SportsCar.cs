namespace CrashMaster.Cars
{
    internal class SportsCar : Car
    {
        public override void Setup()
        {
            _maxSpeed = 140;
            _acceleration = 20;
            _carName = "Sports Car";
            _price = 2500;
            _logo = new string[] {
                " _.-.___\\__    ",
                "|  _      _`-. ",
                "'-(_)----(_)--`"
            };
        }
    }
}
