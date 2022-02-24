using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HandPoseGen.Abstracts
{
    public abstract class Model : HandPoseGenElement
    {
        public float value = 0.0f;
    }

    public abstract class Handler : HandPoseGenElement
    {
        public sealed class ViewModel
        {
            Model model;
            public float value { get { return model.value; } }

            public ViewModel(Model model)
            {
                this.model = model;
            }
        }
        public ViewModel viewModel;

        public UnityEvent onEvent;
    }

    public class GestureController : Handler
    {
        public Model model;

        private void Awake()
        {
            viewModel = new ViewModel(model);
        }
    }
}
