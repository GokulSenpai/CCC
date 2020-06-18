using Com.CompanyName.GameName;

namespace State_Machine
{
    public abstract class State
    {
        protected readonly Player Player;
        protected readonly StateMachine StateMachine;

        protected State(Player player, StateMachine stateMachine)
        {
            this.Player = player;
            this.StateMachine = stateMachine;
        }

        public virtual void Enter()
        {
        }

        public virtual void HandleInput()
        {
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void Exit()
        {
        }
    }
}