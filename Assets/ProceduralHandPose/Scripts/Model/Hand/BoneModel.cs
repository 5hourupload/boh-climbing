using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HandPoseGen.Models.Avatar
{
    public class BoneModel : HandPoseGenElement
    {
        [HideInInspector]
        public FingerModel finger;

        [Header("Refs")]
        public Transform transformRef;
    }
}