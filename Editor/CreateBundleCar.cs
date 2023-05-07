#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class CreateBundleCar : EditorWindow
{
    [Header("Definition")]
    private readonly float spaceHeight = 10f;
    private readonly Color mainLabelColor = new Color(0.4f, 0.4f, 0f);
    private readonly string viewPointCameraName = "Camera (View Point)";

    [Header("Editor")]
    private Vector3 scrollPos;
    private JsonVar jsonVar = new JsonVar();
    [Serializable]
    private class JsonVar
    {
        [Header("01. Set Car Prefab")]
        public GameObject carPrefab;

        [Header("02. Create Paint Infos")]
        public string paintInfosName;

        [Header("03. Add Car Manager")]
        public Vector2 heightRange;
        public Vector2 camberAngleRange;
        public Vector2 wheelProtrusionRange;

        public string bodyName;

        public string wheelFrontLeftName;
        public string wheelFrontRightName;
        public string wheelRearLeftName;
        public string wheelRearRightName;

        public string headLightMaterialName;
        public string turnLightMaterialName;
        public string tailLightMaterialName;
        public string lightProperty;

        [Header("05. Create Blob Shadow")]
        public GameObject blobShadow;
        public Vector3 blobShadowPosition;
        public Vector3 blobShadowRotation;
        public Vector3 blobShadowScale;

        [Header("06. Create Paint Info Objects")]
        public bool enableSelectionObjects;

        [Header("07. Add Paint Info")]
        public int selectedPaintInfo;
        public bool enablePaintInfo;

        /* All Color Surface Info */
        public int selectedAllColorSurfaceInfoPreset;
        public int[] allColorSurfaceInfoPresetSizes
        {
            get
            {
                int[] sizes = new int[allColorSurfaceInfoPresets.Length];
                for (int i = 0, l = sizes.Length; i < l; ++i) sizes[i] = i;
                return sizes;
            }
        }
        public string[] allColorSurfaceInfoPresetNames
        {
            get
            {
                string[] presetNames = new string[allColorSurfaceInfoPresets.Length];
                for (int i = 0, l = presetNames.Length; i < l; ++i) presetNames[i] = allColorSurfaceInfoPresets[i].presetName;
                return presetNames;
            }
        }
        public AllColorSurfaceInfoPreset[] allColorSurfaceInfoPresets = new AllColorSurfaceInfoPreset[0];

        /* Color Surface Info */
        public int selectedColorSurfaceInfoPreset;
        public int[] colorSurfaceInfoPresetSizes
        {
            get
            {
                int[] sizes = new int[colorSurfaceInfoPresets.Length];
                for (int i = 0, l = sizes.Length; i < l; ++i) sizes[i] = i;
                return sizes;
            }
        }
        public string[] colorSurfaceInfoPresetNames
        {
            get
            {
                string[] presetNames = new string[colorSurfaceInfoPresets.Length];
                for (int i = 0, l = presetNames.Length; i < l; ++i) presetNames[i] = colorSurfaceInfoPresets[i].presetName;
                return presetNames;
            }
        }
        public ColorSurfaceInfoPreset[] colorSurfaceInfoPresets = new ColorSurfaceInfoPreset[0];

        /* Color Info */
        public int selectedColorInfoPreset;
        public int[] colorInfoPresetSizes
        {
            get
            {
                int[] sizes = new int[colorInfoPresets.Length];
                for (int i = 0, l = sizes.Length; i < l; ++i) sizes[i] = i;
                return sizes;
            }
        }
        public string[] colorInfoPresetNames
        {
            get
            {
                string[] presetNames = new string[colorInfoPresets.Length];
                for (int i = 0, l = presetNames.Length; i < l; ++i) presetNames[i] = colorInfoPresets[i].presetName;
                return presetNames;
            }
        }
        public ColorInfoPreset[] colorInfoPresets = new ColorInfoPreset[0];

        /* Windshield Info */
        public int selectedWindshieldInfoPreset;
        public int[] windshieldInfoPresetSizes
        {
            get
            {
                int[] sizes = new int[windshieldInfoPresets.Length];
                for (int i = 0, l = sizes.Length; i < l; ++i) sizes[i] = i;
                return sizes;
            }
        }
        public string[] windshieldInfoPresetNames
        {
            get
            {
                string[] presetNames = new string[windshieldInfoPresets.Length];
                for (int i = 0, l = presetNames.Length; i < l; ++i) presetNames[i] = windshieldInfoPresets[i].presetName;
                return presetNames;
            }
        }
        public WindshieldInfoPreset[] windshieldInfoPresets = new WindshieldInfoPreset[0];

        [Serializable]
        public class AllColorSurfaceInfoPreset
        {
            public string presetName;

            public PaintCategory paintCategory;

            public Vector3 cameraViewPointOffset;

            public string partMeshRendererName;

            public string partColorProperty;

            public string[] partSurfaceNames = new string[0];
            public Shader[] partSurfaceShaders = new Shader[0];

            public string[] colorSurfaceInfoNames = new string[0];

            public AllColorSurfaceInfoPreset(string presetName)
            {
                this.presetName = presetName;
            }
        }

        [Serializable]
        public class ColorSurfaceInfoPreset
        {
            public string presetName;

            public PaintCategory paintCategory;

            public Vector3 cameraViewPointOffset;

            public string partMeshRendererName;

            public string partColorProperty;

            public string[] partSurfaceNames = new string[0];
            public Shader[] partSurfaceShaders = new Shader[0];

            public ColorSurfaceInfoPreset(string presetName)
            {
                this.presetName = presetName;
            }
        }

        [Serializable]
        public class ColorInfoPreset
        {
            public string presetName;

            public PaintCategory paintCategory;

            public Vector3 cameraViewPointOffset;

            public string partMeshRendererName;

            public string partColorProperty;

            public ColorInfoPreset(string presetName)
            {
                this.presetName = presetName;
            }
        }

        [Serializable]
        public class WindshieldInfoPreset
        {
            public string presetName;

            public PaintCategory paintCategory;

            public Vector3 cameraViewPointOffset;

            public string partMeshRendererName;

            public string partColorProperty;

            public string partTintingProperty;

            public WindshieldInfoPreset(string presetName)
            {
                this.presetName = presetName;
            }
        }

        [Header("07-A. Define Part Name")]
        public bool enablePartName;
        public string[] partNamesENG = new string[0];
        public string[] partNamesKOR = new string[0];

        [Header("08. Camera View Point Utility")]
        public bool setMultiple;
        public float positionMultiple;
        public float rotationMultiple;
        public bool createViewPointCamera;
    }

    [MenuItem("Custom/Create Bundle Car")]
    private static void Init()
    {
        CreateBundleCar window = (CreateBundleCar)GetWindow(typeof(CreateBundleCar));
        window.Show();
    }

    private void OnEnable()
    {
        LoadConfigJson();
    }

    private void OnDisable()
    {
        SaveConfigJson();
    }

    /* GUI를 생성하는 함수 */
    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        ColorLabelField("01. Set Car Prefab", EditorStyles.boldLabel, mainLabelColor);
        ++EditorGUI.indentLevel;
        jsonVar.carPrefab = (GameObject)EditorGUILayout.ObjectField("Car Prefab", jsonVar.carPrefab, typeof(GameObject), true);
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);

        ColorLabelField("02. Create Paint Infos", EditorStyles.boldLabel, mainLabelColor);
        ++EditorGUI.indentLevel;
        jsonVar.paintInfosName = EditorGUILayout.TextField("Paint Infos Name", jsonVar.paintInfosName);
        if (GUILayout.Button("Create Paint Infos")) CreatePaintInfos();
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);

        ColorLabelField("03. Add Car Manager", EditorStyles.boldLabel, mainLabelColor);
        ++EditorGUI.indentLevel;
        jsonVar.heightRange = EditorGUILayout.Vector2Field("Height Range", jsonVar.heightRange);
        jsonVar.camberAngleRange = EditorGUILayout.Vector2Field("Camber Angle Range", jsonVar.camberAngleRange);
        jsonVar.wheelProtrusionRange = EditorGUILayout.Vector2Field("Wheel Protrusion Range", jsonVar.wheelProtrusionRange);
        EditorGUILayout.Space();
        jsonVar.bodyName = EditorGUILayout.TextField("Body Name", jsonVar.bodyName);
        EditorGUILayout.Space();
        jsonVar.wheelFrontLeftName = EditorGUILayout.TextField("Wheel Front Left Name", jsonVar.wheelFrontLeftName);
        jsonVar.wheelFrontRightName = EditorGUILayout.TextField("Wheel Front Right Name", jsonVar.wheelFrontRightName);
        jsonVar.wheelRearLeftName = EditorGUILayout.TextField("Wheel Rear Left Name", jsonVar.wheelRearLeftName);
        jsonVar.wheelRearRightName = EditorGUILayout.TextField("Wheel Rear Right Name", jsonVar.wheelRearRightName);
        EditorGUILayout.Space();
        jsonVar.headLightMaterialName = EditorGUILayout.TextField("HeadLight Material Name", jsonVar.headLightMaterialName);
        jsonVar.turnLightMaterialName = EditorGUILayout.TextField("TurnLight Material Name", jsonVar.turnLightMaterialName);
        jsonVar.tailLightMaterialName = EditorGUILayout.TextField("TailLight Material Name", jsonVar.tailLightMaterialName);
        EditorGUILayout.Space();
        jsonVar.lightProperty = EditorGUILayout.TextField("Light Property", jsonVar.lightProperty);
        if (GUILayout.Button("Add Car Manager")) AddCarManager();
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);

        ColorLabelField("04. Change Wheel Transforms", EditorStyles.boldLabel, mainLabelColor);
        ++EditorGUI.indentLevel;
        if (GUILayout.Button("Change Wheel Transforms")) ChangeWheelTransforms();
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);


        ColorLabelField("05. Create Blob Shadow", EditorStyles.boldLabel, mainLabelColor);
        ++EditorGUI.indentLevel;
        jsonVar.blobShadow = (GameObject)EditorGUILayout.ObjectField("Blow Shadow", jsonVar.blobShadow, typeof(GameObject), true);
        jsonVar.blobShadowPosition = EditorGUILayout.Vector3Field("Blob Shadow Position", jsonVar.blobShadowPosition);
        jsonVar.blobShadowRotation = EditorGUILayout.Vector3Field("Blob Shadow Rotation", jsonVar.blobShadowRotation);
        jsonVar.blobShadowScale = EditorGUILayout.Vector3Field("Blob Shadow Scale", jsonVar.blobShadowScale);
        if (GUILayout.Button("Create Blob Shadow")) CreateBlobShadow();
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);

        ColorLabelField("06. Create Paint Info Objects", EditorStyles.boldLabel, mainLabelColor);
        ++EditorGUI.indentLevel;
        jsonVar.enableSelectionObjects = EditorGUILayout.Foldout(jsonVar.enableSelectionObjects, "Selection Objects", true);
        if (jsonVar.enableSelectionObjects)
        {
            ++EditorGUI.indentLevel;
            for (int i = 0, l = Selection.objects.Length; i < l; ++i) EditorGUILayout.LabelField(string.Format(" {0:00} : {1}", i, Selection.objects[i].name), EditorStyles.largeLabel);
            --EditorGUI.indentLevel;
        }
        if (GUILayout.Button("Create Paint Info Objects")) CreatePaintInfoObjects();
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);

        ColorLabelField("07. Add Paint Info", EditorStyles.boldLabel, mainLabelColor);
        ++EditorGUI.indentLevel;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<")) ChangeSelectedPaintInfoObject(false);
        if (GUILayout.Button(">")) ChangeSelectedPaintInfoObject(true);
        EditorGUILayout.LabelField(GetSelectedObjectAndPaintInfo());
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        jsonVar.selectedPaintInfo = GUILayout.Toolbar(jsonVar.selectedPaintInfo, Enum.GetNames(typeof(PaintPanel)));
        if (GUILayout.Button("Add Paint Info")) AddPaintInfo((PaintPanel)jsonVar.selectedPaintInfo);
        jsonVar.enablePaintInfo = EditorGUILayout.Foldout(jsonVar.enablePaintInfo, "Paint Info", true);
        EditorGUILayout.Space();
        if (jsonVar.enablePaintInfo)
        {
            ++EditorGUI.indentLevel;
            OnGUIPaintInfo((PaintPanel)jsonVar.selectedPaintInfo);
            --EditorGUI.indentLevel;
        }
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);

        ColorLabelField("07-A. Define Part Name", EditorStyles.boldLabel, new Color(0.2f, 0.2f, 0f));
        ++EditorGUI.indentLevel;
        jsonVar.enablePartName = EditorGUILayout.Foldout(jsonVar.enablePartName, "Part Name", true);
        if (jsonVar.enablePartName)
        {
            ++EditorGUI.indentLevel;
            OnGUIPartName();
            --EditorGUI.indentLevel;
        }
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);

        ColorLabelField("08. Camera View Point Utility", EditorStyles.boldLabel, mainLabelColor);
        ++EditorGUI.indentLevel;
        if (GUILayout.Button("Select Part Material")) SelectPartMaterial();
        GUILayout.BeginHorizontal();
        jsonVar.setMultiple = EditorGUILayout.Toggle("□ Set Multiple", jsonVar.setMultiple);
        jsonVar.positionMultiple = EditorGUILayout.FloatField("Position", jsonVar.positionMultiple);
        jsonVar.rotationMultiple = EditorGUILayout.FloatField("Rotation", jsonVar.rotationMultiple);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        jsonVar.createViewPointCamera = EditorGUILayout.Toggle("□ Create View Point Camera", jsonVar.createViewPointCamera);
        if (GUILayout.Button("Destroy View Point Camera")) DestroyCamera(viewPointCameraName);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Set Camera View Point")) SetCameraViewPoint();
        --EditorGUI.indentLevel;
        EditorGUILayout.Space(spaceHeight);

        EditorGUILayout.EndScrollView();
    }

    #region Utility
    private void ColorLabelField(string label, GUIStyle style, Color color)
    {
        GUIStyleState styleState = style.normal;
        Color boldLabelDefaultColor = styleState.textColor;
        styleState.textColor = color;
        EditorGUILayout.LabelField(label, style);
        styleState.textColor = boldLabelDefaultColor;
    }
    #endregion

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

    #region 02. Create Paint Infos
    /* Paint Infos를 생성하는 함수 */
    private void CreatePaintInfos()
    {
        if (!jsonVar.carPrefab) return;
        Transform carTransform = jsonVar.carPrefab.transform;
        Transform paintInfosTransform = carTransform.Find(jsonVar.paintInfosName);
        if (!paintInfosTransform)
        {
            paintInfosTransform = new GameObject(jsonVar.paintInfosName).transform;
            paintInfosTransform.SetParent(carTransform, false);
            paintInfosTransform.SetAsFirstSibling();
        }
    }
    #endregion

    #region 03. Add Car Manager
    /* Car Manager 컴포넌트를 추가하는 함수 */
    private void AddCarManager()
    {
        if (!jsonVar.carPrefab) return;
        CarManager carManager = jsonVar.carPrefab.GetComponent<CarManager>();
        if (!carManager) carManager = jsonVar.carPrefab.AddComponent<CarManager>();

        Transform carTransform = jsonVar.carPrefab.transform;

        SerializedObject carManagerSO = new SerializedObject(carManager);
        carManagerSO.FindProperty("paintInfoTransform").objectReferenceValue = carTransform.Find(jsonVar.paintInfosName);

        carManagerSO.FindProperty("heightRange").vector2Value = jsonVar.heightRange;
        carManagerSO.FindProperty("camberAngleRange").vector2Value = jsonVar.camberAngleRange;
        carManagerSO.FindProperty("wheelProtrusionRange").vector2Value = jsonVar.wheelProtrusionRange;

        carManagerSO.FindProperty("bodyTransform").objectReferenceValue = carTransform.Find(jsonVar.bodyName);
        carManagerSO.FindProperty("wheelFrontLeftTransform").objectReferenceValue = carTransform.Find(jsonVar.wheelFrontLeftName);
        carManagerSO.FindProperty("wheelFrontRightTransform").objectReferenceValue = carTransform.Find(jsonVar.wheelFrontRightName);
        carManagerSO.FindProperty("wheelRearLeftTransform").objectReferenceValue = carTransform.Find(jsonVar.wheelRearLeftName);
        carManagerSO.FindProperty("wheelRearRightTransform").objectReferenceValue = carTransform.Find(jsonVar.wheelRearRightName);

        MeshRenderer lightMeshRenderer = carTransform.Find(jsonVar.bodyName).GetChild(0).GetComponent<MeshRenderer>();
        carManagerSO.FindProperty("lightMeshRenderer").objectReferenceValue = lightMeshRenderer;

        SerializedProperty lightMaterialNamesSP = carManagerSO.FindProperty("lightMaterialNames");
        lightMaterialNamesSP.ClearArray();

        if (lightMeshRenderer) InsertLightMaterialNames(lightMaterialNamesSP, MaterialNamesContainWord(lightMeshRenderer.sharedMaterials, jsonVar.headLightMaterialName));
        if (lightMeshRenderer) InsertLightMaterialNames(lightMaterialNamesSP, MaterialNamesContainWord(lightMeshRenderer.sharedMaterials, jsonVar.turnLightMaterialName));
        if (lightMeshRenderer) InsertLightMaterialNames(lightMaterialNamesSP, MaterialNamesContainWord(lightMeshRenderer.sharedMaterials, jsonVar.tailLightMaterialName));

        carManagerSO.FindProperty("lightProperty").stringValue = jsonVar.lightProperty;

        carManagerSO.ApplyModifiedProperties();
    }

    /* Light Material Name들을 삽입하는 함수 */
    private void InsertLightMaterialNames(SerializedProperty lightMaterialNamesSP, string[] lightMaterialNames)
    {
        SortStringsByDictionary(ref lightMaterialNames);

        for (int i = 0; i < lightMaterialNames.Length; ++i)
        {
            lightMaterialNamesSP.arraySize += 1;
            lightMaterialNamesSP.GetArrayElementAtIndex(lightMaterialNamesSP.arraySize - 1).stringValue = lightMaterialNames[i];
        }
    }

    /* Material들중 찾고자하는 단어를 포함하는 이름을 반환하는 함수 */
    private string[] MaterialNamesContainWord(Material[] materials, string word)
    {
        List<string> result = new List<string>();

        for (int i = 0; i < materials.Length; ++i)
        {
            string materialName = materials[i].name;
            if (materialName.Contains(word)) result.Add(materialName);
        }

        return result.ToArray();
    }

    /* 문자열들을 사전 순으로 정렬하는 함수 */
    private void SortStringsByDictionary(ref string[] str)
    {
        List<string> listStr = str.ToList();
        listStr.Sort();
        str = listStr.ToArray();
    }
    #endregion

    #region 04. Change Wheel Transforms
    /* Wheel Transform들을 변경하는 함수 */
    private void ChangeWheelTransforms()
    {
        if (!jsonVar.carPrefab) return;

        Transform carTransform = jsonVar.carPrefab.transform;

        Transform wheelFrontLeft = carTransform.Find(jsonVar.wheelFrontLeftName);
        Transform wheelFrontRight = carTransform.Find(jsonVar.wheelFrontRightName);
        Transform wheelRearLeft = carTransform.Find(jsonVar.wheelRearLeftName);
        Transform wheelRearRight = carTransform.Find(jsonVar.wheelRearRightName);

        Transform wheelFrontLeftChild = wheelFrontLeft.GetChild(0);
        Transform wheelFrontRightChild = wheelFrontRight.GetChild(0);
        Transform wheelRearLeftChild = wheelRearLeft.GetChild(0);
        Transform wheelRearRightChild = wheelRearRight.GetChild(0);

        /* 자식의 부모 해제 */
        wheelFrontLeftChild.SetParent(wheelFrontLeft.parent, true);
        wheelFrontRightChild.SetParent(wheelFrontRight.parent, true);
        wheelRearLeftChild.SetParent(wheelRearLeft.parent, true);
        wheelRearRightChild.SetParent(wheelRearRight.parent, true);

        /* 부모의 위치 변경 */
        wheelFrontLeft.position = wheelFrontLeftChild.position;
        wheelFrontRight.position = wheelFrontRightChild.position;
        wheelRearLeft.position = wheelRearLeftChild.position;
        wheelRearRight.position = wheelRearRightChild.position;

        /* 부모의 회전 변경 */
        wheelFrontLeft.localRotation = Quaternion.Euler(0f, 180f, 0f);
        wheelFrontRight.localRotation = Quaternion.Euler(0f, 0f, 0f);
        wheelRearLeft.localRotation = Quaternion.Euler(0f, 180f, 0f);
        wheelRearRight.localRotation = Quaternion.Euler(0f, 0f, 0f);

        /* 부모의 크기 변경 */
        wheelFrontLeft.localScale = Vector3.one;
        wheelFrontRight.localScale = Vector3.one;
        wheelRearLeft.localScale = Vector3.one;
        wheelRearRight.localScale = Vector3.one;

        /* 자식의 부모 적용 */
        wheelFrontLeftChild.SetParent(wheelFrontLeft, true);
        wheelFrontRightChild.SetParent(wheelFrontRight, true);
        wheelRearLeftChild.SetParent(wheelRearLeft, true);
        wheelRearRightChild.SetParent(wheelRearRight, true);
    }
    #endregion

    #region 05. Create Blob Shadow
    /* Blob Shadow를 생성하는 함수 */
    private void CreateBlobShadow()
    {
        if (!jsonVar.carPrefab) return;

        Transform blobShadowTransform = jsonVar.carPrefab.transform.Find(jsonVar.blobShadow.name);
        if (!blobShadowTransform)
        {
            GameObject blobShadow = Instantiate(jsonVar.blobShadow);
            blobShadow.name = jsonVar.blobShadow.name;
            blobShadowTransform = blobShadow.transform;
            blobShadowTransform.SetParent(jsonVar.carPrefab.transform, true);
            blobShadowTransform.localPosition = jsonVar.blobShadowPosition;
            blobShadowTransform.localRotation = Quaternion.Euler(jsonVar.blobShadowRotation);
            blobShadowTransform.localScale = jsonVar.blobShadowScale;
        }
    }
    #endregion

    #region 06. Create Paint Info Objects
    /* Paint Info 오브젝트들을 생성하는 함수 */
    private void CreatePaintInfoObjects()
    {
        if (!jsonVar.carPrefab) return;
        Transform paintInfosTransform = jsonVar.carPrefab.transform.Find(jsonVar.paintInfosName);
        if (!paintInfosTransform) return;

        while (paintInfosTransform.childCount > 0) DestroyImmediate(paintInfosTransform.GetChild(0).gameObject);

        for (int i = 0, l = Selection.objects.Length; i < l; ++i)
        {
            GameObject paintInfo = new GameObject(Selection.objects[i].name);
            paintInfo.transform.SetParent(paintInfosTransform, true);
        }
    }
    #endregion

    #region 07. Add Paint Info
    /* 선택된 Paint Info 오브젝트를 변경하는 함수 */
    private void ChangeSelectedPaintInfoObject(bool isNext)
    {
        if (!Selection.activeTransform || !Selection.activeTransform.parent) return;

        Transform selection = Selection.activeTransform;
        Transform selectionParent = Selection.activeTransform.parent;

        if (isNext) Selection.activeTransform = selectionParent.GetChild((selection.GetSiblingIndex() + 1) % selectionParent.childCount);
        else Selection.activeTransform = selectionParent.GetChild(selection.GetSiblingIndex() - 1 < 0 ? selectionParent.childCount - 1 : selection.GetSiblingIndex() - 1);
    }

    /* 선택된 오브젝트의 이름과 Paint Info를 반환하는 함수 */
    private string GetSelectedObjectAndPaintInfo()
    {
        string result = null;

        if (Selection.activeGameObject)
        {
            result += Selection.activeGameObject.name;

            PaintInfo paintInfo = Selection.activeGameObject.GetComponent<PaintInfo>();

            if (paintInfo)
            {
                result += string.Format(" ▶ {0}", paintInfo.GetType());
            }
        }

        return result;
    }

    /* Paint Info를 추가하는 함수 */
    private void AddPaintInfo(PaintPanel paintPanel)
    {
        GameObject activeGameObject = Selection.activeGameObject;
        if (!activeGameObject) return;

        PaintInfo[] paintInfos = activeGameObject.GetComponents<PaintInfo>();
        for (int i = 0, l = paintInfos.Length; i < l; ++i) DestroyImmediate(paintInfos[i]);

        if (paintPanel == PaintPanel.AllColorSurface)
        {
            AllColorSurfaceInfo allColorSurfaceInfo = activeGameObject.AddComponent<AllColorSurfaceInfo>();

            if (jsonVar.allColorSurfaceInfoPresets.Length < 1) return;

            SerializedObject so = new SerializedObject(allColorSurfaceInfo);
            SerializedProperty sp;

            so.FindProperty("paintCategory").enumValueIndex = (int)jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].paintCategory;
            so.FindProperty("partName").stringValue = GetPartNameKOR(activeGameObject.name);
            so.FindProperty("cameraViewPointTransform").objectReferenceValue = Selection.activeTransform;
            so.FindProperty("cameraViewPointOffset").vector3Value = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].cameraViewPointOffset;
            try { so.FindProperty("partMeshRenderer").objectReferenceValue = jsonVar.carPrefab.transform.Find(jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partMeshRendererName).GetChild(0).GetComponent<MeshRenderer>(); } catch { }
            so.FindProperty("partMaterialName").stringValue = activeGameObject.name;
            so.FindProperty("partColorProperty").stringValue = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partColorProperty;

            sp = so.FindProperty("partSurfaceNames");
            sp.arraySize = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames.Length;
            for (int i = 0, l = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames.Length; i < l; ++i)
                sp.GetArrayElementAtIndex(i).stringValue = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames[i];

            sp = so.FindProperty("partSurfaceShaders");
            sp.arraySize = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders.Length;
            for (int i = 0, l = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders.Length; i < l; ++i)
                sp.GetArrayElementAtIndex(i).objectReferenceValue = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders[i];

            sp = so.FindProperty("colorSurfaceInfos");
            sp.arraySize = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames.Length;
            for (int i = 0, l = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames.Length; i < l; ++i) try { sp.GetArrayElementAtIndex(i).objectReferenceValue = jsonVar.carPrefab.transform.Find(jsonVar.paintInfosName).Find(jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames[i]).GetComponent<ColorSurfaceInfo>(); } catch { }

            so.ApplyModifiedProperties();
        }
        else if (paintPanel == PaintPanel.ColorSurface)
        {
            ColorSurfaceInfo colorSurfaceInfo = activeGameObject.AddComponent<ColorSurfaceInfo>();

            if (jsonVar.colorSurfaceInfoPresets.Length < 1) return;

            SerializedObject so = new SerializedObject(colorSurfaceInfo);
            SerializedProperty sp;

            so.FindProperty("paintCategory").enumValueIndex = (int)jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].paintCategory;
            so.FindProperty("partName").stringValue = GetPartNameKOR(activeGameObject.name);
            so.FindProperty("cameraViewPointTransform").objectReferenceValue = Selection.activeTransform;
            so.FindProperty("cameraViewPointOffset").vector3Value = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].cameraViewPointOffset;
            try { so.FindProperty("partMeshRenderer").objectReferenceValue = jsonVar.carPrefab.transform.Find(jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partMeshRendererName).GetChild(0).GetComponent<MeshRenderer>(); } catch { }
            so.FindProperty("partMaterialName").stringValue = activeGameObject.name;
            so.FindProperty("partColorProperty").stringValue = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partColorProperty;

            sp = so.FindProperty("partSurfaceNames");
            sp.arraySize = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames.Length;
            for (int i = 0, l = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames.Length; i < l; ++i)
                sp.GetArrayElementAtIndex(i).stringValue = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames[i];

            sp = so.FindProperty("partSurfaceShaders");
            sp.arraySize = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders.Length;
            for (int i = 0, l = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders.Length; i < l; ++i)
                sp.GetArrayElementAtIndex(i).objectReferenceValue = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders[i];

            so.ApplyModifiedProperties();
        }
        else if (paintPanel == PaintPanel.Color)
        {
            ColorInfo colorInfo = activeGameObject.AddComponent<ColorInfo>();

            if (jsonVar.colorInfoPresets.Length < 1) return;

            SerializedObject so = new SerializedObject(colorInfo);

            so.FindProperty("paintCategory").enumValueIndex = (int)jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].paintCategory;
            so.FindProperty("partName").stringValue = GetPartNameKOR(activeGameObject.name);
            so.FindProperty("cameraViewPointTransform").objectReferenceValue = Selection.activeTransform;
            so.FindProperty("cameraViewPointOffset").vector3Value = jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].cameraViewPointOffset;
            try { so.FindProperty("partMeshRenderer").objectReferenceValue = jsonVar.carPrefab.transform.Find(jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].partMeshRendererName).GetChild(0).GetComponent<MeshRenderer>(); } catch { }
            so.FindProperty("partMaterialName").stringValue = activeGameObject.name;
            so.FindProperty("partColorProperty").stringValue = jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].partColorProperty;

            so.ApplyModifiedProperties();
        }
        else if (paintPanel == PaintPanel.Windshield)
        {
            WindshieldInfo windshieldInfo = activeGameObject.AddComponent<WindshieldInfo>();

            if (jsonVar.windshieldInfoPresets.Length < 1) return;

            SerializedObject so = new SerializedObject(windshieldInfo);

            so.FindProperty("paintCategory").enumValueIndex = (int)jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].paintCategory;
            so.FindProperty("partName").stringValue = GetPartNameKOR(activeGameObject.name);
            so.FindProperty("cameraViewPointTransform").objectReferenceValue = Selection.activeTransform;
            so.FindProperty("cameraViewPointOffset").vector3Value = jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].cameraViewPointOffset;
            try { so.FindProperty("partMeshRenderer").objectReferenceValue = jsonVar.carPrefab.transform.Find(jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partMeshRendererName).GetChild(0).GetComponent<MeshRenderer>(); } catch { }
            so.FindProperty("partMaterialName").stringValue = activeGameObject.name;
            so.FindProperty("partColorProperty").stringValue = jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partColorProperty;
            so.FindProperty("partTintingProperty").stringValue = jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partTintingProperty;

            so.ApplyModifiedProperties();
        }
    }

    /* Paint Info를 GUI에 나타내는 함수 */
    private void OnGUIPaintInfo(PaintPanel paintPanel)
    {
        if (PaintPanel.AllColorSurface == paintPanel)
        {
            /** Preset **/
            EditorGUILayout.BeginHorizontal();

            if (jsonVar.allColorSurfaceInfoPresets.Length > 0)
                jsonVar.selectedAllColorSurfaceInfoPreset = EditorGUILayout.IntPopup(jsonVar.selectedAllColorSurfaceInfoPreset, jsonVar.allColorSurfaceInfoPresetNames, jsonVar.allColorSurfaceInfoPresetSizes);

            if (GUILayout.Button("Add"))
            {
                ArrayUtility.Add(ref jsonVar.allColorSurfaceInfoPresets, new JsonVar.AllColorSurfaceInfoPreset("New"));
                jsonVar.selectedAllColorSurfaceInfoPreset = jsonVar.allColorSurfaceInfoPresets.Length - 1;
            }
            if (GUILayout.Button("Delete"))
            {
                if (jsonVar.allColorSurfaceInfoPresets.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.allColorSurfaceInfoPresets, jsonVar.selectedAllColorSurfaceInfoPreset);//Preset 제거
                jsonVar.selectedAllColorSurfaceInfoPreset = jsonVar.allColorSurfaceInfoPresets.Length - 1;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            /** Variable **/
            if (jsonVar.allColorSurfaceInfoPresets.Length > 0)
            {
                EditorGUILayout.BeginHorizontal();
                jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].presetName = EditorGUILayout.TextField(jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].presetName);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].paintCategory = (PaintCategory)EditorGUILayout.EnumPopup("Paint Category", jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].paintCategory);
                jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].cameraViewPointOffset = EditorGUILayout.Vector3Field("Camera View Point Offset", jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].cameraViewPointOffset);
                jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partMeshRendererName = EditorGUILayout.TextField("Part Mesh Renderer Name", jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partMeshRendererName);
                jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partColorProperty = EditorGUILayout.TextField("Part Color Property", jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partColorProperty);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Part Surface Names");
                if (GUILayout.Button("Reset")) { ArrayUtility.Clear(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames); }
                if (GUILayout.Button("+")) { ArrayUtility.Add(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames, null); }
                if (GUILayout.Button("-")) { if (jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames, jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames.Length - 1); }
                EditorGUILayout.LabelField(jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames.Length.ToString());
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                for (int i = 0, l = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames.Length; i < l; ++i)
                    jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames[i] = EditorGUILayout.TextField(string.Format("\t{0}", i), jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceNames[i]);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Part Surface Shaders");
                if (GUILayout.Button("Reset")) { ArrayUtility.Clear(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders); }
                if (GUILayout.Button("+")) { ArrayUtility.Add(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders, null); }
                if (GUILayout.Button("-")) { if (jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders, jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders.Length - 1); }
                EditorGUILayout.LabelField(jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders.Length.ToString());
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                for (int i = 0, l = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders.Length; i < l; ++i)
                    jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders[i] = (Shader)EditorGUILayout.ObjectField(string.Format("\t{0}", i), jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].partSurfaceShaders[i], typeof(Shader), false);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Color Surface Info Names");
                if (GUILayout.Button("Reset")) { ArrayUtility.Clear(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames); }
                if (GUILayout.Button("+")) { ArrayUtility.Add(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames, null); }
                if (GUILayout.Button("-")) { if (jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames, jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames.Length - 1); }
                EditorGUILayout.LabelField(jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames.Length.ToString());
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                for (int i = 0, l = jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames.Length; i < l; ++i)
                    jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames[i] = EditorGUILayout.TextField(string.Format("\t{0}", i), jsonVar.allColorSurfaceInfoPresets[jsonVar.selectedAllColorSurfaceInfoPreset].colorSurfaceInfoNames[i]);
            }
        }
        else if (PaintPanel.ColorSurface == paintPanel)
        {
            /** Preset **/
            EditorGUILayout.BeginHorizontal();

            if (jsonVar.colorSurfaceInfoPresets.Length > 0)
                jsonVar.selectedColorSurfaceInfoPreset = EditorGUILayout.IntPopup(jsonVar.selectedColorSurfaceInfoPreset, jsonVar.colorSurfaceInfoPresetNames, jsonVar.colorSurfaceInfoPresetSizes);

            if (GUILayout.Button("Add"))
            {
                ArrayUtility.Add(ref jsonVar.colorSurfaceInfoPresets, new JsonVar.ColorSurfaceInfoPreset("New"));
                jsonVar.selectedColorSurfaceInfoPreset = jsonVar.colorSurfaceInfoPresets.Length - 1;
            }
            if (GUILayout.Button("Delete"))
            {
                if (jsonVar.colorSurfaceInfoPresets.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.colorSurfaceInfoPresets, jsonVar.selectedColorSurfaceInfoPreset);//Preset 제거
                jsonVar.selectedColorSurfaceInfoPreset = jsonVar.colorSurfaceInfoPresets.Length - 1;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            /** Variable **/
            if (jsonVar.colorSurfaceInfoPresets.Length > 0)
            {
                EditorGUILayout.BeginHorizontal();
                jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].presetName = EditorGUILayout.TextField(jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].presetName);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].paintCategory = (PaintCategory)EditorGUILayout.EnumPopup("Paint Category", jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].paintCategory);
                jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].cameraViewPointOffset = EditorGUILayout.Vector3Field("Camera View Point Offset", jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].cameraViewPointOffset);
                jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partMeshRendererName = EditorGUILayout.TextField("Part Mesh Renderer Name", jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partMeshRendererName);
                jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partColorProperty = EditorGUILayout.TextField("Part Color Property", jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partColorProperty);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Part Surface Names");
                if (GUILayout.Button("Reset")) { ArrayUtility.Clear(ref jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames); }
                if (GUILayout.Button("+")) { ArrayUtility.Add(ref jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames, null); }
                if (GUILayout.Button("-")) { if (jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames, jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames.Length - 1); }
                EditorGUILayout.LabelField(jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames.Length.ToString());
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                for (int i = 0, l = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames.Length; i < l; ++i)
                    jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames[i] = EditorGUILayout.TextField(string.Format("\t{0}", i), jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceNames[i]);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Part Surface Shaders");
                if (GUILayout.Button("Reset")) { ArrayUtility.Clear(ref jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders); }
                if (GUILayout.Button("+")) { ArrayUtility.Add(ref jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders, null); }
                if (GUILayout.Button("-")) { if (jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders, jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders.Length - 1); }
                EditorGUILayout.LabelField(jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders.Length.ToString());
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                for (int i = 0, l = jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders.Length; i < l; ++i)
                    jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders[i] = (Shader)EditorGUILayout.ObjectField(string.Format("\t{0}", i), jsonVar.colorSurfaceInfoPresets[jsonVar.selectedColorSurfaceInfoPreset].partSurfaceShaders[i], typeof(Shader), false);
            }
        }
        else if (PaintPanel.Color == paintPanel)
        {
            /** Preset **/
            EditorGUILayout.BeginHorizontal();

            if (jsonVar.colorInfoPresets.Length > 0)
                jsonVar.selectedColorInfoPreset = EditorGUILayout.IntPopup(jsonVar.selectedColorInfoPreset, jsonVar.colorInfoPresetNames, jsonVar.colorInfoPresetSizes);

            if (GUILayout.Button("Add"))
            {
                ArrayUtility.Add(ref jsonVar.colorInfoPresets, new JsonVar.ColorInfoPreset("New"));
                jsonVar.selectedColorInfoPreset = jsonVar.colorInfoPresets.Length - 1;
            }
            if (GUILayout.Button("Delete"))
            {
                if (jsonVar.colorInfoPresets.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.colorInfoPresets, jsonVar.selectedColorInfoPreset);//Preset 제거
                jsonVar.selectedColorInfoPreset = jsonVar.colorInfoPresets.Length - 1;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            /** Variable **/
            if (jsonVar.colorInfoPresets.Length > 0)
            {
                EditorGUILayout.BeginHorizontal();
                jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].presetName = EditorGUILayout.TextField(jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].presetName);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].paintCategory = (PaintCategory)EditorGUILayout.EnumPopup("Paint Category", jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].paintCategory);
                jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].cameraViewPointOffset = EditorGUILayout.Vector3Field("Camera View Point Offset", jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].cameraViewPointOffset);
                jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].partMeshRendererName = EditorGUILayout.TextField("Part Mesh Renderer Name", jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].partMeshRendererName);
                jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].partColorProperty = EditorGUILayout.TextField("Part Color Property", jsonVar.colorInfoPresets[jsonVar.selectedColorInfoPreset].partColorProperty);
            }
        }
        else if (PaintPanel.Windshield == paintPanel)
        {
            /** Preset **/
            EditorGUILayout.BeginHorizontal();

            if (jsonVar.windshieldInfoPresets.Length > 0)
                jsonVar.selectedWindshieldInfoPreset = EditorGUILayout.IntPopup(jsonVar.selectedWindshieldInfoPreset, jsonVar.windshieldInfoPresetNames, jsonVar.windshieldInfoPresetSizes);

            if (GUILayout.Button("Add"))
            {
                ArrayUtility.Add(ref jsonVar.windshieldInfoPresets, new JsonVar.WindshieldInfoPreset("New"));
                jsonVar.selectedWindshieldInfoPreset = jsonVar.windshieldInfoPresets.Length - 1;
            }
            if (GUILayout.Button("Delete"))
            {
                if (jsonVar.windshieldInfoPresets.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.windshieldInfoPresets, jsonVar.selectedWindshieldInfoPreset);//Preset 제거
                jsonVar.selectedWindshieldInfoPreset = jsonVar.windshieldInfoPresets.Length - 1;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            /** Variable **/
            if (jsonVar.windshieldInfoPresets.Length > 0)
            {
                EditorGUILayout.BeginHorizontal();
                jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].presetName = EditorGUILayout.TextField(jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].presetName);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].paintCategory = (PaintCategory)EditorGUILayout.EnumPopup("Paint Category", jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].paintCategory);
                jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].cameraViewPointOffset = EditorGUILayout.Vector3Field("Camera View Point Offset", jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].cameraViewPointOffset);
                jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partMeshRendererName = EditorGUILayout.TextField("Part Mesh Renderer Name", jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partMeshRendererName);
                jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partColorProperty = EditorGUILayout.TextField("Part Color Property", jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partColorProperty);
                jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partTintingProperty = EditorGUILayout.TextField("Part Tinting Property", jsonVar.windshieldInfoPresets[jsonVar.selectedWindshieldInfoPreset].partTintingProperty);
            }
        }
    }
    #endregion

    #region 07-A. Define Part Name
    private void OnGUIPartName()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset"))
        {
            ArrayUtility.Clear(ref jsonVar.partNamesENG);
            ArrayUtility.Clear(ref jsonVar.partNamesKOR);
        }
        if (GUILayout.Button("+"))
        {
            ArrayUtility.Add(ref jsonVar.partNamesENG, null);
            ArrayUtility.Add(ref jsonVar.partNamesKOR, null);
        }
        if (GUILayout.Button("-"))
        {
            if (jsonVar.partNamesENG.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.partNamesENG, jsonVar.partNamesENG.Length - 1);
            if (jsonVar.partNamesKOR.Length > 0) ArrayUtility.RemoveAt(ref jsonVar.partNamesKOR, jsonVar.partNamesKOR.Length - 1);
        }
        EditorGUILayout.LabelField(jsonVar.partNamesENG.Length.ToString());
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        for (int i = 0; i < jsonVar.partNamesENG.Length; ++i)
        {
            GUILayout.BeginHorizontal();
            jsonVar.partNamesENG[i] = EditorGUILayout.TextField(jsonVar.partNamesENG[i]);
            jsonVar.partNamesKOR[i] = EditorGUILayout.TextField(jsonVar.partNamesKOR[i]);

            if (GUILayout.Button("+"))
            {
                ArrayUtility.Insert(ref jsonVar.partNamesENG, i + 1, null);
                ArrayUtility.Insert(ref jsonVar.partNamesKOR, i + 1, null);
            }

            if (GUILayout.Button("-"))
            {
                ArrayUtility.RemoveAt(ref jsonVar.partNamesENG, i);
                ArrayUtility.RemoveAt(ref jsonVar.partNamesKOR, i);
            }
            GUILayout.EndHorizontal();
        }
    }

    private string GetPartNameKOR(string partNameENG)
    {
        int index = ArrayUtility.FindIndex(jsonVar.partNamesENG, (string value) => { return value.Replace(" ", "").ToLower() == partNameENG.Replace(" ", "").ToLower(); });
        return index == -1 ? null : jsonVar.partNamesKOR[index];
    }
    #endregion

    #region 08. Camera View Point Utility
    private void SelectPartMaterial()
    {
        GameObject activeGameObject = Selection.activeGameObject;
        if (!activeGameObject) return;

        PaintInfo paintInfo = Selection.activeGameObject.GetComponent<PaintInfo>();
        if (!paintInfo) return;

        SerializedObject so = new SerializedObject(paintInfo);
        MeshRenderer meshRenderer = (MeshRenderer)so.FindProperty("partMeshRenderer").objectReferenceValue;
        if (!meshRenderer) return;

        Selection.activeObject = Utility.FindMaterialWithName(meshRenderer.sharedMaterials, so.FindProperty("partMaterialName").stringValue);
    }

    private Vector3 GetMultipleValue(Vector3 value, float multiple)
    {
        if (Mathf.Abs(value.x) % multiple < multiple * 0.5f) value.x += ((value.x > 0) ? -1 : 1) * Mathf.Abs(value.x) % multiple;
        else value.x += ((value.x > 0) ? 1 : -1) * (multiple - Mathf.Abs(value.x) % multiple);

        if (Mathf.Abs(value.y) % multiple < multiple * 0.5f) value.y += ((value.y > 0) ? -1 : 1) * Mathf.Abs(value.y) % multiple;
        else value.y += ((value.y > 0) ? 1 : -1) * (multiple - Mathf.Abs(value.y) % multiple);

        if (Mathf.Abs(value.z) % multiple < multiple * 0.5f) value.z += ((value.z > 0) ? -1 : 1) * Mathf.Abs(value.z) % multiple;
        else value.z += ((value.z > 0) ? 1 : -1) * (multiple - Mathf.Abs(value.z) % multiple);

        return value;
    }

    private void CreateCamera(Transform cameraTarget, Vector3 offset, string cameraName)
    {
        if (!cameraTarget) return;

        GameObject camera = GameObject.Find(cameraName);
        if (!camera)
        {
            camera = new GameObject(cameraName);
            Camera cameraCamera = camera.AddComponent<Camera>();
            cameraCamera.depth = 100;
        }

        camera.transform.SetPositionAndRotation(cameraTarget.TransformPoint(offset), cameraTarget.rotation);
    }

    private void DestroyCamera(string cameraName)
    {
        GameObject camera = GameObject.Find(cameraName);
        if (camera) DestroyImmediate(camera);
    }

    private void SetCameraViewPoint()
    {
        Transform activeTransform = Selection.activeTransform;
        if (!activeTransform) return;

        Transform sceneViewCamera = SceneView.lastActiveSceneView.camera.transform;

        if (jsonVar.setMultiple)
        {
            activeTransform.localPosition = GetMultipleValue(sceneViewCamera.localPosition, jsonVar.positionMultiple);
            activeTransform.localEulerAngles = GetMultipleValue(sceneViewCamera.localEulerAngles, jsonVar.rotationMultiple);
        }
        else
        {
            activeTransform.localPosition = sceneViewCamera.localPosition;
            activeTransform.localEulerAngles = sceneViewCamera.localEulerAngles;
        }

        if (jsonVar.createViewPointCamera)
        {
            PaintInfo paintInfo = activeTransform.GetComponent<PaintInfo>();
            CreateCamera(activeTransform, paintInfo ? new SerializedObject(paintInfo).FindProperty("cameraViewPointOffset").vector3Value : Vector3.zero, viewPointCameraName);
        }
    }
    #endregion
}
#endif