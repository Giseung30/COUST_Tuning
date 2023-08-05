using UnityEngine;
using System;

[Serializable]
public struct Pair<T1, T2>
{
    public T1 first;
    public T2 second;

    public Pair(T1 first, T2 second)
    {
        this.first = first;
        this.second = second;
    }
}

public class Utility : MonoBehaviour
{
    public static Material FindMaterialWithName(Material[] materials, string name)
    {
        if (materials == null) return null; //Materials가 존재하면

        for (int i = 0; i < materials.Length; ++i) //Material 개수만큼 반복
        {
            if (materials[i].name.Equals(name)) //이름이 일치하면
            {
                return materials[i]; //반환
            }
        }

        return null; //일치하는 것이 없으면 null 반환
    }

    public static float SetEase(LeanTweenType leanTweenType, float start, float end, float val)
    {
        switch (leanTweenType)
        {
            case LeanTweenType.linear:
                return LeanTween.linear(start, end, val);
            case LeanTweenType.easeOutQuad:
                return LeanTween.easeOutQuad(start, end, val);
            case LeanTweenType.easeInQuad:
                return LeanTween.easeInQuad(start, end, val);
            case LeanTweenType.easeInOutQuad:
                return LeanTween.easeInOutQuad(start, end, val);
            case LeanTweenType.easeInCubic:
                return LeanTween.easeInCubic(start, end, val);
            case LeanTweenType.easeOutCubic:
                return LeanTween.easeOutCubic(start, end, val);
            case LeanTweenType.easeInOutCubic:
                return LeanTween.easeInOutCubic(start, end, val);
            case LeanTweenType.easeInQuart:
                return LeanTween.easeInQuart(start, end, val);
            case LeanTweenType.easeOutQuart:
                return LeanTween.easeOutQuart(start, end, val);
            case LeanTweenType.easeInOutQuart:
                return LeanTween.easeInOutQuart(start, end, val);
            case LeanTweenType.easeInQuint:
                return LeanTween.easeInQuint(start, end, val);
            case LeanTweenType.easeOutQuint:
                return LeanTween.easeOutQuint(start, end, val);
            case LeanTweenType.easeInOutQuint:
                return LeanTween.easeInOutQuint(start, end, val);
            case LeanTweenType.easeInSine:
                return LeanTween.easeInSine(start, end, val);
            case LeanTweenType.easeOutSine:
                return LeanTween.easeOutSine(start, end, val);
            case LeanTweenType.easeInOutSine:
                return LeanTween.easeInOutSine(start, end, val);
            case LeanTweenType.easeInExpo:
                return LeanTween.easeInExpo(start, end, val);
            case LeanTweenType.easeOutExpo:
                return LeanTween.easeOutExpo(start, end, val);
            case LeanTweenType.easeInOutExpo:
                return LeanTween.easeInOutExpo(start, end, val);
            case LeanTweenType.easeInCirc:
                return LeanTween.easeInCirc(start, end, val);
            case LeanTweenType.easeOutCirc:
                return LeanTween.easeOutCirc(start, end, val);
            case LeanTweenType.easeInOutCirc:
                return LeanTween.easeInOutCirc(start, end, val);
            case LeanTweenType.easeInBounce:
                return LeanTween.easeInBounce(start, end, val);
            case LeanTweenType.easeOutBounce:
                return LeanTween.easeOutBounce(start, end, val);
            case LeanTweenType.easeInOutBounce:
                return LeanTween.easeInOutBounce(start, end, val);
            case LeanTweenType.easeInBack:
                return LeanTween.easeInBack(start, end, val);
            case LeanTweenType.easeOutBack:
                return LeanTween.easeOutBack(start, end, val);
            case LeanTweenType.easeInOutBack:
                return LeanTween.easeInOutBack(start, end, val);
            case LeanTweenType.easeInElastic:
                return LeanTween.easeInElastic(start, end, val);
            case LeanTweenType.easeOutElastic:
                return LeanTween.easeOutElastic(start, end, val);
            case LeanTweenType.easeInOutElastic:
                return LeanTween.easeInOutElastic(start, end, val);
            default:
                return Mathf.Lerp(start, end, val);
        }
    }

    public static float Average(params float[] values)
    {
        float avg = 0f;
        int len = values.Length;
        for (int i = 0; i < len; ++i) avg += values[i];
        return avg / len;
    }
}