using Com.CompanyName.GameName;
using UnityEngine;

namespace State_Machine.States
{
    public class RunState : WalkState
    {
        public RunState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }

        private float _headBobRunCounter;
        protected bool LookBackLeftInput;
        protected bool LookBackRightInput;


        public override void Enter()
        {
            base.Enter();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            
            LookBackLeftInput = Input.GetKey(KeyCode.Q);
            LookBackRightInput = Input.GetKey(KeyCode.E);
        }

        public override void LogicUpdate()
        {  
            base.LogicUpdate();
            
            Player.playerAnimations.SetBool(Player.RunAnim, true);
            
            Player.theCamera.fieldOfView = Mathf.Lerp(Player.theCamera.fieldOfView,
                Player.initialFov * Player.runFovMultiplier, Time.smoothDeltaTime * Player.runFovFactor);
            
            Speed = Player.runSpeed;

            // Run HeadBob
            TargetBobPosition = Player.HeadBob(_headBobRunCounter, Player.runXIntensity, Player.runYIntensity);
            _headBobRunCounter += Time.smoothDeltaTime * Player.runFactorA;
            Player.theCamera.transform.localPosition = Vector3.Lerp(Player.theCamera.transform.localPosition,
                TargetBobPosition + Player.runHeadBobOffset, Time.smoothDeltaTime * Player.runFactorB);

            if (LookBackLeftInput || LookBackRightInput)
            {
                StateMachine.ChangeState(Player.lookBackState);
            }
            
            // if (TargetBobPosition.y < 1.6f)
            // {
            //     Footsteps();
            // }

            if (!IWantToRun)
            {
                Player.playerAnimations.SetBool(Player.RunAnim, false);
                Speed = Player.walkSpeed;
                
                StateMachine.ChangeState(Player.walkState);
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