using UnityEngine;
using System;

/* 두 개의 자료형을 저장하는 구조체 */
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
    /* Material들 중에서 이름과 일치하는 Material을 반환하는 함수 */
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
}