using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen.ScriptableObjects
{
    [Serializable]
    public class BonePose
    {
        public string name;
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;

        public BonePose(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, string name)
        {
            this.localPosition = localPosition;
            this.localRotation = localRotation;
            this.localScale = localScale;
            this.name = name;
        }
    }

    [Serializable]
    public class FingerPose
    {
        public string name;
        public BonePose[] bones;

        public FingerPose(string name)
        {
            this.name = name;
            this.bones = new BonePose[0];
        }
    }

    [CreateAssetMenu(menuName = "HandPoser/Hand Pose", order = 2)]
    public class Pose : ScriptableObject
    {
        public string alias;
        public FingerPose[] fingers;
        public BonePose wrist;
        public BonePose forearm;

        public BonePose[] GetBones()
        {
            List<BonePose> bones = new List<BonePose>();

            bones.Add(wrist);
            bones.Add(forearm);

            for (int i = 0; i < fingers.Length; i++)
            {
                bones.AddRange(fingers[i].bones);
            }

            return bones.ToArray();
        }
    }
}
