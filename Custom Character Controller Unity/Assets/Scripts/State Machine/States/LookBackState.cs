using UnityEngine;

namespace State_Machine.States
{
    public class LookBackState : RunState
    {
        public LookBackState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            
        }

        private Quaternion _beforeLookBackRotation;
        private float _smoothPercent = 0f;

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

            _smoothPercent = Player.lookBackCurve.Evaluate(Player.smoothLookBackFactor);
            
            if (LookBackLeftInput)
            {
                Player.theCamera.transform.localRotation = Quaternion.Slerp(_beforeLookBackRotation,
                    Quaternion.Inverse(Quaternion.Euler(Player.lookBackRotation)), _smoothPercent);
                Player.gameObject.GetComponent<Look>().enabled = false;
            } 
            if (LookBackRightInput)
            {
                Player.theCamera.transform.localRotation = Quaternion.Slerp(_beforeLookBackRotation,
                    Quaternion.Euler(Player.lookBackRotation), _smoothPercent);
                Player.gameObject.GetComponent<Look>().enabled = false;
            }
            if(!(LookBackLeftInput || LookBackRightInput))
            {
                Player.theCamera.transform.localRotation = Quaternion.Slerp(Player.theCamera.transform.localRotation,
                    _beforeLookBackRotation, _smoothPercent);
                
                TransitionsFromLookBack();
            }
        }

        private void TransitionsFromLookBack()
        {
            StateMachine.ChangeState(IWantToWalk ? Player.runState : Player.walkState);

            if (IamIdle)
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
            Player.gameObject.GetComponent<Look>().enabled = true;
        }
    }
}