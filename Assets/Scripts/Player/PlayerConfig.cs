using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerConfig
    {
        [Header("Movement")]
        public float walkSpeed = 5;
        public float crawlSpeed = 2;
        
        [Header("Jump")]
        [Min(1)]          public int   maxJumpCount = 1;
        [Min(1)]          public float jumpHeight = 1;
        [Min(0.1f)]       public float timeToPeak = 1;
        [Min(0.1f)]       public float timeToFall = 1;
        [Min(0)]          public float maxFallSpeed = 25;
        [Min(1)]          public float releaseJumpSpeedMultiplier = 1;
        [Range(0.01f, 1)] public float coyoteTime = 0.2f;
        [Range(0.01f, 1)] public float jumpBuffer = 0.2f;

        [Header("Ground Detection")]
        public LayerMask groundLayer;
        public Vector2 gCheckBoxSize = Vector2.one;
        [Min(0.1f)] public float gCheckDistance = 1;
    }
}