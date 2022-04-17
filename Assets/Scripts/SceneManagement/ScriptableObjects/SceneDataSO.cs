using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "NewScene", menuName = "Scene Data/New Scene")]
public class SceneDataSO : ScriptableObject
{
    public AssetReference sceneReference;
}
