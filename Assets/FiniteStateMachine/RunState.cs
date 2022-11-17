using UnityEngine;

namespace FiniteStateMachine
{
	public class RunState : StateBase
	{
		public RunState(PlayerMovement context) : base(context) { }

		public override bool CanEnter()
		{
			return InputManager.Instance.HasMovementInput && ctx.RunRequested;
		}

		public override void OnEnter()
		{
			Debug.Log("Enter run");
		}

		public override bool OnUpdate()
		{
			ctx.SetHorizontalVelocity(InputManager.Instance.MovementInput * ctx.RunSpeed);
			
			bool doContinue = InputManager.Instance.HasMovementInput && ctx.RunRequested;
			if (!doContinue) Debug.Log("Exit Run");
			return doContinue;
		}
	}
}