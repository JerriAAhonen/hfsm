using UnityEngine;

namespace HierarchicalSM
{
	public class WalkState : StateMachine
	{
		protected override void OnEnter()
		{
			Debug.Log("Enter RunState");
			ctx.VerticalVel = ctx.GroundedGravity;
		}

		protected override bool OnUpdate()
		{
			//Debug.Log("Updating RunState");
			ctx.SetHorizontalVelocity(InputManager.Instance.MovementInput * ctx.WalkSpeed);

			bool hasInput = InputManager.Instance.HasMovementInput;
			bool runRequested = ctx.RunRequested;
			bool jumpRequested = ctx.JumpRequested;
			bool crouchRequested = ctx.CrouchRequested;

			if (runRequested)
			{
				SendTrigger(MovementStateTriggers.WalkToRun);
				return false;
			}

			if (jumpRequested)
			{
				SendTrigger(MovementStateTriggers.WalkToJump);
				return false;
			}

			if (crouchRequested)
			{
				SendTrigger(MovementStateTriggers.WalkToCrouch);
				return false;
			}

			return hasInput;
		}

		protected override void OnExit()
		{
			Debug.Log("Exit JumpState");
		}
	}
}