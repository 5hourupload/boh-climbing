using HandPoseGen.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen.ScriptableObjects
{
    [CreateAssetMenu(menuName = "HandPoser/Configuration", order = 2)]
    public class Configuration : ScriptableObject
    {
        [Header("Control")]
        public float maxDistance = 0.5f;

        [Header("Performance")]
        [Range(10, 100)]
        public int maxIterations = 60;
        public bool onlyFingerTips = false;    
    }
}
