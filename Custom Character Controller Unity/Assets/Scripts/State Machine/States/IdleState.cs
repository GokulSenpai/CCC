using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Xsl;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace State_Machine.States
{
    public class IdleState : State
    {
        protected float HorizontalInput;
        protected float VerticalInput;

        protected Vector3 MoveDirection;
        protected Vector3 Velocity;

        private bool _iamGrounded;
        private bool _runningInput;
        private bool _rightMouseZoomInput;
        private bool _jumpingInput;
        private bool _crouchInput;
        private bool _peekLeftInput;
        private bool _peekRightInput;
        
        
        protected bool IamIdle;
        protected bool IAmGrounded;
        protected bool IWantToWalk;
        protected bool IWantToRun;
        protected bool IWantToJump;
        protected bool IWantToCrouch;
        protected bool IWantToPeekLeft;
        protected bool IWantToPeekRight;
        protected bool IWantToPeek;
        protected bool IWantToZoom;
        protected bool FovCheck;
        protected bool IWantToPlayFootsteps;

        private float _headBobIdleCounter;

        protected float Speed;

        protected Vector3 TargetBobPosition;
        

        public IdleState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.playerAnimations.SetBool(Player.RunAnim, false);
        }

        public override void HandleInput()
        {
            base.HandleInput();

            // Input Axes
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetAxis("Vertical");

            _iamGrounded = Physics.CheckSphere(Player.groundCheck.position, Player.groundDistance, Player.groundMask) && Velocity.y < 0;
            _runningInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            _jumpingInput = Input.GetButtonDown("Jump");
            _crouchInput = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            _peekLeftInput = Input.GetKey(KeyCode.Q);
            _peekRightInput = Input.GetKey(KeyCode.E);
            _rightMouseZoomInput = Input.GetKey(KeyCode.Mouse1);
            
            MoveDirection = new Vector3(HorizontalInput, 0, VerticalInput).normalized;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            IAmGrounded = _iamGrounded;
            IamIdle = HorizontalInput.Equals(0) && VerticalInput.Equals(0) && _iamGrounded;
            IWantToWalk = !IamIdle && !IWantToZoom && _iamGrounded;
            IWantToRun = _runningInput && VerticalInput > 0f && _iamGrounded;
            IWantToZoom = _rightMouseZoomInput && _iamGrounded && IamIdle && !IWantToCrouch && !IWantToPeek;
            IWantToJump = _jumpingInput && _iamGrounded;
            IWantToCrouch = _crouchInput && _iamGrounded;
            IWantToPeekLeft = _peekLeftInput && _iamGrounded && !IWantToWalk && !IWantToZoom;
            IWantToPeekRight = _peekRightInput && _iamGrounded && !IWantToWalk && !IWantToZoom;
            IWantToPeek = IWantToPeekLeft || IWantToPeekRight && IamIdle;
            FovCheck = Mathf.Round(Player.theCamera.fieldOfView).Equals(Player.initialFov);
            IWantToPlayFootsteps = _iamGrounded && !Player.walkAudioSource.isPlaying && !IamIdle;

            Player.playerAnimations.SetBool(Player.WalkAnim, false);
            
            // Idle HeadBob
            TargetBobPosition = Player.HeadBob(_headBobIdleCounter, Player.idleXIntensity, Player.idleYIntensity);
            _headBobIdleCounter += Time.smoothDeltaTime * Player.idleFactorA;
            Player.theCamera.transform.localPosition = Vector3.Lerp(Player.theCamera.transform.localPosition,
                TargetBobPosition, Time.smoothDeltaTime * Player.idleFactorB);

            // All the possible transition states from Idle
            TransitionsFromIdle();
        }

        private void TransitionsFromIdle()
        {
            // WalkState call
            if (IWantToWalk)
            {
                StateMachine.ChangeState(Player.walkState);
            }

            //JumpState call
            if (IWantToJump)
            {
                StateMachine.ChangeState(Player.jumpState);
            }

            //CrouchState call
            if (IWantToCrouch)
            {
                StateMachine.ChangeState(Player.crouchState);
            }

            if (IWantToPeek && FovCheck)
            {
                StateMachine.ChangeState(Player.peekState);
            }

            // Right Mouse Zoom
            if (IWantToZoom)
            {
                StateMachine.ChangeState(Player.zoomState);
            }

            // Applies all the external forces on the Player
            ExternalForces();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void ExternalForces()
        {
            // Applying Gravity
            Velocity.y += Player.gravity * Time.smoothDeltaTime;
            
            // Drag
            Velocity.x /= 1 + Player.drag.x * Time.smoothDeltaTime;
            Velocity.y /= 1 + Player.drag.y * Time.smoothDeltaTime;
            Velocity.z /= 1 + Player.drag.z * Time.smoothDeltaTime;
            
            // Reset Velocity on ground
            if (_iamGrounded)
            {
                Velocity.y = -2f;
            }
            
            // Applying all the forces on Movement
            Player.controller.Move(Velocity * Time.smoothDeltaTime);
        }

        protected void Footsteps()
        {
            if(IWantToPlayFootsteps)
            {
                Player.walkAudioSource.volume = Random.Range(0.8f, 1f);
                Player.walkAudioSource.pitch = Random.Range(0.8f, 1.1f);
                Player.walkAudioSource.PlayOneShot(Player.walkAudioClip);
            }
        }
    }
}