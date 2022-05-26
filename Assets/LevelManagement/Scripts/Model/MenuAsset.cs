using LevelManagement.Utils;
using UnityEngine;

namespace LevelManagement.Model
{
    [CreateAssetMenu(fileName = "MenuAsset", menuName = "Menus/Create Menu Asset", order = 1)]
    public class MenuAsset : ScriptableObject
    {
        [SerializeField] AddressableAsset _addressableAsset;

        public AddressableAsset AddressableAsset => _addressableAsset;
    }
}