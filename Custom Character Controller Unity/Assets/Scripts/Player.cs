using System;
using System.Runtime.CompilerServices;
using Com.CompanyName.GameName;
using State_Machine;
using State_Machine.States;
using UnityEngine;
using UnityEngine.UI;

// This script is attached to the Player
public class Player : MonoBehaviour
{
    [HideInInspector] public bool crouch;
    [HideInInspector] public bool headBob;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool peek;
    [HideInInspector] public bool run;
    [HideInInspector] public bool cameraTilt;
    [HideInInspector] public bool zoom;


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


    [Space] public float walkSpeed = 1f;
    [Space] public float runSpeed = 2f;
    
    [Space]  public float jumpHeight = 1f;
    [Space] public float jumpAirTime = 0.4f;
    [Space] public Vector3 jumpEffectRecoil = new Vector3(0f, 0.37f, 0f);
    public Vector3 walkJumpEffectRecoil = new Vector3(0f, 0.17f, 0f);
    public Vector3 runJumpEffectRecoil = new Vector3(0f, 0.17f, 0f);

    [Space] public AnimationCurve jumpRecoilCurve;
    public AnimationCurve walkJumpRecoilCurve;
    public AnimationCurve runJumpRecoilCurve;
    [Range(0,1)]
    public float smoothJumpRecoilFactor = 0.7f;
    
    
    [Tooltip("Changes distance from initial check point to the ground check point")]
    [Space] public float groundDistance = 0.4f;
    [Space] public Transform ceilingCheck;    
            public Transform groundCheck;
    [Space] public LayerMask groundMask;

    // HeadBob ref
    [Tooltip("Changes the way HeadBob behaves")]
    [Space] public float idleXIntensity = 0.03f;
    public float idleYIntensity = 0.03f;
    [Space] public float walkXIntensity = 0.07f;
    public float walkYIntensity = 0.14f;
    [Space]  public float runXIntensity = 0.14f;
    public float runYIntensity = 0.21f;
    [Space] public float crouchXIntensity = 0.07f;
    public float crouchYIntensity = 0.14f;
    [Space] public Vector3 runHeadBobOffset = new Vector3(0f, 0f, 0.17f);
    [Space]  public float idleFactorA = 1f;
    public float idleFactorB = 5f;
    [Space]  public float walkFactorA = 5f;
    public float walkFactorB = 7f;
    [Space]  public float runFactorA = 7f;
    public float runFactorB = 10f;
    [Space] public float crouchFactorA = 5f;
    public float crouchFactorB = 7f;
    
    [Space] public float zoomFov = 14f;
    [Space] public AnimationCurve zoomCurve;
    [Range(0,1)]
    public float smoothZoomFactor = 0.27f;
    

    //Run FOV
    [Tooltip("Changes the FOV while running")]
    [Space] public float runFovMultiplier = 1.28f;
    [Space] public float runFovFactor = 7f;
    [Space] public Vector3 lookBackRotation = new Vector3(0f, 120f, 0f);
    [Space] public AnimationCurve lookBackCurve;
    [Range(0,1)]
    public float smoothLookBackFactor = 0.07f;
    
    [HideInInspector] public float initialFov;
        
    
    [Space] public Vector3 crouchValues = new Vector3(0f, 0.9f, -0.27f);
    [Space] public float collisionOverlapRadius = 0.2f;
    [Space] public AnimationCurve crouchCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    [Range(0,1)]
            public float smoothCrouchFactor = 0.6f;
    [Space] public float crouchControllerHeight = 0.1f;
            public Vector3 controllerCentreOffset = new Vector3(0f, 0.2f, 0f);
    [HideInInspector] public float initialControllerHeight;
    [HideInInspector] public Vector3 initialControllerOffset;
    
    
    
    [Space] public Vector3 peekPosition = new Vector3(0.37f, 0f, 0f);
    public Vector3 peekAngle = new Vector3(0f, 0f,14f);
    [Space] public AnimationCurve peekCurve;
    [Range(0,1)]
    public float smoothPeekFactor = 0.7f;
    [Space] public float mousePeekClampMin = -21f;
    public float mousePeekClampMax = 21f;
    
    
    [Space] public Vector3 cameraTiltPos = new Vector3(1.7f, 0f, 1.7f);
    [Space] public float cameraTiltFactor = 7f;
    
    
    [Space] public float gravity = -19.62f;
    [Space] public Vector3 drag = new Vector3(1f, 1f, 1f);
    
    [Space] public float mouseClampMin = -70f;
            public float mouseClampMax = 70f;
    [Space] public Camera theCamera;
    [Space] public Animator playerAnimations;
    [Space] public AudioSource walkAudioSource;
            public AudioClip walkAudioClip;
    
    private Vector3 _cameraInitialPos;

    [HideInInspector] public Vector3 targetBobPosition;

    [HideInInspector] public CharacterController controller;
    
    [HideInInspector]
    public int toolBar;
    public string currentTab;

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