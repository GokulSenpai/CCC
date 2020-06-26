using Com.CompanyName.GameName;
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
        private float _smoothPercent = 0f;

        private Look _look;
        public override void Enter()
        {
            base.Enter();
            var transform = Player.theCamera.transform;
            _beforePeekPosition = transform.localPosition;
            _beforePeekRotation = transform.localRotation;
            _look = Player.gameObject.GetComponent<Look>();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            var playerTransformRotation = Player.gameObject.transform.rotation;
            _look.playerYRotation = Mathf.Clamp(_look.playerYRotation,
                playerTransformRotation.y + Player.mousePeekClampMin, playerTransformRotation.y + Player.mousePeekClampMax);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _smoothPercent = Player.peekCurve.Evaluate(Player.smoothPeekFactor);
            
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
                targetPeekRotation = Quaternion.identity;
                
                StateMachine.ChangeState(Player.idleState);
            }
            
            Player.theCamera.transform.localPosition = Vector3.LerpUnclamped(_beforePeekPosition,
                targetPeekPosition, _smoothPercent);
            
            Player.theCamera.transform.localRotation = Quaternion.SlerpUnclamped(_beforePeekRotation,
                _beforePeekRotation * targetPeekRotation, _smoothPercent);
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