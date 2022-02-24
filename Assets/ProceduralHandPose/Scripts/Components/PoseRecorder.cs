using HandPoseGen.Models.Avatar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HandPoseGen.Helpers;
using HandPoseGen.ScriptableObjects;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HandPoseGen.Utils
{
    public class PoseRecorder : MonoBehaviour
    {
        public HandModel hand;
        public ScriptableObjects.Pose pose;
        public string alias;

        public bool applyInverted = false;

        public void Save()
        {
            if (hand.bones.Length == 0)
            {
                hand.fingers = new FingerModel[5] { hand.thumb, hand.index, hand.middle, hand.ring, hand.pinky };

                for (int i = 0; i < hand.fingers.Length; i++)
                {
                    hand.fingers[i].hand = hand;
                }

                hand.bones = AvatarHelpers.GetHandBones(hand);
            }

            List<FingerPose> fingers = new List<FingerPose>();
            for (int f = 0; f < hand.fingers.Length; f++)
            {
                List<BonePose> bones = new List<BonePose>();
                for (int b = 0; b < hand.fingers[f].bones.Length; b++)
                {
                    bones.Add(new BonePose(
                        hand.fingers[f].bones[b].transformRef.localPosition,
                        hand.fingers[f].bones[b].transformRef.localRotation,
                        hand.fingers[f].bones[b].transformRef.localScale,
                        hand.fingers[f].bones[b].transformRef.name));
                }

                FingerPose finger = new FingerPose(hand.fingers[f].name);
                finger.bones = bones.ToArray();

                fingers.Add(finger);
            }

            pose.alias = alias;
            pose.fingers = fingers.ToArray();

            pose.wrist = new BonePose(
                        hand.wrist.transformRef.localPosition,
                        hand.wrist.transformRef.localRotation,
                        hand.wrist.transformRef.localScale,
                        hand.wrist.transformRef.name);

            pose.forearm = new BonePose(
                        hand.forearm.transformRef.localPosition,
                        hand.forearm.transformRef.localRotation,
                        hand.forearm.transformRef.localScale,
                        hand.forearm.transformRef.name);
        }

        public void Apply()
        {
            BonePose[] poseBones = pose.GetBones();

            // Wrist won't be applied
            for (int i = 1; i < hand.bones.Length; i++)
            {
                hand.bones[i].transformRef.localPosition = applyInverted ? poseBones[i].localPosition * -1.0f : poseBones[i].localPosition;
                hand.bones[i].transformRef.localRotation = poseBones[i].localRotation;
                hand.bones[i].transformRef.localScale = poseBones[i].localScale;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PoseRecorder))]
    public class HandPoserEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PoseRecorder myScript = (PoseRecorder)target;

            if (GUILayout.Button("OVERWRITE"))
            {
                myScript.Save();
            }
            if (GUILayout.Button("APPLY"))
            {
                myScript.Apply();
            }
        }
    }
#endif
}
