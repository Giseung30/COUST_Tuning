#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.IO;

public class SetShaderToSelection : EditorWindow
{
    [Header("Editor")]
    private JsonVar jsonVar = new JsonVar();
    private Vector3 scrollPos;
    private class JsonVar
    {
        public bool enablePaths;
        public string[] paths = new string[0];
    }

    [Header("Variable")]
    private Shader[] shaders = new Shader[0];

    [MenuItem("Custom/Set Shader To Selection")]
    private static void Init()
    {
        SetShaderToSelection window = (SetShaderToSelection)GetWindow(typeof(SetShaderToSelection));
        window.Show();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0, l = shaders.Length; i < l; ++i)
        {
            if (GUILayout.Button(shaders[i].name)) SetShader((Material)Selection.activeObject, shaders[i], true);
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Create Shader Buttons")) shaders = GetShaders(jsonVar.paths);

        jsonVar.enablePaths = EditorGUILayout.Foldout(jsonVar.enablePaths, "Paths", true);
        if (jsonVar.enablePaths)
        {
            ++EditorGUI.indentLevel;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset")) ArrayUtility.Clear(ref jsonVar.paths);
            if (GUILayout.Button("+")) ArrayUtility.Add(ref jsonVar.paths, null);
            if (GUILayout.Button("-") && jsonVar.paths.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.paths, jsonVar.paths.Length - 1);
            EditorGUILayout.LabelField(jsonVar.paths.Length.ToString());
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            for (int i = 0; i < jsonVar.paths.Length; ++i)
            {
                GUILayout.BeginHorizontal();
                jsonVar.paths[i] = EditorGUILayout.TextField(jsonVar.paths[i]);

                if (GUILayout.Button("+")) ArrayUtility.Insert(ref jsonVar.paths, i + 1, null);

                if (GUILayout.Button("-")) ArrayUtility.RemoveAt(ref jsonVar.paths, i);
                GUILayout.EndHorizontal();
            }
            --EditorGUI.indentLevel;
        }
    }
    private void OnEnable()
    {
        LoadConfigJson();
    }

    private void OnDisable()
    {
        SaveConfigJson();
    }

    #region Load Config Json
    /* Config Json을 불러오는 함수 */
    private void LoadConfigJson()
    {
        TextAsset configJson = AssetDatabase.LoadAssetAtPath<TextAsset>(GetAssetDirectoryPath(GetType().Name + ".cs").Replace(Application.dataPath, "Assets") + '/' + GetType().Name + ".json");
        if (!configJson) return;

        jsonVar = JsonUtility.FromJson<JsonVar>(configJson.text);
    }

    /* Config Json을 저장하는 함수 */
    private void SaveConfigJson()
    {
        File.WriteAllText(GetAssetDirectoryPath(GetType().Name + ".cs") + '/' + GetType().Name + ".json", JsonUtility.ToJson(jsonVar));
        AssetDatabase.Refresh();
    }

    /* 현재 Asset의 디렉터리 경로를 반환하는 함수 */
    private string GetAssetDirectoryPath(string assetName)
    {
        string result = null;
        string[] paths = Directory.GetFiles(Application.dataPath, assetName, SearchOption.AllDirectories);
        if (paths.Length != 0)
        {
            result = Path.GetDirectoryName(paths[0]).Replace('\\', '/');
        }
        return result;
    }
    #endregion

    private Shader[] GetShaders(string[] paths)
    {
        Shader[] shaders = new Shader[0];

        string[] assets = AssetDatabase.FindAssets("t: shader", paths);
        for (int i = 0, l = assets.Length; i < l; ++i)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
            ArrayUtility.Add(ref shaders, (Shader)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Shader)));
        }

        return shaders;
    }

    private void SetShader(Material material, Shader shader, bool reset)
    {
        if (!material) return;

        material.shader = shader;
        if (reset)
        {
            for (int i = 0, l = shader.GetPropertyCount(); i < l; ++i)
            {
                switch (shader.GetPropertyType(i))
                {
                    case ShaderPropertyType.Color:
                        material.SetColor(shader.GetPropertyName(i), shader.GetPropertyDefaultVectorValue(i));
                        break;
                    case ShaderPropertyType.Float:
                        material.SetFloat(shader.GetPropertyName(i), shader.GetPropertyDefaultFloatValue(i));
                        break;
                    case ShaderPropertyType.Int:
                        material.SetInt(shader.GetPropertyName(i), (int)shader.GetPropertyDefaultFloatValue(i));
                        break;
                    case ShaderPropertyType.Vector:
                        material.SetVector(shader.GetPropertyName(i), shader.GetPropertyDefaultVectorValue(i));
                        break;
                }
            }
        }
    }
}
#endif