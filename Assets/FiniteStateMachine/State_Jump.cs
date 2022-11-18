using UnityEngine;

namespace FiniteStateMachine
{
	public class State_Jump : StateBase
	{
		private float groundCheckDelay;
		
		public State_Jump(PlayerMovement context) : base(context) { }

		public override bool CanEnter()
		{
			return ctx.IsGrounded && ctx.JumpRequested;
		}

		public override void OnEnter()
		{
			Debug.Log("Enter Jump");
			ctx.VerticalVel = Mathf.Sqrt(-2f * ctx.JumpHeight * ctx.Gravity);
			groundCheckDelay = 0.1f;
		}

		public override bool OnUpdate()
		{
			ctx.VerticalVel += ctx.Gravity * Time.deltaTime;
			
			groundCheckDelay -= Time.deltaTime;
			if (groundCheckDelay <= 0)
			{
				if (ctx.IsGrounded) Debug.Log("Exit Jump");
				return !ctx.IsGrounded;
			}

			return true;
		}
	}
}