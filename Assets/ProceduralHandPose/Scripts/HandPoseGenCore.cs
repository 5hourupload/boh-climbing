using HandPoseGen.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen
{
    public class HandPoseGenElement : MonoBehaviour
    {
        public HandPoseGenCore core { get { return HandPoseGenCore.core; } }
    }

    public class HandPoseGenCore : MonoBehaviour
    {
        public static HandPoseGenCore core;

        public CoreModel model;

        private void Awake()
        {
            if (!core)
                core = this;
        }

        void Start() { }
    }
}
