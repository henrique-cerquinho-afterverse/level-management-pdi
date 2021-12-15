using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace LevelManagement.Data
{
    public class JsonSaver
    {
        static readonly string _filename = "savedata1.sav";

        public static string GetSaveFilename()
        {
            return Application.persistentDataPath + "/" + _filename;
        }

        public void Save(SaveData data)
        {
            data.HashValue = String.Empty;
            
            string json = JsonUtility.ToJson(data);

            data.HashValue = GetSHA256(json);
            json = JsonUtility.ToJson(data);
            
            string saveFilename = GetSaveFilename();

            FileStream filestream = new FileStream(saveFilename, FileMode.Create);

            using (StreamWriter writer = new StreamWriter(filestream))
            {
                writer.Write(json);
            }
        }

        public bool Load(SaveData data)
        {
            string loadFilename = GetSaveFilename();
            if (File.Exists(loadFilename))
            {
                using (StreamReader reader = new StreamReader(loadFilename))
                {
                    string json = reader.ReadToEnd();
                    
                    // check hash before reading
                    if (CheckData(json))
                    {
                        Debug.Log("hashes are equal!!");
                    }
                    else
                    {
                        Debug.LogWarning("JSONSAVER Load: You've been hacked, bro");
                    }
                    
                    JsonUtility.FromJsonOverwrite(json, data);
                }

                return true;
            }
            return false;
        }

        bool CheckData(string json)
        {
            SaveData tempSaveData = new SaveData();
            JsonUtility.FromJsonOverwrite(json, tempSaveData);

            string oldHash = tempSaveData.HashValue;
            tempSaveData.HashValue = string.Empty;

            string tempJson = JsonUtility.ToJson(tempSaveData);
            string newHash = GetSHA256(tempJson);

            return oldHash == newHash;
        }
        public void Delete()
        {
            File.Delete(GetSaveFilename());
        }

        public string GetHexStringFromHash(byte[] hash)
        {
            string hexString = String.Empty;

            foreach (byte b in hash)
            {
                // x2 = hexadecimal, and 2 digits for every char in the string
                hexString += b.ToString("x2");
            }
            return hexString;
        }
        string GetSHA256(string text)
        {
            byte[] textToBytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed SHA256 = new SHA256Managed();

            byte[] hashValue = SHA256.ComputeHash(textToBytes);

            return GetHexStringFromHash(hashValue);
        }
    }
}
