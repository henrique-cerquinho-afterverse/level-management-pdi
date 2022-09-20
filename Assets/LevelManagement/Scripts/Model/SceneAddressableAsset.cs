using LevelManagement.Utils;
using UnityEngine;

namespace LevelManagement.Model
{
    [CreateAssetMenu(fileName = "SceneAsset", menuName = "Addressables/Assets/Scene Asset", order = 1)]
    public class SceneAddressableAsset : ScriptableObject
    {
        [SerializeField] AddressableAsset _addressableAsset;

        public AddressableAsset AddressableAsset => _addressableAsset;
    }
}