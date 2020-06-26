using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace State_Machine.States
{
    public class ZoomState : IdleState
    {
        public ZoomState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }

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
            
            _smoothPercent = Player.zoomCurve.Evaluate(Player.smoothZoomFactor);

            Player.theCamera.fieldOfView = Mathf.Lerp(Player.theCamera.fieldOfView,
                IWantToZoom ? Player.zoomFov : Player.initialFov, _smoothPercent);

            if (FovCheck && !IWantToZoom && !IWantToPeek)
            {
                StateMachine.ChangeState(Player.idleState);
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