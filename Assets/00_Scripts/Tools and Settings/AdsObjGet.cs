using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System;

public static class AdsObjGet
{

    public static GameObject LoadGameObj(AssetReference reference)
    {
        var obj = Addressables.LoadAssetAsync<GameObject>(reference);
        return obj.WaitForCompletion();
    }

    public static GameObject LoadGameObj(string label)
    {
        var obj = Addressables.LoadAssetAsync<GameObject>(label);
        return obj.WaitForCompletion();
    }

    public static Sprite LoadSprite(string label)
    {
        var obj = Addressables.LoadAssetAsync<Sprite>(label);
        return obj.WaitForCompletion();
    }

    public static void AdsRelease(GameObject obj)
    {
        Addressables.ReleaseInstance(obj);
    }

    public static TextAsset LoadTextAsset(string label)
    {
        AsyncOperationHandle<TextAsset> obj;
        try
        {
            obj = Addressables.LoadAssetAsync<TextAsset>(label);
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.Log(ex);
#endif
            return null;
        }
        return obj.WaitForCompletion();
    }
}
