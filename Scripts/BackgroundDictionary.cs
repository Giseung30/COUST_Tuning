using System.Collections.Generic;

public class BackgroundDictionary
{
    public class Background //���
    {
        public string backgroundName; //��� �̸�
        public string backgroundNameENG; //��� �̸� ����
        public string backgroundNameKO; //��� �̸� �ѱ�

        public Background(string backgroundName, string backgroundNameENG, string backgroundNameKO) //������
        {
            this.backgroundName = backgroundName;
            this.backgroundNameENG = backgroundNameENG;
            this.backgroundNameKO = backgroundNameKO;
        }
    }

    private List<Background> backgroundList; //��� List
    public int backgroundIndex; //��� �ε���
    public int length
    {
        get
        {
            return backgroundList.Count;
        }
    } //����
    public Background background
    {
        get
        {
            return backgroundList[backgroundIndex];
        }
    } //���
    public string[] backgroundNames
    {
        get
        {
            string[] backgroundNames = new string[backgroundList.Count];
            for (int i = 0; i < backgroundList.Count; ++i) backgroundNames[i] = backgroundList[i].backgroundName;
            return backgroundNames;
        }
    } //��� �̸���

    /* ������ */
    public BackgroundDictionary()
    {
        backgroundList = new List<Background>();
        backgroundIndex = -1;
    }

    /* �ε��� */
    public Background this[int index]
    {
        get => backgroundList[index];
        set => backgroundList[index] = value;
    }

    /* �߰��ϴ� �Լ� */
    public void Add(string backgroundName, string backgroundNameENG, string backgroundNameKO)
    {
        Background background = backgroundList.Find(background => string.Equals(background.backgroundName, backgroundName)); //��� �̸����� ã��
        if (background == null) //����� ã�� ���ϸ�
        {
            backgroundList.Add(new Background(backgroundName, backgroundNameENG, backgroundNameKO)); //��� List�� �߰�
        }
    }

    /* �ε����� ã�� �Լ� */
    public int FindIndex(string backgroundName)
    {
        return backgroundList.FindIndex(background => string.Equals(background.backgroundName, backgroundName));
    }
}