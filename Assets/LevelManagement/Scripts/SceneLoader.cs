using System;
using LevelManagement.Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LevelManagement
{
    /// <summary>
    /// Implements addressable scene loading
    /// </summary>
    public static class SceneLoader
    {
        static AsyncOperationHandle<SceneInstance> _handle;

        public static void LoadScene(string sceneName, Action<AsyncOperationHandle<SceneInstance>> callback = null)
        {
            var assetGuids = AssetDatabase.FindAssets("t:" + nameof(SceneAddressableAsset));
            foreach (var guid in assetGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAddressableAsset>(path);

                AssetReference sceneReference = (AssetReference) sceneAsset;
                
                if (sceneAsset == null || !sceneAsset.name.Contains(sceneName)) continue;

                Addressables.LoadSceneAsync(sceneAsset, LoadSceneMode.Additive).Completed += (op =>
                {
                    SceneLoadComplete(op);
                    callback?.Invoke(op);
                });
            }
            
        }

        public static void UnloadScene(Action<AsyncOperationHandle<SceneInstance>> callback = null)
        {
            Addressables.UnloadSceneAsync(_handle).Completed += (op =>
            {
                SceneUnloadComplete(op);
                callback?.Invoke(op);
            });
        }

        static void SceneLoadComplete(AsyncOperationHandle<SceneInstance> op)
        {
            _handle = op;
            
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"{op.Result.Scene.name} scene loaded alright!");
            }
            else
            {
                Debug.LogError($"{op.Result.Scene.name} failed to load!");
            }
        }
        
        static void SceneUnloadComplete(AsyncOperationHandle<SceneInstance> op)
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"{op.Result.Scene.name} scene unloaded alright!");
            }
            else
            {
                Debug.LogError($"{op.Result.Scene.name} failed to unload!");
            }
        }
    }
}