using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandPoseGen.Models.Avatar;
using HandPoseGen.ScriptableObjects;
using HandPoseGen.Views.Handlers;

namespace HandPoseGen.Helpers
{
    public enum Snapper
    {
        Palm,
        Pinch,
        Throat
    }

    public enum PosePointBehaviour
    {
        Default,
        ClosestPoint,
        AlwaysMatch
    }

    public enum PosePointRotationMode
    {
        None,
        MatchXY,
        MatchNormal
    }

    public static class PoseGeneration
    {
        public static void HandPose(HandModel ghostHand, ScriptableObjects.Pose startPose, ScriptableObjects.Pose endPose, PosePointHandler posePoint, Configuration conf, Rigidbody rb)
        {
            int initialIteration = (int)(conf.maxIterations * posePoint.viewModel.startAtLerp);
            int lastIteration = (int)(conf.maxIterations * posePoint.viewModel.stopAtLerp);

            for (int f = 0; f < ghostHand.fingers.Length; f++)
            {
                FingerPose(ghostHand.fingers[f], startPose.fingers[f], endPose.fingers[f], initialIteration, lastIteration, posePoint, conf, rb);
            }
        }

        public static void FingerPose(FingerModel finger, FingerPose startFingerPose, FingerPose endFingerPose, int initialIteration, int lastIteration, PosePointHandler posePoint, Configuration conf, Rigidbody rb)
        {
            float[] lerpMap = new float[finger.bones.Length];

            for (int m = 0; m < lerpMap.Length; m++)
            {
                lerpMap[m] = 1.0f;
            }

            float lerp;

            Vector3 boneBase;
            Vector3 boneTip;
            Vector3 boneVector;

            bool updateMap;
            for (int i = initialIteration; i <= lastIteration; i++)
            {
                lerp = (float)i / (float)conf.maxIterations;

                for (int b = 0; b < finger.bones.Length; b++)
                {
                    if (lerp > lerpMap[b])
                        continue;

                    // Rotate
                    Quaternion localRot = Quaternion.Lerp(startFingerPose.bones[b].localRotation, endFingerPose.bones[b].localRotation, lerp);
                    finger.bones[b].transformRef.localRotation = localRot;

                    if (!conf.onlyFingerTips || (conf.onlyFingerTips && b == finger.bones.Length - 1))
                    {
                        // Search for collisions
                        boneBase = finger.bones[b].transformRef.position;
                        if (b == finger.bones.Length - 1)
                            boneTip = finger.fingerTip.position;
                        else
                            boneTip = finger.bones[b + 1].transformRef.position;

                        boneVector = boneTip - boneBase;

                        Ray ray = new Ray(boneBase, boneVector.normalized);

                        RaycastHit[] hits;
                        if (posePoint.viewModel.accuracy > 0)
                            hits = Physics.SphereCastAll(ray, posePoint.viewModel.accuracy, boneVector.magnitude);
                        else
                            hits = Physics.RaycastAll(ray, boneVector.magnitude);

                        updateMap = false;
                        for (int h = 0; h < hits.Length; h++)
                        {
                            // If we didn't touch anything
                            if (hits[h].rigidbody == null)
                                continue;
                            // If we touch a trigger and we can ignore triggers
                            if (hits[h].collider.isTrigger && !posePoint.viewModel.collideWithTriggers)
                                continue;

                            // If we touch the desired rigidbody
                            if (hits[h].rigidbody == rb)
                            {
                                updateMap = true;
                            }
                            // If we touched something and we can collide with any rigidbody (except ourselves)
                            else if (rb == null && hits[h].rigidbody && !hits[h].rigidbody.transform.IsChildOf(finger.hand.wrist.transformRef))
                            {
                                updateMap = true;
                            }
                        }

                        // If there are, update lerp map
                        if (updateMap)
                        {
                            for (int m = 0; m <= b; m++)
                            {
                                lerpMap[m] = lerp;
                            }
                        }
                    }
                }

                // If any bone has a lerp lower than its limit then we can keep iterating
                bool exit = true;
                for (int b = 0; b < finger.bones.Length; b++)
                {
                    if (lerp < lerpMap[b])
                        exit = false;
                }

                if (exit)
                    break;
            }

        }

