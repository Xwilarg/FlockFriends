﻿using UnityEngine;

namespace TouhouJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        public float Speed;
        public float JumpForce;

        public float JumpReloadTime;
    }
}