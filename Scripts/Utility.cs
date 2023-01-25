using UnityEngine;
using System;

/* �� ���� �ڷ����� �����ϴ� ����ü */
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
    /* Material�� �߿��� �̸��� ��ġ�ϴ� Material�� ��ȯ�ϴ� �Լ� */
    public static Material FindMaterialWithName(Material[] materials, string name)
    {
        if (materials == null) return null; //Materials�� �����ϸ�

        for (int i = 0; i < materials.Length; ++i) //Material ������ŭ �ݺ�
        {
            if (materials[i].name.Equals(name)) //�̸��� ��ġ�ϸ�
            {
                return materials[i]; //��ȯ
            }
        }

        return null; //��ġ�ϴ� ���� ������ null ��ȯ
    }
}