        public static void HandPlacement(HandModel ghostHand, HandModel masterHand, PosePointHandler posePoint, Configuration conf)
        {
            ghostHand.boneTransforms = AvatarHelpers.GetHandTransforms(ghostHand);
            ghostHand.bones = AvatarHelpers.GetHandBones(ghostHand);

            BoneModel[] distantBones = masterHand.bones;

            // Copy values from master hand
            for (int i = 0; i < distantBones.Length; i++)
            {
                ghostHand.bones[i].transformRef.position = distantBones[i].transformRef.position;
                ghostHand.bones[i].transformRef.rotation = distantBones[i].transformRef.rotation;
            }

            Transform palmCenter = ghostHand.palmNormal;
            Vector3 palmInterior = ghostHand.palmCenter.position + masterHand.palmCenter.up * 0.05f;

            /*
            Transform throatCenter = ghostHand.throatCenter;
            Vector3 throatInterior = ghostHand.index.fingerBase.position;

            Transform pinchCenter = ghostHand.pinchCenter;
            Vector3 pinchInterior = ghostHand.pinchCenter.up * 0.05f;
            */

            if (posePoint.viewModel == null)
                return;

            Rigidbody desiredRb = posePoint.viewModel.rigidbodyRef;

            if (masterHand.palmCenter.position != palmCenter.position)
            {
                Debug.LogError("Palm center and palm normal only can differ on rotation!");
                return;
            }

            Transform snapCenter;
            Vector3 snapUp;

            switch (posePoint.viewModel.snapAt)
            {
                case Snapper.Palm:
                    snapCenter = palmCenter;
                    snapUp = palmInterior;
                    break;
                /*
                case Snapper.Throat:
                    snapCenter = throatCenter;
                    snapUp = throatInterior;
                    break;
                case Snapper.Pinch:
                    snapCenter = pinchCenter;
                    snapUp = pinchInterior;
                    break;
                */
                default:
                    snapCenter = palmCenter;
                    snapUp = palmInterior;
                    break;
            }

            // Get hit to snap hand to (including point and collider)
            Ray normalToSnap = new Ray(snapCenter.position, posePoint.viewModel.transformRef.position - snapCenter.position);
            RaycastHit normalHit = ClosestHitFromPoint(normalToSnap, conf.maxDistance, desiredRb, posePoint.viewModel.collideWithOtherRbs, posePoint.viewModel.collideWithTriggers);

            Vector3 centerDestination;
            switch (posePoint.viewModel.behaviour)
            {
                case PosePointBehaviour.Default:

                    if (normalHit.rigidbody == null)
                        centerDestination = posePoint.viewModel.transformRef.position;
                    else
                        centerDestination = normalHit.point;

                    break;

                case PosePointBehaviour.ClosestPoint:

                    if (normalHit.collider != null)
                    {
                        // Get closest point to that collider
                        centerDestination = normalHit.collider.ClosestPoint(snapCenter.position);

                        // Raycast to that point and sotre hit (including normal)
                        normalToSnap = new Ray(snapCenter.position, centerDestination - snapCenter.position);
                        normalHit.collider.Raycast(normalToSnap, out normalHit, conf.maxDistance);
                    }
                    else
                        centerDestination = posePoint.viewModel.transformRef.position;

                    break;

                case PosePointBehaviour.AlwaysMatch:

                    centerDestination = posePoint.viewModel.transformRef.position;

                    break;

                default:

                    centerDestination = posePoint.viewModel.transformRef.position;

                    break;
            }

            // Rotate hand so its normal will be the opposite to hit normal
            Vector3 forwardDir = normalHit.normal * -1.0f;

            Quaternion desiredRot;
            Quaternion relRot;

            switch (posePoint.viewModel.rotationMode)
            {
                case PosePointRotationMode.MatchXY:

                    desiredRot = posePoint.viewModel.transformRef.rotation;
                    relRot = Quaternion.Inverse(ghostHand.wrist.transformRef.rotation) * ghostHand.palmCenter.rotation;
                    ghostHand.wrist.transformRef.rotation = desiredRot * Quaternion.Inverse(relRot);

                    break;

                case PosePointRotationMode.MatchNormal:

                    desiredRot = Quaternion.LookRotation(posePoint.viewModel.transformRef.forward, ghostHand.palmCenter.up);
                    relRot = Quaternion.Inverse(ghostHand.wrist.transformRef.rotation) * ghostHand.palmNormal.rotation;
                    ghostHand.wrist.transformRef.rotation = desiredRot * Quaternion.Inverse(relRot);

                    break;

                default:

                    Vector3 rayDir = normalToSnap.direction;
                    RaycastHit upHit = ClosestHitFromLine(rayDir, conf.maxDistance, snapCenter.position, snapUp, 4, desiredRb, posePoint.viewModel.collideWithOtherRbs, posePoint.viewModel.collideWithTriggers);

                    Vector3 interiorDestination;
                    if (upHit.rigidbody != null)
                        interiorDestination = upHit.point;
                    else if (normalHit.collider != null)
                        interiorDestination = normalHit.collider.ClosestPoint(snapUp);
                    else
                        interiorDestination = posePoint.viewModel.transformRef.up;

                    Vector3 upDir = (interiorDestination - centerDestination).normalized;

                    desiredRot = Quaternion.LookRotation(forwardDir, upDir);
                    relRot = Quaternion.Inverse(ghostHand.wrist.transformRef.rotation) * ghostHand.palmNormal.rotation;
                    ghostHand.wrist.transformRef.rotation = desiredRot * Quaternion.Inverse(relRot);

                    break;
            }

            // Move hand to match hit point
            Vector3 posDiff = centerDestination - snapCenter.position;

            ghostHand.wrist.transformRef.position += posDiff;

            posDiff = snapCenter.forward * posePoint.viewModel.minDistance;

            ghostHand.wrist.transformRef.position -= posDiff;
        }

