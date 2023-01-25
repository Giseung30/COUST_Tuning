using System.Collections.Generic;

public class CarDictionary
{
    public class Brand //브랜드
    {
        public class Car //차량
        {
            public string carName; //차량 이름
            public string carNameENG; //차량 이름 영어
            public string carNameKO; //차량 이름 한글

            /* 생성자*/
            public Car(string carName, string carNameENG, string carNameKO)
            {
                this.carName = carName;
                this.carNameENG = carNameENG;
                this.carNameKO = carNameKO;
            }
        }

        public string brandName; //브랜드 이름
        public string brandNameENG; //브랜드 이름 영어
        public string brandNameKO; //브랜드 이름 한글
        private List<Car> carList; //차량 List
        public int carIndex; //차량 인덱스
        public int length
        {
            get { return carList.Count; }
        } //길이
        public Car car
        {
            get
            {
                return carList[carIndex];
            }
        } //차량
        public string[] carNames
        {
            get
            {
                string[] carNames = new string[carList.Count];
                for (int i = 0; i < carList.Count; ++i) carNames[i] = carList[i].carName;
                return carNames;
            }
        } //차량 이름들
        public string[] carNamesKO
        {
            get
            {
                string[] carNamesKO = new string[carList.Count];
                for (int i = 0; i < carList.Count; ++i) carNamesKO[i] = carList[i].carNameKO;
                return carNamesKO;
            }
        } //차량 이름들 한글

        /* 생성자 */
        public Brand(string brandName, string brandNameENG, string brandNameKO)
        {
            this.brandName = brandName;
            this.brandNameENG = brandNameENG;
            this.brandNameKO = brandNameKO;
            carList = new List<Car>();
            carIndex = -1;
        }

        /* 인덱서 */
        public Car this[int index]
        {
            get => carList[index];
            set => carList[index] = value;
        }

        /* 추가하는 함수 */
        public void Add(string carName, string carNameENG, string carNameKO)
        {
            if (!carList.Exists(car => string.Equals(car.carName, carName))) //차량을 찾지 못하면
            {
                carList.Add(new Car(carName, carNameENG, carNameKO)); //차량 List에 추가
                carIndex = 0; //차량 인덱스 초기화
            }
        }

        /* 인덱스를 찾는 함수 */
        public int FindIndex(string carName)
        {
            return carList.FindIndex(car => string.Equals(car.carName, carName));
        }
    }

    private List<Brand> brandList; //브랜드 List
    public int brandIndex;
    public int length
    {
        get
        {
            return brandList.Count;
        }
    } //길이
    public Brand brand
    {
        get
        {
            return brandList[brandIndex];
        }
    } //브랜드
    public string[] brandNames
    {
        get
        {
            string[] brandNames = new string[brandList.Count];
            for (int i = 0; i < brandList.Count; ++i) brandNames[i] = brandList[i].brandName;
            return brandNames;
        }
    } //브랜드 이름들

    /* 생성자 */
    public CarDictionary()
    {
        brandList = new List<Brand>();
        brandIndex = -1;
    }

    /* 인덱서 */
    public Brand this[int index]
    {
        get => brandList[index];
        set => brandList[index] = value;
    }

    /* 추가하는 함수 */
    public void Add(string brandName, string brandNameENG, string brandNameKO, string carName, string carNameENG, string carNameKO)
    {
        Brand brand = brandList.Find(brand => string.Equals(brand.brandName, brandName)); //브랜드 이름으로 찾기
        if (brand == null) //브랜드를 찾지 못하면
        {
            brand = new Brand(brandName, brandNameENG, brandNameKO); //Brand 클래스 생성
            brandList.Add(brand); //브랜드 List에 추가
            brandIndex = 0; //브랜드 인덱스 초기화
        }

        brand.Add(carName, carNameENG, carNameKO); //차량 추가
    }

    /* 인덱스를 찾는 함수 */
    public int FindIndex(string brandName)
    {
        return brandList.FindIndex(brand => string.Equals(brand.brandName, brandName));
    }
}