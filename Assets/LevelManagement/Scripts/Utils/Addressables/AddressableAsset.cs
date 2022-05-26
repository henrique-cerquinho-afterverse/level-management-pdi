using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace LevelManagement.Utils
{
    [Serializable]
    public class AddressableAsset
    {
        [SerializeField] string _address;
        [SerializeField] string _realPath;

        [SerializeField] bool _useFallback;

        [ShowIf("_useFallback"), SerializeField] Object _fallback; 
            
        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string RealPath
        {
            get
            {
                if (string.IsNullOrEmpty(_realPath)) _realPath = _address.GetRealAssetPath();
                return _realPath;
            }
            set => _realPath = value;
        }

        [SerializeField] string _guid;
        public string Guid
        {
            get => _guid;
            set => _guid = value;
        }
        
        public bool IsDefined => !string.IsNullOrEmpty(_address) || !string.IsNullOrEmpty(_realPath);

        public T GetFallback<T>() where T : Object
        {
            return (_useFallback ? _fallback : null) as T;
        }
        
        public T Get<T>() where T : Object
        {
            return Resources.Load<T>(RealPath);
        }

        public GameObject Instantiate()
        {
            var prefab = Get<GameObject>();
            return Object.Instantiate(prefab);
        }
        
        public GameObject Instantiate(Transform t)
        {
            var prefab = Get<GameObject>();
            return Object.Instantiate(prefab, t);
        }
        
        public GameObject Instantiate(Transform t, bool instantiateInWorldSpace)
        {
            var prefab = Get<GameObject>();
            return Object.Instantiate(prefab, t, instantiateInWorldSpace);
        }

        public AsyncOperationHandle<T> GetAsync<T>() where T : Object
        {
            return Addressables.LoadAssetAsync<T>(_address);
        }

        public AsyncOperationHandle<GameObject> InstantiateAsync(Transform parent = null, bool instantiateInWorldPos = false, bool trackHandle = true)
        {
            if (string.IsNullOrEmpty(_address) && string.IsNullOrEmpty(_realPath))
            {
                throw new InvalidOperationException();
            }
            return Addressables.InstantiateAsync(!string.IsNullOrEmpty(_address) ? _address : _realPath, parent, instantiateInWorldPos, trackHandle);
        }
    }
}