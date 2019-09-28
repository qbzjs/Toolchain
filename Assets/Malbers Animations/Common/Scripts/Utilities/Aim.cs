﻿using UnityEngine;
using System.Collections.Generic;
using System;
//using Physics = RotaryHeart.Lib.PhysicsExtension.Physics;
using MalbersAnimations.Scriptables;
using MalbersAnimations.Events;

namespace MalbersAnimations.Utilities
{
    public class Aim : MonoBehaviour, IAim, IAnimatorListener
    {
        #region Vars and Props

        #region Public Variables
        [SerializeField] private BoolReference m_active = new BoolReference(true);
        [SerializeField] private Transform m_aimOrigin;
        [SerializeField] private float m_directionSmooth = 10f;

        [Tooltip("Default Screen Center")]
        [SerializeField] private Vector2Reference m_screenCenter = new Vector2Reference(0.5f, 0.5f);

        [Tooltip("Does the Aiming Logic ignore Colliders??")]
        [SerializeField] private QueryTriggerInteraction m_Triggers = QueryTriggerInteraction.Collide;

        [Tooltip("Layers inlcuded on the Aiming Logic")]
        [SerializeField] private LayerMask m_aimLayer = -1;

        [Space, Tooltip("Forced  a Target on the Aiming Logic, and skip the calculation from the Camera")]
        [SerializeField] private Transform m_ForcedTarget;

        ///// <summary>Radius for the Sphere Casting, if this is set to Zero they I will use a Ray Casting</summary>
        //[Header("RayCast Properties"), Tooltip("Radius for the Sphere Casting, if this is set to Zero they I will use a Ray Casting")]
        //public FloatReference RayRadius = new FloatReference(0.25f);

        /// <summary>Maximun Lenght for the Ray Casting</summary>
        [Header("Ray Settings"), Space, Tooltip("Maximun Lenght for the Ray Casting")]
        public float RayLength = 75f;

        /// <summary>Ray Counts for the Ray casting</summary>
        [Tooltip("Maximum Ray Hits for the Ray casting")]
        public int RayCount = 3;

        [Header("Events")]
        public TransformEvent OnAimRayTarget = new TransformEvent();
        public Vector3Event OnScreenCenter = new Vector3Event();

        public bool debug;
        #endregion

        private int hitcount;
        private Transform _t;

        #region Properties


        /// <summary>Main Camera</summary>
        public Camera MainCamera { get; private set; }

        /// <summary>Sets the Aim Origin Transform </summary>
        public Transform AimOrigin
        {
            get { return m_aimOrigin; }
            set { m_aimOrigin = value; }
        }

        /// <summary>Set a Extra Transform to Ignore it (Used in case of the Mount for the Rider)</summary>
        public Transform IgnoreTransform { get; set; }

        /// <summary>Direction the GameObject is Aiming</summary>
        public Vector3 AimDirection { get; private set; }

        /// <summary>is the Current AimTarget a Target Assist?</summary>
        public bool IsTargetAssist { get; private set; }


        /// <summary>Direct Point the Rider is Aiming</summary>
        public Vector3 AimPoint { get; private set; }


        /// <summary>Default Screen Center</summary>
        public Vector3 ScreenCenter { get; private set; }

        public IAimTarget LastAimTarget;

        /// <summary>Is the Aiming Logic Active?</summary>
        public bool Active
        {
            get { return m_active; }
            set
            {
                if (m_active.Value != value)
                {
                    m_active.Value = value;

                    if (!value)
                    {
                        OnScreenCenter.Invoke(ScreenCenter);
                        OnAimRayTarget.Invoke(null);
                        AimDirection = Vector3.zero;
                        aimHit = new RaycastHit();
                    }
                    else
                    {
                        AimDirection = GetAIMDirection();
                    }
                }
            }
        }

        /// <summary>Limit the Aiming via Angle limit Which means the Aiming is Active but should not be used</summary>
        public bool Limited { get; set; }

        /// <summary>Check if the camera is in the right:true or Left: False side of the Animal </summary>
        public bool CameraSide { get; private set; }

        /// <summary>Check if the Target is in the right:true or Left: False side of the Animal </summary>
        public bool TargetSide { get; private set; }

        /// <summary>Main Camera Transform</summary>
        public Transform MainCameraT { get; private set; }


        /// <summary> Last Raycast stored for calculating the Aim</summary>
        private RaycastHit aimHit;
        /// <summary>RaycastHit Data of the Aim logic</summary>
        public RaycastHit AimHit { get { return aimHit; } }
        private Transform m_aimRayTarget;

        /// <summary>Target Transform Stored from the AimRay</summary>
        public Transform AimRayTargetAssist
        {
            get { return m_aimRayTarget; }
            set {
                if (m_aimRayTarget != value)
                {
                    m_aimRayTarget = value;
                    OnAimRayTarget.Invoke(value);
                }
            }
        }

        /// <summary>Forced Target on the Aiming Logic</summary>
        public Transform ForcedTarget
        {
            get { return m_ForcedTarget; }
            set { m_ForcedTarget = value; }
        }

        /// <summary>Layer to Aim and Hit</summary>
        public LayerMask AimLayer
        {
            get { return m_aimLayer; }
            private set { m_aimLayer = value; }
        }

        public QueryTriggerInteraction TriggerInteraction { get { return m_Triggers; } }

        #endregion
        #endregion

        void Start()
        {
            MainCamera = MalbersTools.FindMainCamera();
            MainCameraT = MainCamera.transform;

            _t = transform;

            if (AimOrigin == null) AimOrigin = transform;

            GetCenterScreen();
        }

