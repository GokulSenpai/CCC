using UnityEngine;

namespace State_Machine.States
{
    public class JumpState : IdleState
    {
        public JumpState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }

        private bool _didIJumpOnce = false;
        private Vector3 _currentCameraPos;
        private float _smoothPercent = 0f;

        public override void Enter()
        {
            base.Enter();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _currentCameraPos = Player.theCamera.transform.localPosition;

            _smoothPercent = Player.jumpRecoilCurve.Evaluate(Player.smoothJumpRecoilFactor);
            
            
            if (!IWantToJump)
            {
                TransitionsFromJump();
            }
            

            if (!_didIJumpOnce)
            {
                Velocity.y = Mathf.Sqrt(-2f * Player.gravity * Player.jumpHeight);
                
                _didIJumpOnce = true;
            }

            if (_didIJumpOnce && IAmGrounded)
            {
                Player.theCamera.transform.localPosition = Vector3.Lerp(_currentCameraPos,
                    _currentCameraPos - Player.jumpEffectRecoil, _smoothPercent);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private void TransitionsFromJump()
        {
            if (IamIdle)
            {
                StateMachine.ChangeState(Player.idleState);
            }
            
            if (IWantToWalk)
            {
                StateMachine.ChangeState(Player.walkState);
            }

            if (IWantToRun)
            {
                StateMachine.ChangeState(Player.runState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _didIJumpOnce = false;
        }
    }
}