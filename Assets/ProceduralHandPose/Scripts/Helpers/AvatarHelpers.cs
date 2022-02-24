using HandPoseGen.Models.Avatar;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HandPoseGen.Helpers
{
    public static class AvatarHelpers
    {
        public static BoneModel[] GetHandBones(HandModel hand)
        {
            // Same order as OVRSkeleton.Bones
            List<BoneModel> handBones = new List<BoneModel>();

            // Wrist
            handBones.Add(hand.wrist);

            // Forearm
            handBones.Add(hand.forearm);

            // Finger bones
            for (int i = 0; i < hand.fingers.Length; i++)
            {
                handBones.AddRange(hand.fingers[i].bones);
            }

            return handBones.ToArray();
        }

        public static Transform[] GetHandTransforms(HandModel hand)
        {
            List<BoneModel> handBones = new List<BoneModel>(GetHandBones(hand));

            List<Transform> handTransforms = new List<Transform>();

            // Wrist, Forearm and Finger bones
            for (int i = 0; i < handBones.Count; i++)
            {
                handTransforms.Add(handBones[i].transformRef);
            }

            // Figner tips
            for (int i = 0; i < hand.fingers.Length; i++)
            {
                handTransforms.Add(hand.fingers[i].fingerTip);
            }

            return handTransforms.ToArray();
        }

        public static Transform[] GetFingerTransforms(FingerModel finger)
        {
            Transform[] boneTransforms = new Transform[finger.bones.Length];

            for (int i = 0; i < boneTransforms.Length; i++)
            {
                boneTransforms[i] = finger.bones[i].transformRef;
            }

            return boneTransforms;
        }

        public static int GetBonesCount(HandModel hand)
        {
            int n = 2; // wrist + forearm

            for (int i = 0; i < hand.fingers.Length; i++)
            {
                n += hand.fingers[i].bones.Length;
            }

            return n;
        }
    }
}
