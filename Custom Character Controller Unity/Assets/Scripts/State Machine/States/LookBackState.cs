using UnityEngine;

namespace State_Machine.States
{
    public class LookBackState : RunState
    {
        public LookBackState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            
        }

        private Quaternion _beforeLookBackRotation;

        public override void Enter()
        {
            base.Enter();
            _beforeLookBackRotation = Player.theCamera.transform.localRotation;
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {  
            base.LogicUpdate();
            
            if (LookBackLeftInput)
            {
                Player.theCamera.transform.localRotation = Quaternion.Slerp(_beforeLookBackRotation,
                    Quaternion.Inverse(Quaternion.Euler(Player.lookBackRotation)), Time.smoothDeltaTime * Player.lookBackFactor);
            } 
            if (LookBackRightInput)
            {
                Player.theCamera.transform.localRotation = Quaternion.Slerp(_beforeLookBackRotation,
                    Quaternion.Euler(Player.lookBackRotation), Time.smoothDeltaTime * Player.lookBackFactor);
            }
            if(!(LookBackLeftInput || LookBackRightInput))
            {
                Player.theCamera.transform.localRotation = Quaternion.Slerp(Player.theCamera.transform.localRotation,
                    _beforeLookBackRotation, Time.smoothDeltaTime * Player.lookBackFactor);
                
                StateMachine.ChangeState(Player.runState);
            }

            if (!IWantToRun)
            {
                StateMachine.ChangeState(Player.walkState);
            }
            
            if (IamIdle)
            {
                Player.theCamera.transform.localRotation = Quaternion.Slerp(Player.theCamera.transform.localRotation,
                    _beforeLookBackRotation, Time.smoothDeltaTime * Player.lookBackFactor);
                
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