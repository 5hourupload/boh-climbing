using HandPoseGen.Controllers.Avatar;
using HandPoseGen.Models.Avatar;
using HandPoseGen.Views.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HandPoseGen.Views.Handlers
{
    public class PosableHandHandler : HandPoseGenElement
    {
        public sealed class ProxyHandViewModel
        {
            PosableHandModel model;

            public PosePointHandler snapPoint { get { return model.posePoint; } }

            public ProxyHandViewModel(PosableHandModel model)
            {
                this.model = model;
            }
        }
        public ProxyHandViewModel viewModel;

        public SnapPointEvent onSetSnapPoint = new SnapPointEvent();
    }
}
