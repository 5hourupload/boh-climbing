using HandPoseGen.Helpers;
using HandPoseGen.Models.Avatar;
using HandPoseGen.ScriptableObjects;
using HandPoseGen.Views.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen.Controllers.Avatar
{
    public class PosableHandController : PosableHandHandler
    {
        public PosableHandModel model;

        int initialIteration;
        int lastIteration;

        Quaternion localRot;
        RaycastHit[] hits;
        Ray ray;

        BonePose[] startPoseBones;
        BonePose[] endPoseBones;

        HandModel ghost;
        HandModel master;

        private void Awake()
        {
            model.handler = this;
            viewModel = new ProxyHandViewModel(model);

            onSetSnapPoint.AddListener(SetSanpPoint);
        }

        private void Start()
        {
            master = model.proxyHand.master;
            ghost = model.proxyHand.ghost;         

            StartHand(master);
            StartHand(ghost);

            startPoseBones = model.startPose.GetBones();
            endPoseBones = model.endPose.GetBones();

            // for (int b = 0; b < ghost.bones.Length; b++)
            // {
            //     Debug.Log(startPoseBones[b].name + " / " + endPoseBones[b].name + " / " + ghost.bones[b].transformRef.name + " / " + master.bones[b].transformRef.name);
            // }
        }

        private void Update()
        {
            PoseGeneration.HandPlacement(ghost, master, model.posePoint, model.configuration);

            if (model.posePoint.viewModel.collideWithOtherRbs)
                PoseGeneration.HandPose(ghost, model.startPose, model.endPose, model.posePoint, model.configuration, null);
            else
                PoseGeneration.HandPose(ghost, model.startPose, model.endPose, model.posePoint, model.configuration, model.posePoint.viewModel.rigidbodyRef);

        }

        void StartHand(HandModel hand)
        {
            hand.boneTransforms = AvatarHelpers.GetHandTransforms(hand);
            hand.bones = AvatarHelpers.GetHandBones(hand);
        }

        void SetSanpPoint(PosePointHandler snapPoint)
        {
            model.posePoint = snapPoint;
        }
    }
}
