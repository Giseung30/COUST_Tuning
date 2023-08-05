using UnityEngine.Events;
using System.Collections.Generic;

public class BackgroundDictionary
{
    public class Background
    {
        /** Data **/
        public string backgroundName;
        public string backgroundNameENG;
        public string backgroundNameKO;

        /** Function **/
        public Background(string backgroundName, string backgroundNameENG, string backgroundNameKO)
        {
            this.backgroundName = backgroundName;
            this.backgroundNameENG = backgroundNameENG;
            this.backgroundNameKO = backgroundNameKO;
        }
    }

    /** Background List **/
    private List<Background> backgroundList;
    private int backgroundIndex;
    public int GetBackgroundIndex()
    {
        return backgroundIndex;
    }
    public void SetBackgroundIndex(int value)
    {
        bool onChange = backgroundIndex != value;
        backgroundIndex = value;
        if (onChange) onChangeIndex?.Invoke();
    }
    public int length
    {
        get
        {
            return backgroundList.Count;
        }
    }
    public Background background
    {
        get
        {
            return backgroundList[GetBackgroundIndex()];
        }
    }
    public string[] backgroundNames
    {
        get
        {
            string[] backgroundNames = new string[backgroundList.Count];
            for (int i = 0; i < backgroundList.Count; ++i) backgroundNames[i] = backgroundList[i].backgroundName;
            return backgroundNames;
        }
    }
    public Background this[int index]
    {
        get => backgroundList[index];
        set => backgroundList[index] = value;
    }
    public void Add(string backgroundName, string backgroundNameENG, string backgroundNameKO)
    {
        Background background = backgroundList.Find(background => string.Equals(background.backgroundName, backgroundName));
        if (background == null) backgroundList.Add(new Background(backgroundName, backgroundNameENG, backgroundNameKO));
    }
    public int FindIndex(string backgroundName)
    {
        return backgroundList.FindIndex(background => string.Equals(background.backgroundName, backgroundName));
    }

    /** Function **/
    public BackgroundDictionary()
    {
        backgroundList = new List<Background>();
        SetBackgroundIndex(-1);
        onChangeIndex = new UnityEvent();
    }

    /** Event **/
    public UnityEvent onChangeIndex;
}