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
        
        private Vector3 _currentCameraPos;
        private float _smoothPercent = 0f;
        


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

            Speed = Player.runSpeed;

            Player.playerAnimations.SetBool(Player.RunAnim, true);

            _currentCameraPos = Player.theCamera.transform.localPosition;

            Player.theCamera.fieldOfView = Mathf.LerpUnclamped(Player.theCamera.fieldOfView,
                Player.initialFov * Player.runFovMultiplier, Time.smoothDeltaTime * Player.runFovFactor);

            if (Player.headBob)
            {
                // Run HeadBob
                TargetBobPosition = Player.HeadBob(_headBobRunCounter, Player.runXIntensity, Player.runYIntensity);
                _headBobRunCounter += Time.smoothDeltaTime * Player.runFactorA;
                Player.theCamera.transform.localPosition = Vector3.LerpUnclamped(Player.theCamera.transform.localPosition,
                    TargetBobPosition + Player.runHeadBobOffset, Time.smoothDeltaTime * Player.runFactorB);
            }
            
            if (LookBackLeftInput || LookBackRightInput)
            {
                StateMachine.ChangeState(Player.lookBackState);
            }
            
            if (AirTime > Player.jumpAirTime)
            {
                _smoothPercent = Player.runJumpRecoilCurve.Evaluate(Player.smoothJumpRecoilFactor);
                
                Player.theCamera.transform.localPosition = Vector3.Lerp(_currentCameraPos,
                    _currentCameraPos - Player.runJumpEffectRecoil, _smoothPercent); 
            }

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
            Speed = 0f;
        }
    }
}