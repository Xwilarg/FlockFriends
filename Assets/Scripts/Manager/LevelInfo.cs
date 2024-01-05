﻿using TouhouJam.Player;
using UnityEngine;

namespace TouhouJam.Manager
{
    [System.Serializable]
    public class LevelInfo
    {
        public TextAsset IntroStory;
        public TextAsset OutroStory;
        public EBird UnlockedBird;
    }
}