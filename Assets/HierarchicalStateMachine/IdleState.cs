using UnityEngine;

namespace HierarchicalSM
{
	public class IdleState : StateMachine
	{
		protected override void OnEnter()
		{
			Debug.Log("Enter Idle");
			ctx.SetHorizontalVelocity(Vector2.zero);
		}

		protected override bool OnUpdate()
		{
			//Debug.Log($"Has movementInput: {InputManager.Instance.HasMovementInput}, input: {InputManager.Instance.MovementInput}");
			return !InputManager.Instance.HasMovementInput;

		}

		protected override void OnExit()
		{
			Debug.Log("Exit Idle");
		}
	}
}