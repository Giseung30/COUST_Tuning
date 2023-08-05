#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SelectMipMapTextures : EditorWindow
{
    [Header("Editor")]
    private string path;
    private bool isMipMap;

    [MenuItem("Custom/Select Mip Map Textures")]
    private static void Init()
    {
        SelectMipMapTextures window = (SelectMipMapTextures)GetWindow(typeof(SelectMipMapTextures));
        window.Show();
    }

    private void OnGUI()
    {
        path = EditorGUILayout.TextField("Path", path);
        isMipMap = EditorGUILayout.Toggle("Enable Mip Map", isMipMap);

        if (GUILayout.Button("Select Textures")) Selection.objects = GetMipMapTextures(path, isMipMap);
    }

    private Texture[] GetMipMapTextures(string path, bool isMipMap)
    {
        Texture[] textures = new Texture[0];

        string[] assets = AssetDatabase.FindAssets("t: texture", new string[] { path });
        for (int i = 0, l = assets.Length; i < l; ++i)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
            Texture texture = (Texture)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture));
            if (isMipMap && texture.mipmapCount > 1) ArrayUtility.Add(ref textures, texture);
            else if(!isMipMap && texture.mipmapCount <= 1) ArrayUtility.Add(ref textures, texture);
        }

        return textures;
    }
}
#endif