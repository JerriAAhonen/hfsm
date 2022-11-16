﻿using UnityEngine;

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
		bool hasInput = InputManager.Instance.HasMovementInput;
		bool runRequested = ctx.RunRequested;
		bool jumpRequested = ctx.JumpRequested;
		bool crouchRequested = ctx.CrouchRequested;

		return hasInput && !runRequested && !jumpRequested && !crouchRequested;
	}

	protected override void OnExit()
	{
		Debug.Log("Exit JumpState");
	}
}