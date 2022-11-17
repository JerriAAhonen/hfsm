﻿using UnityEngine;

namespace FiniteStateMachine
{
	public class WalkState : StateBase
	{
		public WalkState(PlayerMovement ctx) : base(ctx) { }
		
		public override bool CanEnter()
		{
			return InputManager.Instance.HasMovementInput;
		}

		public override void OnEnter()
		{
			Debug.Log("Enter walk");
		}

		public override bool OnUpdate()
		{
			ctx.SetHorizontalVelocity(InputManager.Instance.MovementInput * ctx.WalkSpeed);
			
			bool doContinue = InputManager.Instance.HasMovementInput;
			if (!doContinue) Debug.Log("Exit Walk");
			return doContinue;
		}
	}
}