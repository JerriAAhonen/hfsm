using UnityEngine;

namespace Game.Shared
{
	public class GroundedState : BaseState
	{
		public GroundedState(MovementStateMachine ctx) : base(ctx) { }
		
		public override void OnEnterState()
		{
			Debug.Log("Enter Grounded");
			ctx.VerticalVel = ctx.GroundedGravity;
		}

		public override void OnExitState()
		{
			Debug.Log("Exit Grounded");
		}

		public override void OnUpdate()
		{
			CheckSwitchState();
		}

		private void CheckSwitchState()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				ctx.SetCurrentState(States.Jump);
		}
	}
}