        public static RaycastHit ClosestHitFromPoint(Ray ray, float rayLength, Rigidbody rb, bool collideWithOtherRbs, bool collideWithTriggers)
        {
            List<RaycastHit> hits = new List<RaycastHit>(Physics.RaycastAll(ray, rayLength));

            if (rb != null && !collideWithOtherRbs)
                hits = hits.FindAll(x => x.rigidbody == rb);

            if (!collideWithTriggers)
                hits = hits.FindAll(x => x.collider.isTrigger == false);

            RaycastHit candidate = new RaycastHit();
            float minDistance = Mathf.Infinity;
            float distance;
            for (int i = 0; i < hits.Count; i++)
            {
                distance = Vector3.Distance(ray.origin, hits[i].point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    candidate = hits[i];
                }
            }

            return candidate;
        }

        public static RaycastHit ClosestHitFromLine(Vector3 rayDir, float rayLength, Vector3 lineStartWorldPos, Vector3 lineEndWorldPos, int resolution, Rigidbody rb, bool ignoreOtherRbs, bool ignoreTriggers)
        {
            Vector3 slice = (lineEndWorldPos - lineStartWorldPos) / resolution;

            RaycastHit closestHit = new RaycastHit();
            RaycastHit tempHit;
            float minDistance = Mathf.Infinity;
            float tempDistance;

            Vector3 rayOrigin;
            Ray ray;
            for (int i = 1; i < resolution; i++)
            {
                rayOrigin = lineStartWorldPos + slice * i;
                ray = new Ray(rayOrigin, rayDir);
                tempHit = ClosestHitFromPoint(ray, rayLength, rb, ignoreOtherRbs, ignoreTriggers);

                tempDistance = Vector3.Distance(rayOrigin, tempHit.point);

                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    closestHit = tempHit;
                }
            }

            return closestHit;

        }
    }
}
