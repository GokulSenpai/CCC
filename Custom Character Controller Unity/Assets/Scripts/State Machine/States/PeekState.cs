using UnityEngine;

namespace State_Machine.States
{
    public class PeekState : IdleState
    {
        public PeekState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }
        
        private Vector3 _beforePeekPosition;
        private Quaternion _beforePeekRotation;
        
        public override void Enter()
        {
            base.Enter();
            var transform = Player.theCamera.transform;
            _beforePeekPosition = transform.localPosition;
            _beforePeekRotation = transform.localRotation;
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Vector3 targetPeekPosition;
            Quaternion targetPeekRotation;
            
            if (IWantToPeekLeft)
            {
                targetPeekPosition = _beforePeekPosition - Player.peekPosition;
                targetPeekRotation = Quaternion.Euler(Player.peekAngle);
            }
            else if (IWantToPeekRight)
            {
                targetPeekPosition = _beforePeekPosition + Player.peekPosition;
                targetPeekRotation = Quaternion.Inverse(Quaternion.Euler(Player.peekAngle));
            }
            else
            {
                var transform = Player.theCamera.transform;
                targetPeekPosition = _beforePeekPosition;
                targetPeekRotation = _beforePeekRotation;
                
                StateMachine.ChangeState(Player.idleState);
            }
            
            Player.theCamera.transform.localPosition = Vector3.Lerp(_beforePeekPosition,
                targetPeekPosition, Player.peekFactor * Time.smoothDeltaTime);
            Player.theCamera.transform.localRotation = Quaternion.Slerp(_beforePeekRotation,
                _beforePeekRotation * targetPeekRotation, Player.peekFactor * Time.smoothDeltaTime);
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