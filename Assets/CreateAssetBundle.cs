#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/**
 * Script for exporting 3D objects into Asset Bundle
 * @see https://docs.unity3d.com/ScriptReference/BuildPipeline.BuildAssetBundles.html
 */
public class CreateAssetBundle : MonoBehaviour
{
    [MenuItem("Assets/Build Windows64 AssetBundles")]
    static void BuildWindowsAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/Windows64AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
#endif