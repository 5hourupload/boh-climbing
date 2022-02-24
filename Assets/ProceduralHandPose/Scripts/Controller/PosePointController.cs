using HandPoseGen.Models.Interaction;
using HandPoseGen.Views.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen.Controllers.Interaction
{
    public class PosePointController : PosePointHandler
    {
        public PosePointModel model;

        private void Awake()
        {
            model.handler = this;
            viewModel = new PosePointViewModel(model);
        }

        private void Start()
        {
        }
    }
}
