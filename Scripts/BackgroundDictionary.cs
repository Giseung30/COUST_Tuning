using System.Collections.Generic;

public class BackgroundDictionary
{
    public class Background //배경
    {
        public string backgroundName; //배경 이름
        public string backgroundNameENG; //배경 이름 영어
        public string backgroundNameKO; //배경 이름 한글

        public Background(string backgroundName, string backgroundNameENG, string backgroundNameKO) //생성자
        {
            this.backgroundName = backgroundName;
            this.backgroundNameENG = backgroundNameENG;
            this.backgroundNameKO = backgroundNameKO;
        }
    }

    private List<Background> backgroundList; //배경 List
    public int backgroundIndex; //배경 인덱스
    public int length
    {
        get
        {
            return backgroundList.Count;
        }
    } //길이
    public Background background
    {
        get
        {
            return backgroundList[backgroundIndex];
        }
    } //배경
    public string[] backgroundNames
    {
        get
        {
            string[] backgroundNames = new string[backgroundList.Count];
            for (int i = 0; i < backgroundList.Count; ++i) backgroundNames[i] = backgroundList[i].backgroundName;
            return backgroundNames;
        }
    } //배경 이름들

    /* 생성자 */
    public BackgroundDictionary()
    {
        backgroundList = new List<Background>();
        backgroundIndex = -1;
    }

    /* 인덱서 */
    public Background this[int index]
    {
        get => backgroundList[index];
        set => backgroundList[index] = value;
    }

    /* 추가하는 함수 */
    public void Add(string backgroundName, string backgroundNameENG, string backgroundNameKO)
    {
        Background background = backgroundList.Find(background => string.Equals(background.backgroundName, backgroundName)); //배경 이름으로 찾기
        if (background == null) //배경을 찾지 못하면
        {
            backgroundList.Add(new Background(backgroundName, backgroundNameENG, backgroundNameKO)); //배경 List에 추가
        }
    }

    /* 인덱스를 찾는 함수 */
    public int FindIndex(string backgroundName)
    {
        return backgroundList.FindIndex(background => string.Equals(background.backgroundName, backgroundName));
    }
}