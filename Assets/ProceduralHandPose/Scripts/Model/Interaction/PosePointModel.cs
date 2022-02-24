using HandPoseGen.Helpers;
using HandPoseGen.Views.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen.Models.Interaction
{
    public class PosePointModel : HandPoseGenElement
    {
        [HideInInspector]
        public PosePointHandler handler;

        [Header("Refs")]
        public Transform transformRef;
        public Rigidbody rigidbodyRef;

        [Header("Control")]
        public PosePointRotationMode rotationMode = PosePointRotationMode.None;
        public PosePointBehaviour behaviour = PosePointBehaviour.Default;
        [Range(0.0f, 0.5f)]
        public float minDistance;

        [Range(0.0f, 1.0f)]
        public float startAtLerp = 0.0f;
        [Range(0.0f, 1.0f)]
        public float stopAtLerp = 1.0f;
        [HideInInspector]
        public Snapper snapAt = Snapper.Palm;
        [Range(0.0f, 0.01f)]
        public float boneThickness = 0.008f;
        public bool collideWithOtherRbs = false;
        public bool collideWithTriggers = false;
    }
}
