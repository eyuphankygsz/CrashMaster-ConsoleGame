using System.Diagnostics;
using System.Reflection;

namespace CrashMaster
{
    internal class CarFactory
    {
        public CarFactory()
        {
            Instance = this;
            CreateCars();
        }

        public static CarFactory Instance { get; private set; }
        private Dictionary<string, Car> _cars;
        public void CreateCars()
        {
            _cars = new Dictionary<string, Car>();
            var carTypes = Assembly.GetExecutingAssembly().GetTypes()
                                   .Where(t => t.IsSubclassOf(typeof(Car)) && !t.IsAbstract)
                                   .ToList();

            foreach (var carType in carTypes)
            {
                Car carInstance = (Car)Activator.CreateInstance(carType);
                carInstance.Setup();
                Debug.WriteLine(carInstance);
                _cars.Add(carInstance.GetName(), carInstance);
            }
        }
        public Car GetCar(string carName)
        {
            return _cars[carName];
        }
        public Car[] GetCars()
        {
            return _cars.Values.OrderBy(x => x.GetPrice()).ToArray();
        }


    }
}
