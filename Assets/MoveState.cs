using UnityEngine;

public class MoveState : StateMachine
{
	protected override void OnEnter()
	{
		Debug.Log("Enter MoveState");
	}

	protected override bool OnUpdate()
	{
		return InputManager.Instance.HasMovementInput;
	}

	protected override void OnExit()
	{
		Debug.Log("Exit MoveState");
	}
}