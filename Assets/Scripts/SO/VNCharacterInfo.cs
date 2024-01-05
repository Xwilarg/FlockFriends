﻿using UnityEngine;

namespace TouhouJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/VNCharacterInfo", fileName = "VNCharacterInfo")]
    public class VNCharacterInfo : ScriptableObject
    {
        public string Name;
        public string DisplayName;
        public Sprite Sprite;
    }
}