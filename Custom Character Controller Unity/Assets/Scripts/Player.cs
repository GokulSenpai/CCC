using System;
using System.Runtime.CompilerServices;
using Com.CompanyName.GameName;
using State_Machine;
using State_Machine.States;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// This script is attached to the Player
public class Player : MonoBehaviour
{
    public bool run;
    public bool jump;
    public bool zoom;
    public bool peek;
    public bool crouch;
    public bool cameraTilt;

    public Text stateText;

    public StateMachine stateMachine;

    public IdleState idleState;
    public WalkState walkState;
    public RunState runState;
    public JumpState jumpState;
    public CrouchState crouchState;
    public CrouchWalkState crouchWalkState;
    public PeekState peekState;
    public ZoomState zoomState;
    public LookBackState lookBackState;
    // Create all state references here


    [Space] [Header("Speed Data")] public float walkSpeed = 1f;
    public float runSpeed = 2f;

    public float jumpHeight = 1f;
    public Vector3 jumpEffectRecoil = new Vector3(0f, 7f, 0f);
    public Vector3 jumpEffectRotation = new Vector3(7f, 0f, 0f);
    public float jumpEffectFactor = 7f;
        
    [Space] [Header("Ground Data")] [Tooltip("Changes distance from initial check point to the ground check point")]
    public float groundDistance = 0.4f;

    public Transform ceilingCheck;    
    public Transform groundCheck;
    public LayerMask groundMask;

    // HeadBob ref
    [Space] [Header("HeadBob Data")] [Tooltip("Changes the way HeadBob behaves")]
    public float idleXIntensity = 0.03f;
    public float idleYIntensity = 0.03f;
    public float walkXIntensity = 0.07f;
    public float walkYIntensity = 0.14f;
    public float runXIntensity = 0.14f;
    public float runYIntensity = 0.21f;
    public float crouchXIntensity = 0.07f;
    public float crouchYIntensity = 0.14f;
    public Vector3 runHeadBobOffset = new Vector3(0f, 0f, 0.17f);
        
    public float idleFactorA = 1f;
    public float idleFactorB = 5f;
    public float walkFactorA = 5f;
    public float walkFactorB = 7f;
    public float runFactorA = 7f;
    public float runFactorB = 10f;
    public float crouchFactorA = 5f;
    public float crouchFactorB = 7f;

    [Space] [Header("Zoom Data")]
    public float zoomFov = 14f;
    public float zoomFactor = 7f;

    //Run FOV
    [Space] [Header("Run FOV Data")] [Tooltip("Changes the FOV while running")]
    public float runFovMultiplier = 1.28f;

    [FormerlySerializedAs("currentFov")] [HideInInspector] public float initialFov;
    public float runFovFactor = 7f;
    
    [Space] [Header("Look Back Run Data")]
    public Vector3 lookBackRotation = new Vector3(0f, 160f, 0f);
    public float lookBackFactor = 35f;
        
    [Space] [Header("Crouch Data")]
    public Vector3 crouchValues = new Vector3(0f, 0.9f, -0.27f);
    public float crouchFactor = 7f;
    public float collisionOverlapRadius = 0.2f;
    
    [HeaderAttribute ("Animation Curve")] public AnimationCurve crouchCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    public float crouchTransitionDuration = 0.5f;
    
    public float crouchControllerHeight = 0.1f;
    [HideInInspector] public float initialControllerHeight;

    public Vector3 controllerCentreOffset = new Vector3(0f, 0.5f, 0f);
    [HideInInspector] public Vector3 initialControllerOffset;
    
    [Space] [Header("Peek Data")]
    public Vector3 peekPosition = new Vector3(0.37f, 0f, 0f);
    public Vector3 peekAngle = new Vector3(0f, 0f,14f);
    public float peekFactor = 7f;
    public float peekClampMin = -70f;
    public float peekClampMax = 70f;
    


    [Space] [Header("Camera Tilt")]
    public Vector3 cameraTiltPos = new Vector3(1.7f, 0f, 1.7f);
    public float cameraTiltFactor = 7f;
    

    [Space] [Header("External Forces")]
    public float gravity = -19.62f;
    public Vector3 drag = new Vector3(1f, 1f, 1f);

    [Space] [Header("References")]
    public Camera theCamera;
    public Animator playerAnimations;
    public AudioSource walkAudioSource;
    public AudioClip walkAudioClip;
    
    private Vector3 _cameraInitialPos;

    [HideInInspector] public Vector3 targetBobPosition;

    [HideInInspector] public CharacterController controller;

    public static readonly int WalkAnim = Animator.StringToHash("iWalk");
    public static readonly int RunAnim = Animator.StringToHash("iRun");
    public static readonly int CrouchAnim = Animator.StringToHash("iCrouch");
    public static readonly int BlendX = Animator.StringToHash("BlendX");
    public static readonly int BlendY = Animator.StringToHash("BlendY");

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        initialFov = theCamera.fieldOfView;
        
        initialControllerHeight = controller.height;
        initialControllerOffset = controller.center;

        _cameraInitialPos = theCamera.transform.localPosition;
        
        stateMachine = new StateMachine();
        idleState = new IdleState(this, stateMachine);
        walkState = new WalkState(this, stateMachine);
        runState = new RunState(this, stateMachine);
        jumpState = new JumpState(this, stateMachine);
        crouchState = new CrouchState(this, stateMachine);
        crouchWalkState = new CrouchWalkState(this, stateMachine);
        peekState = new PeekState(this, stateMachine);
        zoomState = new ZoomState(this, stateMachine);
        lookBackState = new LookBackState(this, stateMachine);
        //more states here

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();
        stateText.text = stateMachine.CurrentState.ToString();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    public Vector3 HeadBob(float graphValue, float xIntensity, float yIntensity)
    {
        targetBobPosition = _cameraInitialPos + new Vector3(Mathf.Cos(graphValue) * xIntensity,
            Mathf.Sin(graphValue * 2) * yIntensity, 0f);
        return targetBobPosition;
    }
}