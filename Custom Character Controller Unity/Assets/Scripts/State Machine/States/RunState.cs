using UnityEngine;

namespace Com.CompanyName.GameName
{
    public class RunState : GroundedState
    {
        public RunState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            
        }

        private bool _runningInput;
        private float _currentFov;
        
        public override void Enter()
        {
            base.Enter();
            _currentFov = Player.theCamera.fieldOfView;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            _runningInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            var iWantToRun = _runningInput && IamGrounded && VerticalInput > 0f;

            if (iWantToRun)
            {
                Player.playerAnimations.SetBool(Player.RunAnim, true);
                Player.theCamera.fieldOfView = Mathf.Lerp(Player.theCamera.fieldOfView, _currentFov * Player.runFovMultiplier, Time.smoothDeltaTime * Player.runFovFactor);
                Speed = Player.runSpeed;
            }
            else
            {
                Player.playerAnimations.SetBool(Player.RunAnim, false);
                Player.theCamera.fieldOfView = Mathf.Lerp(Player.theCamera.fieldOfView, _currentFov, Time.smoothDeltaTime * Player.runFovFactor);
                Speed = Player.walkSpeed;
            }
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