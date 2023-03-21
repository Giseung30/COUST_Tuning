#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public class PackageManager
{
    #region Build Package
    [MenuItem("Custom/Package/Build Package_PC")] //메뉴로써 제작
    /* 패키지 제작하는 함수 - PC */
    public static void BuildPackageForPC()
    {
        /* Bundles */
        string bundlesDirectory = "Assets/Package_PC/Bundles"; //Bundle들을 저장할 폴더의 위치
        if (!Directory.Exists(bundlesDirectory)) //폴더가 존재하지 않으면
        {
            Directory.CreateDirectory(bundlesDirectory); //폴더 생성
        }
        BuildPipeline.BuildAssetBundles(bundlesDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64); //번들 제작

        /* Images */
        string imagesDirectory = "Assets/Package_PC/Images"; //Image들을 저장할 폴더의 위치
        string imagesTempDirectory = "Assets/Temps/Package/Images"; //Image들을 담은 폴더의 위치

        CopyImagesDirectory(imagesTempDirectory, imagesDirectory); //폴더 복사

        AssetDatabase.Refresh(); //에셋 초기화
    }

    [MenuItem("Custom/Package/Build Package_Android")] //메뉴로써 제작
    /* 패키지 제작하는 함수 - Android */
    public static void BuildPackageForAndroid()
    {
        /* Bundles */
        string bundlesDirectory = "Assets/Package_Android/Bundles"; //Bundle들을 저장할 폴더의 위치
        if (!Directory.Exists(bundlesDirectory)) //폴더가 존재하지 않으면
        {
            Directory.CreateDirectory(bundlesDirectory); //폴더 생성
        }
        BuildPipeline.BuildAssetBundles(bundlesDirectory, BuildAssetBundleOptions.None, BuildTarget.Android); //번들 제작

        /* Images */
        string imagesDirectory = "Assets/Package_Android/Images"; //Image들을 저장할 폴더의 위치
        string imagesTempDirectory = "Assets/Temps/Package/Images"; //Image들을 담은 폴더의 위치

        CopyImagesDirectory(imagesTempDirectory, imagesDirectory); //폴더 복사

        AssetDatabase.Refresh(); //에셋 초기화
    }
    #endregion

    /* Images 디렉터리를 복사하는 함수 */
    public static void CopyImagesDirectory(string sourceFolder, string destFolder)
    {
        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);

        string[] files = Directory.GetFiles(sourceFolder);
        string[] folders = Directory.GetDirectories(sourceFolder);

        foreach (string file in files)
        {
            if (Path.GetExtension(file).Equals(".png")) //확장자 PNG
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
        }

        foreach (string folder in folders)
        {
            string name = Path.GetFileName(folder);
            string dest = Path.Combine(destFolder, name);
            CopyImagesDirectory(folder, dest);
        }
    }
}
#endif