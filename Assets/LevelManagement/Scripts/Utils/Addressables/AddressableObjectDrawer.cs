using System.Collections.Generic;
using System.IO;
using System.Linq;
using LevelManagement.Utils;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace LevelManagement.Utils
{
    [CustomPropertyDrawer(typeof(AddressableAsset))]
    public class AddressableObjectDrawer : PropertyDrawer
    {
	    const int _textHeight = 18;
		const int _imageSize = 128;
		private const string _resourcesFolderPath = "/Resources/";
		GUIStyle _style = new GUIStyle();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			SerializedProperty address = property.FindPropertyRelative("_address");
			SerializedProperty path = property.FindPropertyRelative("_realPath");
			SerializedProperty guid = property.FindPropertyRelative("_guid");
			SerializedProperty useFallback = property.FindPropertyRelative("_useFallback");
			SerializedProperty fallback = property.FindPropertyRelative("_fallback");

			Object resourcesObject = null;

			Object obj = AddressablesUtils.LoadInEditor<Object>(address.stringValue);
			
			DrawLabel(position, label.text, 0);

			if (obj == null)
			{
				resourcesObject = Resources.Load(path.stringValue);
				obj = resourcesObject;
			}
			
			Rect foldoutRect = new Rect(position.x, position.y, 25, EditorGUIUtility.singleLineHeight);

			property.isExpanded = EditorGUI.Foldout (foldoutRect, property.isExpanded, GUIContent.none);

			if (property.isExpanded)
			{
				if (resourcesObject != null)
				{
					DrawLabel(position, "From Resources", 1);

					var assetPath = AssetDatabase.GetAssetPath(resourcesObject);
					
					if (string.IsNullOrEmpty(assetPath) == false)
					{
						if (assetPath.Contains(_resourcesFolderPath))
						{
							int index = assetPath.IndexOf(_resourcesFolderPath) + _resourcesFolderPath.Length;
							assetPath = assetPath.Substring(index, assetPath.Length - index);
						}
						path.stringValue = assetPath.Replace(Path.GetExtension(assetPath), string.Empty);
					}
				}
				
				obj = DrawObjectField(position, GUIContent.none, obj, 2);

				string resourcesPath = null;
				if (IsFromResources(obj, out resourcesPath))
				{
					path.stringValue = resourcesPath;
				}
				
				path.stringValue = DrawTextField(position, "RealPath", path.stringValue, 3);

				if (obj != null)
				{
					AddressableAssetEntry addrObj = AddressablesUtils.GetAddressableAssetEntry(obj);
					if (addrObj != null)
					{
						address.stringValue = addrObj.address;
						guid.stringValue = addrObj.guid;
					}
					
					long objSize = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(obj)/2;
					CreateAssetPreview(position, obj.GetType(), objSize, obj);
				}

				address.stringValue = DrawTextField(position, "Address", address.stringValue, 4);

				useFallback.boolValue = DrawBoolField(position, "Use Fallback", useFallback.boolValue, 9);

				if (useFallback.boolValue)
				{
					fallback.objectReferenceValue = DrawObjectField(position, GUIContent.none, fallback.objectReferenceValue, 10);
				}
				else
				{
					fallback.objectReferenceValue = null;
				}
			}

			EditorGUI.EndProperty();
		}

		private bool IsFromResources(Object obj, out string resourcesPath)
		{
			var path = AssetDatabase.GetAssetPath(obj);

			if (!path.Contains("Resources"))
			{
				resourcesPath = "";
				return false;
			}
			
			resourcesPath = path.GetRealAssetPath();
			return !string.IsNullOrEmpty(resourcesPath);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!property.isExpanded)
			{
				return _textHeight;
			}
			
			SerializedProperty address = property.FindPropertyRelative("_address");
			SerializedProperty path = property.FindPropertyRelative("_realPath");
			
			Object obj = AddressablesUtils.LoadInEditor<Object>(address.stringValue);

			if (obj == null)
			{
				obj = Resources.Load(path.stringValue);
			}
			
			if (obj == null)
			{
				return 8 * _textHeight;
			}

			return _imageSize + 8 * _textHeight;
		}
		
		void CreateAssetPreview(Rect position, System.Type type, long size, Object obj)
		{
			DrawLabel(position, "Type: "+type.ToString(), 5);
			DrawLabel(position, "Size: "+FormatMemory(size), 6);

			Texture2D myTexture = null;
			
			if (myTexture == null)
			{
				myTexture = AssetPreview.GetAssetPreview(obj);
			}

			if(myTexture != null)
			{
				DrawTexture(position, myTexture, 8);
			}
		}
		
		string FormatMemory(long bytes)
		{
			if (bytes < 1024)
				return bytes + "B";
			if (bytes < 1024 * 1024)
				return (bytes / 1024) + "KB";
			return (bytes / (1024 * 1024)) + "MB";
		}
		
		void DrawLabel(Rect position, string text, int line = 0) 
		{
			position.y = position.y + _textHeight * line;
			position.height = _textHeight;
			EditorGUI.LabelField(position, text);
		}
		
		void DrawTexture(Rect position, Texture2D texture, int line = 0) 
		{
			position.x = position.xMax - _imageSize;
			position.y = position.y + _textHeight * (line -2);
			position.width = _imageSize;
			position.height = _imageSize;

			if(texture != null)
			{
				GUI.Label(position, texture);
			}
		}

		string DrawTextField(Rect position, string label, string text, int line = 0)
		{
			position.y = position.y + _textHeight * line;
			position.height = _textHeight;
			text = EditorGUI.TextField(position, label, text);
			return text;
		}
		
		bool DrawBoolField(Rect position, string label, bool value, int line)
		{
			position.y = position.y + _textHeight * line;
			position.height = _textHeight;
			value = EditorGUI.Toggle(position, label, value);
			return value;
		}
		
		Object DrawObjectField(Rect position, GUIContent label, Object obj, int line = 0)
		{
			position.y = position.y + _textHeight * line;
			position.height = _textHeight;
			obj = EditorGUI.ObjectField(position, label, obj, typeof(Object), false);
			return obj;
		}
    }
}