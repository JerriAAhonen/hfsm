using UnityEngine;

namespace HierarchicalSM
{
	public class RunState : StateMachine
	{
		protected override void OnEnter()
		{
			Debug.Log("Enter RunState");
			ctx.VerticalVel = ctx.GroundedGravity;
		}

		protected override bool OnUpdate()
		{
			ctx.SetHorizontalVelocity(InputManager.Instance.MovementInput * ctx.RunSpeed);

			bool hasInput = InputManager.Instance.HasMovementInput;
			bool walkRequested = !ctx.RunRequested;
			bool jumpRequested = ctx.JumpRequested;
			bool crouchRequested = ctx.CrouchRequested;

			if (walkRequested)
			{
				SendTrigger(MovementStateTriggers.RunToWalk);
				ctx.RunRequested = false;
				return false;
			}

			if (jumpRequested)
			{
				SendTrigger(MovementStateTriggers.RunToJump);
				return false;
			}

			if (crouchRequested)
			{
				SendTrigger(MovementStateTriggers.RunToCrouch);
				ctx.RunRequested = false;
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