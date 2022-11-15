using UnityEngine;

namespace Game.Shared
{
	public class JumpState : BaseState
	{
		public JumpState(MovementStateMachine ctx) : base(ctx) { }

		public override void OnEnterState()
		{
			Debug.Log("Enter Jump");
			ctx.VerticalVel = Mathf.Sqrt(-2f * ctx.JumpHeight * ctx.Gravity);
		}

		public override void OnExitState()
		{
			Debug.Log("Exit Jump");
		}

		public override void OnUpdate()
		{
			ctx.VerticalVel += ctx.Gravity * Time.deltaTime;
			CheckSwitchState();
		}

		private void CheckSwitchState()
		{
			if (ctx.IsGrounded)
				ctx.SetCurrentState(States.Grounded);
		}
	}
}