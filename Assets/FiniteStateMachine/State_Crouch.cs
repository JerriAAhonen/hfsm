using UnityEngine;

namespace FiniteStateMachine
{
	public class State_Crouch : StateBase
	{
		public State_Crouch(PlayerMovement context) : base(context) { }

		public override bool CanEnter()
		{
			return ctx.CrouchRequested;
		}

		public override void OnEnter()
		{
			Debug.Log("Enter crouch");
		}

		public override bool OnUpdate()
		{
			ctx.SetHorizontalVelocity(InputManager.Instance.MovementInput * ctx.CrouchSpeed);

			bool doContinue = ctx.Crouching;
			if (!doContinue) Debug.Log("Exit crouch");
			return doContinue;
		}
	}
}