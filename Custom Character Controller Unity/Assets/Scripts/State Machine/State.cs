namespace Com.CompanyName.GameName
{
    public abstract class State
    {
        protected readonly Player Player;
        private StateMachine _stateMachine;
        
        protected State(Player player, StateMachine stateMachine)
        {
            this.Player = player;
            this._stateMachine = stateMachine;
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