using HandPoseGen.Helpers;
using HandPoseGen.Models.Interaction;
using HandPoseGen.Views.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HandPoseGen.Views.Handlers
{
    public class PosePointHandler : HandPoseGenElement
    {
        public sealed class PosePointViewModel
        {
            PosePointModel model;
            public Transform transformRef { get { return model.transformRef; } }
            public Rigidbody rigidbodyRef { get { return model.rigidbodyRef; } }
            public PosePointRotationMode rotationMode { get { return model.rotationMode; } }
            public PosePointBehaviour behaviour { get { return model.behaviour; } }
            public float minDistance { get { return model.minDistance; } }
            public float startAtLerp { get { return model.startAtLerp; } }
            public float stopAtLerp { get { return model.stopAtLerp; } }
            public Snapper snapAt { get { return model.snapAt; } }
            public float accuracy { get { return model.boneThickness; } }
            public bool collideWithOtherRbs { get { return model.collideWithOtherRbs; } }
            public bool collideWithTriggers { get { return model.collideWithTriggers; } }

            public PosePointViewModel(PosePointModel model)
            {
                this.model = model;
            }
        }
        public PosePointViewModel viewModel;

        // public UnityEvent onEvent = new UnityEvent();
    }
}
