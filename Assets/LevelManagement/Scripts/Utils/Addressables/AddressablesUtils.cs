using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelManagement.Utils
{
    public static class AddressablesUtils
    {
        public static AddressableAssetEntry GetAddressableAssetEntry(Object o)
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
 
            AddressableAssetEntry entry = null;
            string guid = string.Empty;
            long localID = 0;
            string path;

            try
            {
                bool foundAsset = AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out guid, out localID);
                path = AssetDatabase.GUIDToAssetPath(guid);
 
                if (foundAsset && (path.ToLower().Contains("assets")))
                {
                    if (aaSettings != null)
                    {
                        entry = aaSettings.FindAssetEntry(guid);
                    }
                }
 
                if (entry != null)
                {
                    return entry;
                }
 
                return null;
            }
            catch
            {
                return null;
            }
        }
        
        public static TValue LoadInEditor<TValue>(string address)
            where TValue : Object
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
 
            AddressableAssetEntry foundEntry = settings.groups
                .SelectMany(g => g.entries)
                .FirstOrDefault(e => e.address == address);
            
            return foundEntry != null
                ? AssetDatabase.LoadAssetAtPath<TValue>(foundEntry.AssetPath)
                : null;
        }

        public static Object GetAsset(this AddressableAsset self) => self.GetAsset<Object>();

        public static T GetAsset<T>(this AddressableAsset self) where T : Object
        {
            if (!string.IsNullOrEmpty(self.Address))
            {
                return LoadInEditor<T>(self.Address);
            }

            if (!string.IsNullOrEmpty(self.RealPath))
            {
                return Resources.Load<T>(self.RealPath);
            }
            
            return null;
        }
    }
}