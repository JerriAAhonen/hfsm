using UnityEngine;

namespace HierarchicalSM
{
	public class CrouchState : StateMachine
	{
		protected override void OnEnter()
		{
			Debug.Log("Enter CrouchState");
		}

		protected override bool OnUpdate()
		{
			bool hasInput = InputManager.Instance.HasMovementInput;
			bool runRequested = ctx.RunRequested;
			bool crouchRequested = ctx.CrouchRequested;

			if (!crouchRequested && !runRequested && hasInput)
			{
				SendTrigger(MovementStateTriggers.CrouchToWalk);
				ctx.CrouchRequested = false;
				return false;
			}

			if (runRequested)
			{
				SendTrigger(MovementStateTriggers.CrouchToRun);
				ctx.CrouchRequested = false;
				return false;
			}

			if (crouchRequested)
			{
				SendTrigger(MovementStateTriggers.WalkToCrouch);
				return false;
			}

			return true;
		}

		protected override void OnExit()
		{
			Debug.Log("Exit CrouchState");
		}
	}
}