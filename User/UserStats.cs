namespace CrashMaster.User
{
    internal class UserStats
    {
        public static UserStats Instance { get; private set; }
        public bool[] OwnedCars { get; private set; }
        public Car SelectedCar {  get; private set; }
        public int Money { get; private set; } = 100;
        public int HighScore { get; private set; }
        public int CurrentScore { get; set; }
        public UserStats() 
        {
            Instance = this;
            OwnedCars = new bool[CarFactory.Instance.GetCars().Length];
            SelectedCar = null;
        }




        public void SelectCar(Car car) => SelectedCar = car; 
        public void EarnMoney(int amount) => Money += amount;
        public void TryHighScore()
        {
            if(CurrentScore > HighScore)
                HighScore = CurrentScore;
        }
    }
}
