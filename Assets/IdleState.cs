using UnityEngine;

public class IdleState : StateMachine
{
	protected override void OnEnter() { }

	protected override bool OnUpdate()
	{
		return !InputManager.Instance.HasMovementInput;

	}
	protected override void OnExit() { }
}