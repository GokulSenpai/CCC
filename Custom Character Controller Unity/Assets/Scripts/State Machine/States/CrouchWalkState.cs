using UnityEngine;

namespace State_Machine.States
{
    public class CrouchWalkState : CrouchState

    {
        public CrouchWalkState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }
        
        private float _headBobCrouchCounter;

        public override void Enter()
        {
            base.Enter();
            Player.playerAnimations.SetBool(Player.WalkAnim, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Crouch Walk HeadBob
            TargetBobPosition = Player.HeadBob(_headBobCrouchCounter, Player.crouchXIntensity, Player.crouchYIntensity);
            _headBobCrouchCounter += Time.smoothDeltaTime * Player.crouchFactorA;
            Player.theCamera.transform.localPosition = Vector3.Lerp(Player.theCamera.transform.localPosition,
                TargetBobPosition, Time.smoothDeltaTime * Player.crouchFactorB);

            if (IamIdle)
            {
                StateMachine.ChangeState(Player.crouchState);
            }
            
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Player.controller.Move(Player.transform.TransformDirection(MoveDirection) * (Time.smoothDeltaTime * Speed));
        }

        public override void Exit()
        {
            base.Exit();
            Player.playerAnimations.SetBool(Player.WalkAnim, false);
        }
    }
}