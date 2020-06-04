using UnityEngine;

namespace Com.CompanyName.GameName
{
    public class WalkState : GroundedState
    {
        public WalkState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            
        }
        
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
            // If I am idle, play idle animation. Else, play Walk animations
            if (IamIdle)
            {
                Player.playerAnimations.SetBool(Player.WalkAnim, false);
                
                // Idle HeadBob
                TargetBobPosition = Player.HeadBob(HeadBobIdleCounter, Player.idleXIntensity, Player.idleYIntensity);
                HeadBobIdleCounter += Time.smoothDeltaTime * Player.idleFactorA;
                Player.theCamera.transform.localPosition = Vector3.Lerp(Player.theCamera.transform.localPosition, TargetBobPosition, Time.smoothDeltaTime * Player.idleFactorB);
            }
            else
            {
                Player.playerAnimations.SetBool(Player.WalkAnim, true);
                // Blend X for left and right strafe animations blend
                Player.playerAnimations.SetFloat(Player.BlendX, HorizontalInput);
                // Blend Y for forward and backward animations blend
                Player.playerAnimations.SetFloat(Player.BlendY, VerticalInput);
                
                // Walk HeadBob
                TargetBobPosition = Player.HeadBob(HeadBobMovementCounter, Player.walkXIntensity, Player.walkYIntensity);
                HeadBobMovementCounter += Time.smoothDeltaTime * Player.walkFactorA;
                Player.theCamera.transform.localPosition = Vector3.Lerp(Player.theCamera.transform.localPosition, TargetBobPosition, Time.smoothDeltaTime * Player.walkFactorB);
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
        }
    }
}