        void Update()
        {
            if (Active)
            {
                SetAiming();
            }
            CalculateCameraTargetSide();
        }


        private void CalculateCameraTargetSide()
        {
            float targetSide = 0;
            float cameraSide = 0;

            if (ForcedTarget)
                targetSide = Vector3.Dot((_t.position - ForcedTarget.position).normalized, _t.right);       //Calculate the side from the Target

            if (MainCamera)
                cameraSide = Vector3.Dot(MainCameraT.right, _t.forward);                                    //Get the camera Side Float

            TargetSide = targetSide > 0;                                                                    //Get the Target Side Right/Left
            CameraSide = cameraSide > 0;                                                                    //Get the Camera Side Left/Right
        }

        public virtual void SetAiming()
        {
            if (AimOrigin == null) return;

            var TargetDir = GetAIMDirection();

           AimDirection = (m_directionSmooth > 0) ? Vector3.Lerp(AimDirection, TargetDir, Time.deltaTime * m_directionSmooth) : TargetDir;
        }

        Vector3 GetAIMDirection()
        {
            Vector3 TargetDir;

            if (ForcedTarget)
            {
                TargetDir = MalbersTools.DirectionTarget(AimOrigin, ForcedTarget);
                AimRayTargetAssist = !Limited ? ForcedTarget : null;
                AimPoint = ForcedTarget.position;
            }
            else
            {
                GetCenterScreen();
                DirectionFromCamera(ScreenCenter, out aimHit, IgnoreTransform);
                AimPoint = AimHit.point;

                TargetDir = (AimPoint - AimOrigin.position);
            }

            return TargetDir;
        }

        void GetCenterScreen()
        {
            var SC = new Vector3(Screen.width * m_screenCenter.Value.x, Screen.height * m_screenCenter.Value.y); //Gets the Center of the Aim Dot Transform

            if (SC != ScreenCenter)
            {
                ScreenCenter = SC;
                OnScreenCenter.Invoke(ScreenCenter);
            }
        }

        public void DirectionFromCamera(Vector3 ScreenPoint, out RaycastHit hit,  Transform Ignore = null)
        {
            Ray ray = MainCamera.ScreenPointToRay(ScreenPoint);

            hit = new RaycastHit()
            {
                distance = float.MaxValue,
                point = ray.GetPoint(RayLength)
            };

            var Hits = new RaycastHit[RayCount];

            //if (RayRadius>0)
            //    hitcount = Physics.SphereCastNonAlloc(ray, RayRadius, Hits, RayLength, AimLayer, m_Triggers);
            //else
                hitcount = Physics.RaycastNonAlloc(ray, Hits, RayLength, AimLayer, m_Triggers);


            if (hitcount > 0)
            {
                foreach (RaycastHit rHit in Hits)
                {
                    if (rHit.transform == null) break;                              //Means nothing was found

                    if (rHit.transform.root == Ignore) continue;                               //Dont Hit anything the Ignore
                    if (rHit.transform.root == AimOrigin.root) continue;                       //Dont Hit anything in this hierarchy

                    if (Vector3.Distance(MainCameraT.position, rHit.point) < Vector3.Distance(MainCameraT.position, AimOrigin.position)) continue; //If I hit something behind me skip

                    if (hit.distance > rHit.distance) hit = rHit;
                }
            }


            IsTargetAssist = false;

            IAimTarget IAimTarg = hit.transform != null ? hit.transform.GetComponent<IAimTarget>() : null;

            if (IAimTarg != null)
            {
                if (IAimTarg.AimAssist)
                {
                    hit.point = hit.collider.bounds.center;
                    IsTargetAssist = true;
                    AimRayTargetAssist = hit.collider.transform;
                }


                if (IAimTarg != LastAimTarget)
                {
                    LastAimTarget = IAimTarg;
                    LastAimTarget.AimEnter(true);
                }
            }
            else
            {
                if (LastAimTarget != null)
                {
                    LastAimTarget.AimEnter(false);
                    LastAimTarget = null;
                }

                AimRayTargetAssist = null;
            }
        } 

        /// <summary>This is used to listen the Animator asociated to this gameObject </summary>
        public virtual void OnAnimatorBehaviourMessage(string message, object value)
        {
            this.InvokeWithParams(message, value);
        }

#if UNITY_EDITOR


        void Reset()
        {
            MEvent FollowUITransform = MalbersTools.GetInstance<MEvent>("Follow UI Transform");

            OnAimRayTarget = new TransformEvent();
            OnScreenCenter = new Vector3Event();

            if (FollowUITransform != null)
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(OnAimRayTarget, FollowUITransform.Invoke);
                UnityEditor.Events.UnityEventTools.AddPersistentListener(OnScreenCenter, FollowUITransform.Invoke);
            }
        }

        void OnDrawGizmos()
        {
            if (debug && Application.isPlaying)
            {
                if (Active && !Limited)
                {
                    //float radius = RayRadius > 0.01f ? RayRadius.Value : 0.05f;
                    float radius =  0.05f;
                    Gizmos.color = Color.green ;
                    Gizmos.DrawWireSphere(AimPoint, radius);
                    Gizmos.DrawSphere(AimPoint, radius);
                    Gizmos.DrawRay(AimOrigin.position, AimDirection);

                    Gizmos.color = Color.gray;
                    Gizmos.DrawLine(AimOrigin.position, AimPoint); 
                }
            }
        }
#endif
    }
}