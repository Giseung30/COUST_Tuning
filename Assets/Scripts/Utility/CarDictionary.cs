using UnityEngine.Events;
using System.Collections.Generic;

public class CarDictionary
{
    public class Brand
    {
        public class Car
        {
            /** Data **/
            public string carName;
            public string carNameENG;
            public string carNameKO;

            /** Function **/
            public Car(string carName, string carNameENG, string carNameKO)
            {
                this.carName = carName;
                this.carNameENG = carNameENG;
                this.carNameKO = carNameKO;
            }
        }

        /** Data **/
        public string brandName;
        public string brandNameENG;
        public string brandNameKO;

        /** Function **/
        public Brand(string brandName, string brandNameENG, string brandNameKO, UnityEvent onChangeIndex)
        {
            this.brandName = brandName;
            this.brandNameENG = brandNameENG;
            this.brandNameKO = brandNameKO;
            this.onChangeIndex = onChangeIndex;

            carList = new List<Car>();
            SetCarIndex(-1);
        }

        /** Car List **/
        private List<Car> carList;
        private int carIndex;
        public int GetCarIndex()
        {
            return carIndex;
        }
        public void SetCarIndex(int value)
        {
            bool onChange = carIndex != value;
            carIndex = value;
            if (onChange) onChangeIndex?.Invoke();
        }
        public int length
        {
            get { return carList.Count; }
        }
        public Car car
        {
            get
            {
                return carList[GetCarIndex()];
            }
        }
        public string[] carNames
        {
            get
            {
                string[] carNames = new string[carList.Count];
                for (int i = 0; i < carList.Count; ++i) carNames[i] = carList[i].carName;
                return carNames;
            }
        }
        public string[] carNamesKO
        {
            get
            {
                string[] carNamesKO = new string[carList.Count];
                for (int i = 0; i < carList.Count; ++i) carNamesKO[i] = carList[i].carNameKO;
                return carNamesKO;
            }
        }
        public Car this[int index]
        {
            get => carList[index];
            set => carList[index] = value;
        }
        public void Add(string carName, string carNameENG, string carNameKO)
        {
            if (!carList.Exists(car => string.Equals(car.carName, carName)))
            {
                carList.Add(new Car(carName, carNameENG, carNameKO));
                SetCarIndex(0);
            }
        }
        public int FindIndex(string carName)
        {
            return carList.FindIndex(car => string.Equals(car.carName, carName));
        }

        /** Event **/
        private UnityEvent onChangeIndex;
    }

    /** Function **/
    public CarDictionary()
    {
        brandList = new List<Brand>();
        SetBrandIndex(-1);
        onChangeIndex = new UnityEvent();
    }

    /** Brand List **/
    private List<Brand> brandList;
    private int brandIndex;
    public int GetBrandIndex()
    {
        return brandIndex;
    }
    public void SetBrandIndex(int value)
    {
        bool onChange = brandIndex != value;
        brandIndex = value;
        if (onChange) onChangeIndex?.Invoke();
    }
    public int length
    {
        get
        {
            return brandList.Count;
        }
    }
    public Brand brand
    {
        get
        {
            return brandList[GetBrandIndex()];
        }
    }
    public string[] brandNames
    {
        get
        {
            string[] brandNames = new string[brandList.Count];
            for (int i = 0; i < brandList.Count; ++i) brandNames[i] = brandList[i].brandName;
            return brandNames;
        }
    }
    public Brand this[int index]
    {
        get => brandList[index];
        set => brandList[index] = value;
    }
    public void Add(string brandName, string brandNameENG, string brandNameKO, string carName, string carNameENG, string carNameKO)
    {
        Brand brand = brandList.Find(brand => string.Equals(brand.brandName, brandName));
        if (brand == null)
        {
            brand = new Brand(brandName, brandNameENG, brandNameKO, onChangeIndex);
            brandList.Add(brand);
            SetBrandIndex(0);
        }

        brand.Add(carName, carNameENG, carNameKO);
    }
    public int FindIndex(string brandName)
    {
        return brandList.FindIndex(brand => string.Equals(brand.brandName, brandName));
    }

    /** Event **/
    public UnityEvent onChangeIndex;
}