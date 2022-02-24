using HandPoseGen.Views.Handlers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HandPoseGen.Views.Events
{
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [Serializable]
    public class RigidbodyEvent : UnityEvent<Rigidbody> { }

    [Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }

    [Serializable]
    public class SnapPointEvent : UnityEvent<PosePointHandler> { }
}