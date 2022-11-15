using UnityEngine;

namespace Game.Shared
{
	public abstract class BaseState
	{
		protected readonly MovementStateMachine ctx;
		protected BaseState(MovementStateMachine ctx) => this.ctx = ctx;
		public abstract void OnEnterState();
		public abstract void OnExitState();
		public abstract void OnUpdate();
	}
}