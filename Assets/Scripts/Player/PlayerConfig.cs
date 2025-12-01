using System;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerConfig
    {
        [Header("General")]
        public LayerMask groundLayer;
        
        [Header("Movement")]
        public float walkSpeed = 5;
        public float runSpeed = 10;
        public float crawlSpeed = 2;
        public Vector2 crawlHitboxSize = Vector2.one;
        public Vector2 crawlHitboxOffset = Vector2.zero;
        [Min(0)] public float timeToRun = 3;
        [Min(0)] public float timeToStop = 1;
        
        [Header("Jump")]
        [Min(1)]          public int   maxJumpCount = 1;
        [Min(1)]          public float jumpHeight = 1;
        [Min(0.1f)]       public float timeToPeak = 1;
        [Min(0.1f)]       public float timeToFall = 1;
        [Min(0)]          public float maxFallSpeed = 25;
        [Min(1)]          public float releaseJumpSpeedMultiplier = 1;
        [Range(0.01f, 1)] public float coyoteTime = 0.2f;
        [Range(0.01f, 1)] public float jumpBuffer = 0.2f;

        [Header("Ledge Grab")]
        public Vector2 ledgeCheckBoxPosition = Vector2.one;
        public Vector2 ledgeCheckBoxSize = Vector2.one;
        public Vector2 ledgeRayVerticalOrigin = Vector2.zero;
        public float ledgeRayVerticalDistance = 1f; 
        public float ledgeRayHorizontalDistance = 1f;
        public Vector2 ledgeHoldPosition = Vector2.zero;

        [Header("Ground Detection")]
        public Vector2 gCheckBoxSize = Vector2.one;
        [Min(0.1f)] public float gCheckDistance = 1;
        
        [Header("Wall Detection")]
        public Vector2 wCheckBoxSize = Vector2.one;
        [Min(0.1f)] public float wCheckDistance = 1;
        
        [Header("Ceiling Detection")]
        public Vector2 ceCheckBoxSize = Vector2.one;
        [Min(0.1f)] public float ceCheckDistance = 1;
    }
}