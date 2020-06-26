using System.Collections;
using UnityEngine;

namespace State_Machine.States
{
    public class CrouchState : IdleState 
    {
        public CrouchState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }
        
        private Vector3 _initialPosition;

        private Vector3 _beforeCrouchPosition;
        private float _smoothCrouch = 0f;

        private CrouchState _instance;

        private IEnumerator _crouchPlayer;

        public override void Enter()
        {
            base.Enter();
            Player.playerAnimations.SetBool(Player.CrouchAnim, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _initialPosition = Player.theCamera.transform.localPosition;
            
            _smoothCrouch = Player.crouchCurve.Evaluate(Player.smoothCrouchFactor);

            Player.controller.height = Player.crouchControllerHeight;
            Player.controller.center = Player.controllerCentreOffset;
            
                Player.theCamera.transform.localPosition = Vector3.LerpUnclamped(_initialPosition,
                _initialPosition - Player.crouchValues, _smoothCrouch);

            if (!IamIdle)
            {
                StateMachine.ChangeState(Player.crouchWalkState);
            }
            
            
            if (!IWantToCrouch)
            {
                if (Physics.OverlapSphere(Player.ceilingCheck.transform.position, Player.collisionOverlapRadius,
                    Player.groundMask).Length > 0)
                {
                    IWantToCrouch = true; 
                }
                else
                {
                    Player.theCamera.transform.localPosition = Vector3.LerpUnclamped(Player.theCamera.transform.localPosition,
                        _initialPosition, _smoothCrouch);

                    Player.controller.height = Player.initialControllerHeight;
                    Player.controller.center = Player.initialControllerOffset;
                    
                    StateMachine.ChangeState(Player.idleState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            Player.playerAnimations.SetBool(Player.CrouchAnim, false);
        }
    }
}