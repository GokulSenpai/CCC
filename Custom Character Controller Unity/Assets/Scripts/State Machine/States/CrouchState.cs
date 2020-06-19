﻿using UnityEngine;

namespace State_Machine.States
{
    public class CrouchState : IdleState 
    {
        public CrouchState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }
        
        private Vector3 _initialPosition;
        private float _graphValue;
        private float _speed;

        private Vector3 _beforeCrouchPosition;
        private float _smoothCrouch = 0f;

        private float _reqCent;

        public override void Enter()
        {
            base.Enter();

            _speed = 1f / Player.crouchTransitionDuration;
            Player.crouchCurve.postWrapMode = WrapMode.Clamp;
            
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

            _graphValue += Time.smoothDeltaTime * _speed;
            _smoothCrouch = Player.crouchCurve.Evaluate(_graphValue);

            Player.controller.height =
                Mathf.LerpUnclamped(Player.controller.height, Player.crouchControllerHeight, _smoothCrouch);

            _reqCent = Mathf.LerpUnclamped(Player.controller.center.y, Player.controllerCentreOffset.y, _smoothCrouch);
            
            Player.controller.center = new Vector3(0, _reqCent, 0);

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
                    Player.crouchCurve.postWrapMode = WrapMode.PingPong;
                    
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