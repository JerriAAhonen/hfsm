using UnityEngine;

namespace FiniteStateMachine
{
	public class State_Idle : StateBase
	{
		public State_Idle(PlayerMovement ctx) : base(ctx) { }
		
		public override bool CanEnter()
		{
			return true;
		}

		public override void OnEnter()
		{
			Debug.Log("Enter idle");
			ctx.VerticalVel = ctx.GroundedGravity;
			
			// Reset context
			ctx.Running = false;
		}

		public override bool OnUpdate()
		{
			bool doContinue = !InputManager.Instance.HasMovementInput;
			if (!doContinue) Debug.Log("Exit Idle");
			return doContinue;
		}
	}
}