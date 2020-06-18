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
        private Quaternion _currentCameraRot;
        
        public override void Enter()
        {
            base.Enter();
            var transform = Player.theCamera.transform;
            _currentCameraPos = transform.localPosition;
            _currentCameraRot = transform.localRotation;
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (!IWantToJump)
            {
                TransitionsFromJump();
            }
            
            if (!_didIJumpOnce)
            {
                Velocity.y = Mathf.Sqrt(-2f * Player.gravity * Player.jumpHeight);
                
                _didIJumpOnce = true;
            }
            
            if (IAmGrounded)
            {
                Player.theCamera.transform.localPosition = Vector3.Lerp(_currentCameraPos,
                    _currentCameraPos - Player.jumpEffectRecoil, Player.jumpEffectFactor * Time.smoothDeltaTime);
                    
                Player.theCamera.transform.localRotation = Quaternion.Slerp(_currentCameraRot,
                    _currentCameraRot * Quaternion.Euler(Player.jumpEffectRotation),
                    Player.jumpEffectFactor * Time.smoothDeltaTime);
                    
                Player.theCamera.transform.localRotation = Quaternion.Slerp(_currentCameraRot,
                    _currentCameraRot * Quaternion.Inverse(Quaternion.Euler(Player.jumpEffectRotation)),
                    Player.jumpEffectFactor * Time.smoothDeltaTime);
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