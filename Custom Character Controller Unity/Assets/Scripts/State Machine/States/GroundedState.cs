using UnityEngine;

namespace Com.CompanyName.GameName
{
    public class GroundedState : State
    {
        protected float HorizontalInput;
        protected float VerticalInput;

        protected Vector3 MoveDirection;
        private Vector3 _velocity;

        protected bool IamIdle;
        protected bool IamGrounded;
        
        protected float HeadBobIdleCounter;
        protected float HeadBobMovementCounter;

        protected float Speed;
        
        protected Vector3 TargetBobPosition;

        protected GroundedState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            Speed = Player.walkSpeed;
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Input Axes
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetAxis("Vertical");
            
            MoveDirection = new Vector3(HorizontalInput, 0, VerticalInput).normalized;
            
            IamIdle = HorizontalInput.Equals(0) && VerticalInput.Equals(0);
            IamGrounded = Physics.CheckSphere(Player.groundCheck.position, Player.groundDistance, Player.groundMask);
            
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}