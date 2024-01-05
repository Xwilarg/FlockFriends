﻿using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine;

namespace TouhouJam.Persistency
{
    public class PersistencyManager
    {
        private readonly string _key = "ワンつーすりぃ不思議なステップで踊りだすbeat今宵輝くネオンの下で光るわたしかわいいKawaii召使いたちあの子はどこに？うだうだごろごろだらだらにゃんにゃん何もしたくない！";

        private string Encrypt(string s)
        {
            StringBuilder str = new();
            for (var i = 0; i < s.Length; i++)
            {
                str.Append((char)(s[i] ^ _key[i % _key.Length]));
            }
            return str.ToString();
        }

        private static PersistencyManager _instance;
        public static PersistencyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log($"[PER] Persistency Manager created, data will be saved at {Application.persistentDataPath}/save.bin");
                    _instance = new();
                }
                return _instance;
            }
        }

        private SaveData _saveData;
        public SaveData SaveData
        {
            get
            {
                if (_saveData == null)
                {
                    if (File.Exists($"{Application.persistentDataPath}/save.bin"))
                    {
                        _saveData = JsonConvert.DeserializeObject<SaveData>(Encrypt(File.ReadAllText($"{Application.persistentDataPath}/save.bin")));
                    }
                    else
                    {
                        _saveData = new();
                    }
                }
                return _saveData;
            }
        }

        public void Save()
        {
            File.WriteAllText($"{Application.persistentDataPath}/save.bin", Encrypt(JsonConvert.SerializeObject(_saveData)));
        }

        public void DeleteSaveFolder()
        {
            _saveData = new();
            File.Delete($"{Application.persistentDataPath}/save.bin");
        }
    }
}