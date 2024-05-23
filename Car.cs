namespace CrashMaster
{
    internal abstract class Car
    {

        #region fields

        protected string _carName;
        protected int _maxSpeed;
        protected int _currentSpeed;
        protected int _acceleration;
        protected int _price;
        protected string[] _logo;
        #endregion

        #region AbstractMethods
        public abstract void Setup();
        #endregion

        #region Methods
        public int CurrentSpeed
        {
            get { return _currentSpeed; }
            set
            {
                if (value > _maxSpeed)
                    value = _maxSpeed;
                else if (value < 0)
                    value = 0;

                _currentSpeed = value;
            }
        }

        public string GetName() => _carName;
        public float GetMaxSpeed() => _maxSpeed;
        public float GetCurrentSpeed() => _currentSpeed;
        public float GetCarAccelerationSpeed() => _acceleration;
        public int GetPrice() => _price;
        public string[] GetLogo() => _logo;
        public void Accelerate(bool faster)
        {
            CurrentSpeed += (faster ? 1 : -1) * _acceleration;
        }

        #endregion


    }
}
