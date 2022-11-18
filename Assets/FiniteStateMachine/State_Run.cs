using UnityEngine;

namespace FiniteStateMachine
{
	public class State_Run : StateBase
	{
		public State_Run(PlayerMovement context) : base(context) { }

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
			
			bool doContinue = InputManager.Instance.HasMovementInput;
			if (!doContinue) Debug.Log("Exit Run");
			return doContinue;
		}
	}
}