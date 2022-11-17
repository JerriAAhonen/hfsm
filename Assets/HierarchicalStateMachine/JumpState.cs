using UnityEngine;

namespace HierarchicalSM
{
	public class JumpState : StateMachine
	{
		private float groundCheckDelay;

		protected override void OnEnter()
		{
			Debug.Log("Enter JumpState");
			ctx.VerticalVel = Mathf.Sqrt(-2f * ctx.JumpHeight * ctx.Gravity);
			groundCheckDelay = 0.1f;
		}

		protected override bool OnUpdate()
		{
			//Debug.Log("Updating JumpState");
			ctx.VerticalVel += ctx.Gravity * Time.deltaTime;
			groundCheckDelay -= Time.deltaTime;

			return groundCheckDelay > 0 || !ctx.IsGrounded;
		}

		protected override void OnExit()
		{
			Debug.Log("Exit JumpState");
		}
	}
}