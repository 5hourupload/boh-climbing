using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HandPoseGen.Models.Avatar
{
    public class FingerModel : HandPoseGenElement
    {
        [HideInInspector]
        public HandModel hand;

        [Header("Models")]
        public BoneModel[] bones;

        [Header("Refs")]
        public Transform fingerBase;
        public Transform fingerTip;
        public BoneModel distal;

        private void Awake()
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].finger = this;
            }
        }
    }

 
}
