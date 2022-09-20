using LevelManagement.Utils;
using UnityEngine;

namespace LevelManagement.Model
{
    [CreateAssetMenu(fileName = "MenuAsset", menuName = "Addressables/Assets/Menu Asset", order = 1)]
    public class MenuAsset : ScriptableObject
    {
        [SerializeField] AddressableAsset _addressableAsset;

        public AddressableAsset AddressableAsset => _addressableAsset;
    }
}