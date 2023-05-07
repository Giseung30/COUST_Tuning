#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class MaterialManager
{
    [MenuItem("Custom/Material/Enable GPU Instancing")]
    public static void EnableGPUInstancing()
    {
        Object[] objs = Selection.objects;
        if (objs == null) return;

        for (int i = 0; i < objs.Length; ++i)
        {
            if (objs[i].GetType().Name == "Material") (objs[i] as Material).enableInstancing = true;
        }
    }

    [MenuItem("Custom/Material/Disable GPU Instancing")]
    public static void DisableGPUInstancing()
    {
        Object[] objs = Selection.objects;
        if (objs == null) return;

        for (int i = 0; i < objs.Length; ++i)
        {
            if (objs[i].GetType().Name == "Material") (objs[i] as Material).enableInstancing = false;
        }
    }
}
#endif