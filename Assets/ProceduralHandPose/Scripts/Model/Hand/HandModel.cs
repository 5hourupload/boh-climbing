using HandPoseGen.Models.Interaction;
using HandPoseGen.Views.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen.Models.Avatar
{
    public class HandModel : HandPoseGenElement
    {
        [HideInInspector]
        public ProxyHandModel proxyHand;

        [Header("Models")]
        public FingerModel thumb;
        public FingerModel index;
        public FingerModel middle;
        public FingerModel ring;
        public FingerModel pinky;

        [HideInInspector]
        public FingerModel[] fingers;

        public BoneModel wrist;
        public BoneModel forearm;

        [Header("Refs")]
        public Transform pinchCenter;
        public Transform throatCenter;
        public Transform palmCenter;
        public Transform palmNormal;
        public Transform palmExterior;
        public Transform palmInterior;

        public SkinnedMeshRenderer skinnedMR;

        [HideInInspector]
        public Transform[] boneTransforms;

        [HideInInspector]
        public BoneModel[] bones;

        protected void Awake()
        {
            fingers = new FingerModel[5] { thumb, index, middle, ring, pinky };

            for (int i = 0; i < fingers.Length; i++)
            {
                fingers[i].hand = this;
            }
        }
    }
}
