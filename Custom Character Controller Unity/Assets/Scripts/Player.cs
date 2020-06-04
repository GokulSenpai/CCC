using System;
using UnityEditor.Animations;
using UnityEngine;

// This script is attached to the Player
namespace Com.CompanyName.GameName
{
    public class Player : MonoBehaviour
    {
        public bool run;
        public bool jump;
        public bool zoom;
        public bool peek;
        public bool crouch;

        public StateMachine stateMachine;
        
        public WalkState walkState;
        public RunState runState;
        
        // Create all state references here
        
        
        [Space]
        [Header("Speed Data")]
        public float walkSpeed = 1f;
        public float runSpeed = 2f;

        
        [Space]
        [Header("Ground Data")]
        public float groundDistance = 0.4f;
        public Transform groundCheck;
        public LayerMask groundMask;
        
        // HeadBob ref
        [Space]
        [Header("HeadBob Data")]
        public float idleXIntensity = 0.03f;
        public float idleYIntensity = 0.03f;
        public float walkXIntensity = 0.07f;
        public float walkYIntensity = 0.14f;
        
        public float idleFactorA = 1f;
        public float idleFactorB = 5f;
        public float walkFactorA = 5f;
        public float walkFactorB = 7f;
        public float runFactorA = 7f;
        
        //Run FOV
        [Space]
        [Header("Run FOV Data")]
        public float runFovMultiplier = 1.28f;
        public float runFovFactor = 7f;
        
        [Space]
        [Header("References")]
        public Camera theCamera;
        public Animator playerAnimations;

        private Vector3 _cameraInitialPos;
        
        [HideInInspector] public Vector3 targetBobPosition;

        [HideInInspector] public CharacterController controller;
        
        public static readonly int WalkAnim = Animator.StringToHash("iWalk");
        public static readonly int RunAnim = Animator.StringToHash("iRun");
        public static readonly int BlendX = Animator.StringToHash("BlendX");
        public static readonly int BlendY = Animator.StringToHash("BlendY");
        
        private void Start()
        {
            controller = this.gameObject.GetComponent<CharacterController>();
            ////////////////////////////////////////////DOUBT///////////////////////////////////////
            _cameraInitialPos = theCamera.transform.localPosition;

            stateMachine = new StateMachine();
            walkState = new WalkState(this, stateMachine);
            runState = new RunState(this, stateMachine);
            //more states here

            stateMachine.Initialize(walkState);
        }

        private void Update()
        {
            stateMachine.CurrentState.HandleInput();
            stateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsUpdate();
        }

        public Vector3 HeadBob(float graphValue, float xIntensity, float yIntensity)
        {
            targetBobPosition = _cameraInitialPos + new Vector3(Mathf.Cos(graphValue) * xIntensity, Mathf.Sin(graphValue * 2) * yIntensity, 0f);
            return targetBobPosition;
        }
    }
}