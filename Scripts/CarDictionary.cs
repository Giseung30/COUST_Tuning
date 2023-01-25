using System.Collections.Generic;

public class CarDictionary
{
    public class Brand //�귣��
    {
        public class Car //����
        {
            public string carName; //���� �̸�
            public string carNameENG; //���� �̸� ����
            public string carNameKO; //���� �̸� �ѱ�

            /* ������*/
            public Car(string carName, string carNameENG, string carNameKO)
            {
                this.carName = carName;
                this.carNameENG = carNameENG;
                this.carNameKO = carNameKO;
            }
        }

        public string brandName; //�귣�� �̸�
        public string brandNameENG; //�귣�� �̸� ����
        public string brandNameKO; //�귣�� �̸� �ѱ�
        private List<Car> carList; //���� List
        public int carIndex; //���� �ε���
        public int length
        {
            get { return carList.Count; }
        } //����
        public Car car
        {
            get
            {
                return carList[carIndex];
            }
        } //����
        public string[] carNames
        {
            get
            {
                string[] carNames = new string[carList.Count];
                for (int i = 0; i < carList.Count; ++i) carNames[i] = carList[i].carName;
                return carNames;
            }
        } //���� �̸���
        public string[] carNamesKO
        {
            get
            {
                string[] carNamesKO = new string[carList.Count];
                for (int i = 0; i < carList.Count; ++i) carNamesKO[i] = carList[i].carNameKO;
                return carNamesKO;
            }
        } //���� �̸��� �ѱ�

        /* ������ */
        public Brand(string brandName, string brandNameENG, string brandNameKO)
        {
            this.brandName = brandName;
            this.brandNameENG = brandNameENG;
            this.brandNameKO = brandNameKO;
            carList = new List<Car>();
            carIndex = -1;
        }

        /* �ε��� */
        public Car this[int index]
        {
            get => carList[index];
            set => carList[index] = value;
        }

        /* �߰��ϴ� �Լ� */
        public void Add(string carName, string carNameENG, string carNameKO)
        {
            if (!carList.Exists(car => string.Equals(car.carName, carName))) //������ ã�� ���ϸ�
            {
                carList.Add(new Car(carName, carNameENG, carNameKO)); //���� List�� �߰�
                carIndex = 0; //���� �ε��� �ʱ�ȭ
            }
        }

        /* �ε����� ã�� �Լ� */
        public int FindIndex(string carName)
        {
            return carList.FindIndex(car => string.Equals(car.carName, carName));
        }
    }

    private List<Brand> brandList; //�귣�� List
    public int brandIndex;
    public int length
    {
        get
        {
            return brandList.Count;
        }
    } //����
    public Brand brand
    {
        get
        {
            return brandList[brandIndex];
        }
    } //�귣��
    public string[] brandNames
    {
        get
        {
            string[] brandNames = new string[brandList.Count];
            for (int i = 0; i < brandList.Count; ++i) brandNames[i] = brandList[i].brandName;
            return brandNames;
        }
    } //�귣�� �̸���

    /* ������ */
    public CarDictionary()
    {
        brandList = new List<Brand>();
        brandIndex = -1;
    }

    /* �ε��� */
    public Brand this[int index]
    {
        get => brandList[index];
        set => brandList[index] = value;
    }

    /* �߰��ϴ� �Լ� */
    public void Add(string brandName, string brandNameENG, string brandNameKO, string carName, string carNameENG, string carNameKO)
    {
        Brand brand = brandList.Find(brand => string.Equals(brand.brandName, brandName)); //�귣�� �̸����� ã��
        if (brand == null) //�귣�带 ã�� ���ϸ�
        {
            brand = new Brand(brandName, brandNameENG, brandNameKO); //Brand Ŭ���� ����
            brandList.Add(brand); //�귣�� List�� �߰�
            brandIndex = 0; //�귣�� �ε��� �ʱ�ȭ
        }

        brand.Add(carName, carNameENG, carNameKO); //���� �߰�
    }

    /* �ε����� ã�� �Լ� */
    public int FindIndex(string brandName)
    {
        return brandList.FindIndex(brand => string.Equals(brand.brandName, brandName));
    }
}