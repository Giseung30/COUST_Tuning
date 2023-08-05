#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public class PackageManager
{
    #region Build Package
    [MenuItem("Custom/Package/Build Package_PC")] //�޴��ν� ����
    /* ��Ű�� �����ϴ� �Լ� - PC */
    public static void BuildPackageForPC()
    {
        /* Bundles */
        string bundlesDirectory = "Assets/Package_PC/Bundles"; //Bundle���� ������ ������ ��ġ
        if (!Directory.Exists(bundlesDirectory)) //������ �������� ������
        {
            Directory.CreateDirectory(bundlesDirectory); //���� ����
        }
        BuildPipeline.BuildAssetBundles(bundlesDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64); //���� ����

        /* Images */
        string imagesDirectory = "Assets/Package_PC/Images"; //Image���� ������ ������ ��ġ
        string imagesTempDirectory = "Assets/Temps/Package/Images"; //Image���� ���� ������ ��ġ

        CopyImagesDirectory(imagesTempDirectory, imagesDirectory); //���� ����

        AssetDatabase.Refresh(); //���� �ʱ�ȭ
    }

    [MenuItem("Custom/Package/Build Package_Android")] //�޴��ν� ����
    /* ��Ű�� �����ϴ� �Լ� - Android */
    public static void BuildPackageForAndroid()
    {
        /* Bundles */
        string bundlesDirectory = "Assets/Package_Android/Bundles"; //Bundle���� ������ ������ ��ġ
        if (!Directory.Exists(bundlesDirectory)) //������ �������� ������
        {
            Directory.CreateDirectory(bundlesDirectory); //���� ����
        }
        BuildPipeline.BuildAssetBundles(bundlesDirectory, BuildAssetBundleOptions.None, BuildTarget.Android); //���� ����

        /* Images */
        string imagesDirectory = "Assets/Package_Android/Images"; //Image���� ������ ������ ��ġ
        string imagesTempDirectory = "Assets/Temps/Package/Images"; //Image���� ���� ������ ��ġ

        CopyImagesDirectory(imagesTempDirectory, imagesDirectory); //���� ����

        AssetDatabase.Refresh(); //���� �ʱ�ȭ
    }
    #endregion

    /* Images ���͸��� �����ϴ� �Լ� */
    public static void CopyImagesDirectory(string sourceFolder, string destFolder)
    {
        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);

        string[] files = Directory.GetFiles(sourceFolder);
        string[] folders = Directory.GetDirectories(sourceFolder);

        foreach (string file in files)
        {
            if (Path.GetExtension(file).Equals(".png")) //Ȯ���� PNG
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