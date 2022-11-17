using UnityEngine;

namespace FiniteStateMachine
{
	public class IdleState : StateBase
	{
		public IdleState(PlayerMovement ctx) : base(ctx) { }
		
		public override bool CanEnter()
		{
			return true;
		}

		public override void OnEnter()
		{
			Debug.Log("Enter idle");
		}

		public override bool OnUpdate()
		{
			bool doContinue = !InputManager.Instance.HasMovementInput;
			if (!doContinue) Debug.Log("Exit Idle");
			return doContinue;
		}
	}
}