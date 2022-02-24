using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandPoseGen.Utils
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class LRMapper : MonoBehaviour
    {
        LineRenderer lr;

        public List<Transform> points = new List<Transform>();

        public bool isActive = false;

        void Start()
        {
            lr = GetComponent<LineRenderer>();

        }

        void Update()
        {
            if (!isActive)
                return;

            lr.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
    }
}
