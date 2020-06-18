using Com.CompanyName.GameName;
using UnityEngine;

namespace State_Machine.States
{
    public class WalkState : IdleState
    {
        public WalkState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
        }
        
        private float _headBobWalkCounter;
        private Quaternion _beforeTiltRotation;
        
        private Vector3 _cameraTiltHorizontal;
        private Vector3 _cameraTiltVertical;
        
        public override void Enter()
        {
            base.Enter();
            _beforeTiltRotation = Player.theCamera.transform.localRotation;
            _cameraTiltHorizontal = new Vector3(0f, 0f, Player.cameraTiltPos.z);
            _cameraTiltVertical = new Vector3(Player.cameraTiltPos.x, 0f, 0f);
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Player.playerAnimations.SetBool(Player.WalkAnim, true);
            // Blend X for left and right strafe animations blend
            Player.playerAnimations.SetFloat(Player.BlendX, HorizontalInput);
            // Blend Y for forward and backward animations blend
            Player.playerAnimations.SetFloat(Player.BlendY, VerticalInput);

            // Walk HeadBob
            TargetBobPosition = Player.HeadBob(_headBobWalkCounter, Player.walkXIntensity, Player.walkYIntensity);
            _headBobWalkCounter += Time.smoothDeltaTime * Player.walkFactorA;
            Player.theCamera.transform.localPosition = Vector3.Lerp(Player.theCamera.transform.localPosition,
                TargetBobPosition, Time.smoothDeltaTime * Player.walkFactorB);

            if (TargetBobPosition.x < 0 && !IWantToRun)
            {
                Footsteps();
            }
            
            Player.theCamera.fieldOfView = Mathf.Lerp(Player.theCamera.fieldOfView, Player.initialFov,
                Time.smoothDeltaTime * Player.runFovFactor);

            if (!IWantToRun && Player.cameraTilt)
            {
                // Camera Tilt
                CameraTiltWhileWalking();
            }

            // All the possible transition states from walk
            TransitionsFromWalk();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Player.controller.Move(Player.transform.TransformDirection(MoveDirection) * (Time.smoothDeltaTime * Speed));
        }
        
        private void CameraTiltWhileWalking()
        {
            Quaternion targetTiltRotation;
            
            if (HorizontalInput < 0)
            {
                targetTiltRotation = Quaternion.Euler(_cameraTiltHorizontal);
            }
            else if (HorizontalInput > 0)
            {
                targetTiltRotation = Quaternion.Inverse(Quaternion.Euler(_cameraTiltHorizontal));
            }
            else if (VerticalInput < 0)
            {
                targetTiltRotation = Quaternion.Inverse(Quaternion.Euler(_cameraTiltVertical));
            }
            else if (VerticalInput > 0)
            {
                targetTiltRotation = Quaternion.Euler(_cameraTiltVertical);
            }
            else
            {
                targetTiltRotation = _beforeTiltRotation;
            }

            if (targetTiltRotation != _beforeTiltRotation)
            {
                Player.theCamera.transform.localRotation = Quaternion.Slerp(_beforeTiltRotation,
                    _beforeTiltRotation * targetTiltRotation,
                    Player.cameraTiltFactor * Time.smoothDeltaTime);
            }
        }
        
        private void TransitionsFromWalk()
        {
            if (IWantToRun)
            {
                StateMachine.ChangeState(Player.runState);
            }

            if (IWantToJump)
            {
                StateMachine.ChangeState(Player.jumpState);
            }

            if (IWantToCrouch)
            {
                StateMachine.ChangeState(Player.crouchState);
            }

            if (IamIdle)
            {
                StateMachine.ChangeState(Player.idleState);
            }
        }


        public override void Exit()
        {
            base.Exit();
        }
       
    }
}