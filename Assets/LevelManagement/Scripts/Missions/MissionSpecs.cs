using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement.Missions
{
    [Serializable]
    public class MissionSpecs
    {
        [SerializeField] protected string _name;
        [SerializeField] [Multiline] protected string _description;
        [SerializeField] protected string _sceneName;
        [SerializeField] protected string _id;
        [SerializeField] protected Sprite _image;
        
        public string Name
        {
            get { return _name; }
        }
        public string Description
        {
            get { return _description; }
        }
        public string SceneName
        {
            get { return _sceneName; }
        }
        public string Id
        {
            get { return _id; }
        }
        public Sprite Image
        {
            get { return _image; }
        }
    }
}
