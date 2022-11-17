namespace FiniteStateMachine
{
	public abstract class StateBase
	{
		protected PlayerMovement ctx;
		public StateBase(PlayerMovement context) => ctx = context;
		public abstract bool CanEnter();
		public abstract void OnEnter();
		public abstract bool OnUpdate();
	}
}