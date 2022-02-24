using HandPoseGen.Models.Interaction;
using HandPoseGen.ScriptableObjects;
using HandPoseGen.Views.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen.Models.Avatar
{
    public class PosableHandModel : HandPoseGenElement
    {
        [HideInInspector]
        public PosableHandHandler handler;

        [Header("Handlers")]
        public PosePointHandler posePoint;

        [Header("Models")]
        public ProxyHandModel proxyHand;

        [Header("Control")]
        public ScriptableObjects.Configuration configuration;
        public ScriptableObjects.Pose startPose;
        public ScriptableObjects.Pose endPose;
    }
}